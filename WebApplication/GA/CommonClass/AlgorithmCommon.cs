using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using WebApplication.BasicClasses;
using WebApplication.GA.CommonClass.GA;
using Machine = WebApplication.GA.CommonClass.GA.Machine;

namespace WebApplication.GA
{
    class AlgorithmCommon
    {
        public Dictionary<int, List<double>> macnineRunningNumsDict = new Dictionary<int, List<double>>();
        public List<List<double>> line = new List<List<double>>();
        public int MSpinnerNum;     //细纱机数目
        public Machine[,] produceLineMessage;
        public Machine[] spinnerCurrentMessage;


        public AlgorithmCommon(List<CommonClass.Produce.Batch> b)
        {
            for (int i = 0; i < b.Count; i++)
            {
                List<double> macnineRunningNums = ToolUtil.getMachineRunningNum(b[i].BatchDetail.Weight, b[i].BatchDetail.Id);
                macnineRunningNumsDict.Add(i, macnineRunningNums);
            }
            line = getRandomProduceLine();
        }

        /// <summary>
        /// 随机获取种群
        /// </summary>
        /// <param name="b">批次信息</param>
        public Population getPopulation(List<CommonClass.Produce.Batch> b)
        {
            int batchNum = b.Count;
            Population p = new Population();
            p.Gene = new List<GenePart>();
            p.RandomLine = line;
            p.IsReasonable = true;
            p.ChangeTimes = 0;
            for (int i = 0; i < batchNum; i++)
            {
                GenePart temp = new GenePart();
                temp.Id = i;
                temp.LineSelect = getRandom(0, line.Count - 1);
                p.Gene.Add(temp);
            }
            //计算种群的适应度函数
            p.Cost = fitness(p, b);
            return p;
        }

        /// <summary>
        /// 计算适应度函数（生产总时间）
        /// </summary>
        /// <param name="x">种群信息</param>
        /// <param name="b">批次信息(自带优先级)</param>
        /// <returns></returns>
        public double fitness(Population x, List<CommonClass.Produce.Batch> b)
        {
            Machine[,] produceLineMessage_copy = Clone(produceLineMessage);
            Machine[] spinnerCurrentMessage_copy = Clone(spinnerCurrentMessage);

            double time = 0;
            for (int batchNo = 0; batchNo < b.Count; batchNo++)
            {
                int produceType = b[batchNo].BatchDetail.Id;    //本次生产产品类型
                int lineSelect = x.Gene[batchNo].LineSelect;

                ProduceMessage message = new ProduceMessage();
                message.BeginTime = new List<double>();
                message.EndTime = new List<double>();
                message.DuringTime = new List<double>();
                message.InTimes = new List<int>();
                message.OutTimes = new List<int>();
                List<double> macnineRunningNums = macnineRunningNumsDict[batchNo];  //每个工序运行次数

                //7道工序
                //前6道工序
                for (int i = 0; i < 6; i++)
                {
                    message.InTimes.Add((int)Math.Ceiling(macnineRunningNums[i] / line[lineSelect][i]));
                    message.OutTimes.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][i].DoffingTimes);
                    message.DuringTime.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][i].RunTime);

                    if (produceLineMessage_copy[lineSelect, i].Param.Type == 0)
                        produceLineMessage_copy[lineSelect, i].Param.Type = produceType;

                    if (i == 0)
                    {
                        message.BeginTime.Add(produceLineMessage_copy[lineSelect, i].Param.EndTime
                            + ToolUtil.changeTime[i][produceType - 1][produceLineMessage_copy[lineSelect, i].Param.Type - 1]);
                        message.EndTime.Add(message.BeginTime.Last() + message.DuringTime.Last());
                    }
                    else
                    {
                        int t = (int)Math.Ceiling((line[lineSelect][i] * ToolUtil.productCapacityDict[produceType][i].InputNum)
                            / (line[lineSelect][i - 1] * ToolUtil.productCapacityDict[produceType][i - 1].OutputNum));

                        double temp = message.BeginTime.Last() + t * ToolUtil.productCapacityDict[produceType][i - 1].RunTime
                            > produceLineMessage_copy[lineSelect, i].Param.EndTime
                            ? message.BeginTime.Last() + t * ToolUtil.productCapacityDict[produceType][i - 1].RunTime :
                            produceLineMessage_copy[lineSelect, i].Param.EndTime;

                        if (message.EndTime.Last() + ToolUtil.productCapacityDict[produceType][i].RunTime >
                            temp + message.DuringTime.Last())
                        {
                            message.EndTime.Add(message.EndTime.Last() + ToolUtil.productCapacityDict[produceType][i].RunTime);
                        }
                        else
                        {
                            message.EndTime.Add(temp + message.DuringTime.Last());
                        }
                        message.BeginTime.Add(message.EndTime.Last() - message.DuringTime.Last());
                    }

                    produceLineMessage_copy[lineSelect, i].Param.Type = produceType;
                    produceLineMessage_copy[lineSelect, i].Param.BeginTime = message.BeginTime.Last();
                    produceLineMessage_copy[lineSelect, i].Param.EndTime = message.EndTime.Last();
                }

                //第7道工序begin
                int num = x.Gene[batchNo].SpinnerSelect.Count;  //选取细纱机台数
                message.InTimes.Add((int)Math.Ceiling(macnineRunningNums[6] / num));
                message.OutTimes.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][6].DoffingTimes);
                message.DuringTime.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][6].RunTime);

                double endTimeTemp = spinnerCurrentMessage_copy.Where(o=>x.Gene[batchNo].SpinnerSelect.Contains(o.Id)).Max(o => o.Param.EndTime);
                int t_7 = (int)Math.Ceiling((num * ToolUtil.productCapacityDict[produceType][6].InputNum)
                    / (line[lineSelect][5] * ToolUtil.productCapacityDict[produceType][5].OutputNum));
                double temp_7 = message.BeginTime.Last() + t_7 * ToolUtil.productCapacityDict[produceType][5].RunTime
                    > endTimeTemp
                    ? message.BeginTime.Last() + t_7 * ToolUtil.productCapacityDict[produceType][5].RunTime :
                    endTimeTemp;

                if (message.EndTime.Last() + ToolUtil.productCapacityDict[produceType][6].RunTime >
                            temp_7 + message.DuringTime.Last())
                {
                    message.EndTime.Add(message.EndTime.Last() + ToolUtil.productCapacityDict[produceType][6].RunTime);
                }
                else
                {
                    message.EndTime.Add(temp_7 + message.DuringTime.Last());
                }
                message.BeginTime.Add(message.EndTime.Last() - message.DuringTime.Last());
                //需要切换设备，切换次数+1
                foreach (var k in x.Gene[batchNo].SpinnerSelect)
                {
                    if (produceType != spinnerCurrentMessage_copy[k].Param.Type)
                        x.ChangeTimes++;
                    spinnerCurrentMessage_copy[k].Param.Type = produceType;
                    spinnerCurrentMessage_copy[k].Param.BeginTime = message.BeginTime.Last();
                    spinnerCurrentMessage_copy[k].Param.EndTime = message.EndTime.Last();
                }
                //第7道工序end

                if (message.EndTime.Max() > (b[batchNo].BatchDetail.Deadline - DateTime.Now).TotalMinutes)
                    x.IsReasonable = false;
                time = time > message.EndTime.Max() ? time : message.EndTime.Max();
                x.Gene[batchNo].Message = message;
            }
            return time;
        }

        /// <summary>
        /// 取得随机数
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int getRandom(int begin, int end)
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            Random rd = new Random(iSeed);
            return rd.Next(begin, end + 1);
        }

        /// <summary>
        /// 根据布局图，获取随机生产线
        /// </summary>
        /// <returns></returns>
        public List<List<double>> getRandomProduceLine()
        {
            List<List<double>> randomLine = new List<List<double>>();
            List<List<double>> nowConditionList = ToolUtil.getNowConditions();
            DataSet dataSet_1 = Util.ExcelToDS(MainDeal.path + "2生产线配置表模板.xls", 1);
            int rowlength = dataSet_1.Tables[0].Rows.Count;
            for (int i = 0; i < rowlength; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (j == 0)
                        randomLine.Add(new List<double>());
                    randomLine.Last().Add(Convert.ToDouble(dataSet_1.Tables[0].Rows[i].ItemArray[j + 1]));
                }
            }

            DataSet dataSet_2 = Util.ExcelToDS(MainDeal.path + "1工厂机器数量配置表模板.xls", 1);
            MSpinnerNum = Convert.ToInt32(dataSet_2.Tables[0].Rows[0].ItemArray[7]);

            produceLineMessage = new Machine[randomLine.Count, 6];

            int[,] startmnum = new int[3, 6] { {0,0,0,0,0,0 },
                {12,4,2,12,5,5},
                {22,6,3,19,7,7 } };
            for (int i = 0; i < randomLine.Count; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Machine temp = new Machine
                    {
                        Id = j,
                        Param = new MachineParam
                        {
                            Type = 0,
                            BeginTime = 0,
                            EndTime = 0
                        },
                        //ChangeTimes = 0
                    };
                    //找到该生产线上的该工序机器中最长的预计时间
                    double maxtime = 0;
                    //获得该生产线中该工序机器数量
                    double mnum = randomLine[i][j];
                    //获得该生产线中机器在总的机器中的序号
                    for (int k = startmnum[i, j]; k < startmnum[i, j] + mnum; k++)
                    {
                        //获得机器序号后，得到其预计还需生产时间；经过不断比较，得到该生产线上该工序最多还应当生产多久
                        if (nowConditionList[j][k] > maxtime)
                            maxtime = nowConditionList[j][k];
                    }
                    temp.Param.EndTime += maxtime;
                    produceLineMessage[i, j] = temp;
                }
            }

            spinnerCurrentMessage = new Machine[MSpinnerNum];
            for (int i = 0; i < MSpinnerNum; i++)
            {
                Machine temp = new Machine
                {
                    Id = i,
                    Param = new MachineParam
                    {
                        Type = i / (MSpinnerNum / 4) + 1,
                        BeginTime = 0,
                        EndTime = 0
                    },
                    //ChangeTimes = 0
                };
                temp.Param.EndTime += nowConditionList[6][i];
                spinnerCurrentMessage[i] = temp;
            }
            return randomLine;
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Clone<T>(T t)
        {
            //创建二进制序列化对象
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())//创建内存流
            {
                //将对象序列化到内存中
                bf.Serialize(ms, t);
                ms.Position = 0;//将内存流的位置设为0
                return (T)bf.Deserialize(ms);//继续反序列化
            }
        }
    }
}
