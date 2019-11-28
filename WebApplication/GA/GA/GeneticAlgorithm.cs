using WebApplication.GA.CommonClass.GA;
using WebApplication.GA.CommonClass.Produce;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;

namespace WebApplication.GA.GA
{
    /// <summary>
    /// 遗传算法
    /// </summary>
    class GeneticAlgorithm
    {
        public static int M = 10;    //遗传进化迭代次数
        public static int N = 40;    //种群规模
        public static int Parent_N = 20; //每一代中保持不变的数目
        public static double Pm = 0.5;  //变异概率
        private List<double> best_fitness = new List<double>();     //每一代的最佳适应度
        private List<Population> elite = new List<Population>();        //每一代的精英的参数值
        private Dictionary<int, List<double>> macnineRunningNumsDict = new Dictionary<int, List<double>>();
        private List<List<double>> line = new List<List<double>>();

        /// <summary>
        /// 遗传算法主逻辑
        /// </summary>
        /// <param name="b">批次信息</param>
        public string GA(List<Batch> b)
        {
            for (int i = 0; i < b.Count; i++)
            {
                List<double> macnineRunningNums = ToolUtil.getMachineRunningNum(b[i].BatchDetail.Weight, b[i].BatchDetail.Id);
                macnineRunningNumsDict.Add(i, macnineRunningNums);
            }
            line = getRandomProduceLine();
            List<Population> populations = new List<Population>();
            //1.随机获取初始种群；2、计算每个种群的适应度函数
            for (int i = 0; i < N; i++)
                populations.Add(getPopulation(b));

            List<double> cost = new List<double>();
            double min_cost = populations.Min(o => o.Cost);
            for (int gengration = 1; gengration <= M; gengration++)  //开始进化
            {
                populations = populations.Where(o => o.IsReasonable == true).OrderBy(o => o.Cost).Take(Parent_N).ToList();
                best_fitness.Add(populations.FirstOrDefault().Cost);
                elite.Add(clonePopulation(populations, 0, b));

                Console.WriteLine(best_fitness.LastOrDefault());
                if (gengration == M)
                    continue;

                int num = populations.Count;
                //3.染色体交叉，每一次交叉会产生2个孩子(交换随机生产线)
                for (int i = 0; i < num / 2; i++)
                {
                    populations.AddRange(crossPopulations(populations, i*2, i*2+1, b));
                }

                //4.染色体变异
                int number_of_elements = (populations.Count() - 1) * b.Count;   //全部基因数目
                int number_of_mutations = (int)(number_of_elements * Pm);//变异的基因数目（基因总数*变异率）
                for (int i = 0; i < number_of_mutations; i++)
                    populations = nutatePopulation(populations, b);

                //补充新种群，满足N个
                num = populations.Count;
                for (int i = num; i < N; i++)
                    populations.Add(getPopulation(b));
            }

            //for(int i = 0; i < M; i++)
            //{
            //    for(int j = 0; j <7; j++)
            //    {
            //        Console.Write(elite[i].Gene.Last().Message.EndTime[j]+" ");
            //    }
            //    Console.WriteLine();
            //}

            return elite.Last().toString(b);
        }

        /// <summary>
        /// 随机获取种群
        /// </summary>
        /// <param name="b">批次信息</param>
        public Population getPopulation(List<Batch> b)
        {
            int batchNum = b.Count;
            Population p = new Population();
            p.Gene = new List<GenePart>();
            p.RandomLine = line;
            p.IsReasonable = true;
            for (int i = 0; i < batchNum; i++)
            {
                GenePart temp = new GenePart();
                temp.Id = i;
                temp.LineSelect = getRandom(0, 2);
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
        public double fitness(Population x, List<Batch> b)
        {
            // 每个生产线的运行总时间
            double[] lineTotalTime = new double[3];
            List<List<int>> lineBatches = new List<List<int>>() { new List<int>(), new List<int>(), new List<int>()};
            // 生产线包含哪些批次
            for(int i = 0; i < x.Gene.Count; i++)
                lineBatches[x.Gene[i].LineSelect].Add(i);

            for(int i = 0; i < 3; i++)
            {
                if(lineBatches[i].Count != 0)
                {
                    lineTotalTime[i] = getLineRunningTime(x, lineBatches[i], b, x.RandomLine[i]);
                }
            }
            x.Cost = lineTotalTime.Max();
            return lineTotalTime.Max();
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
            //randomLine.Add(new List<double>() { 12, 4, 2, 12, 5, 5, 19 });
            //int nums1 = getRandom(1, 6);
            //int nums2 = getRandom(1, 21);
            //randomLine.Add(new List<double>() { 10, 2, 1, 7, 7 - nums1, 7 - nums1, 22 - nums2 });
            //randomLine.Add(new List<double>() { 12, 2, 2, 11, nums1, nums1, nums2 });

            DataSet dataSet = BasicClasses.Util.ExcelToDS(MainDeal.path + "2生产线配置表模板.xls", 1);
            int rowlength = dataSet.Tables[0].Rows.Count;
            for (int i = 0; i < rowlength; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if(j == 0)
                        randomLine.Add(new List<double>());
                    randomLine.Last().Add(Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[j + 1]));
                }
            }

            return randomLine;
        }

        /// <summary>
        /// 获取生产线运行时间(不涉及一种粗纱生产两种细纱，都按生产线走)
        /// </summary>
        /// <param name="x">种群信息</param>
        /// <param name="lineBatch">目标生产线，生产的批次list</param>
        /// <param name="b">批次list</param>
        /// <param name="line">当前生产线</param>
        /// <returns></returns>
        public double getLineRunningTime(Population x, List<int> lineBatch, List<Batch> b, List<double> line)
        {
            double time = 0;
            int preId = -1;
            int preBatch = -1;
            foreach (int batchNo in lineBatch)
            {
                int id = b[batchNo].BatchDetail.Id;
                ProduceMessage message = new ProduceMessage();
                message.BeginTime = new List<double>();
                message.EndTime = new List<double>();
                message.DuringTime = new List<double>();
                message.InTimes = new List<int>();
                message.OutTimes = new List<int>();
                List<double> macnineRunningNums = macnineRunningNumsDict[batchNo];
                for (int i = 0; i < 7; i++)
                {
                    message.InTimes.Add((int)Math.Ceiling(macnineRunningNums[i] / line[i]));
                    message.OutTimes.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[id][i].DoffingTimes);
                    message.DuringTime.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[id][i].RunTime);

                    if(preId == -1)
                    {
                        if(i == 0)
                        {
                            message.BeginTime.Add(0);
                            message.EndTime.Add(message.DuringTime.Last());
                        }
                        else
                        {
                            int t = (int)Math.Ceiling((line[i] * ToolUtil.productCapacityDict[id][i].InputNum)
                                / (line[i - 1] * ToolUtil.productCapacityDict[id][i-1].OutputNum));

                            if (message.EndTime.Last() + ToolUtil.productCapacityDict[id][i].RunTime >
                                message.BeginTime.Last() + t * ToolUtil.productCapacityDict[id][i - 1].RunTime + message.DuringTime.Last())
                            {
                                message.EndTime.Add(message.EndTime.Last() + ToolUtil.productCapacityDict[id][i].RunTime);
                            }
                            else
                            {
                                message.EndTime.Add(message.BeginTime.Last() + t * ToolUtil.productCapacityDict[id][i - 1].RunTime
                                    + message.DuringTime.Last());
                            }
                            message.BeginTime.Add(message.EndTime.Last() - message.DuringTime.Last());
                        }
                    }
                    else
                    {
                        if(i == 0)
                        {
                            if (preId / 3 == id / 3)   //前纺相同
                            {
                                message.BeginTime.Add(x.Gene[preBatch].Message.EndTime.First());
                            }
                            else    //前纺不同,上一批次结束时间+设备配置切换时间
                            {
                                message.BeginTime.Add(x.Gene[preBatch].Message.EndTime.First() + ToolUtil.changeTime[i][id-1][preId-1]);
                            }
                            message.EndTime.Add(message.BeginTime.Last() + message.DuringTime.Last());
                        }
                        else
                        {
                            if (message.EndTime.Last() + ToolUtil.productCapacityDict[id][i].RunTime >
                                x.Gene[preBatch].Message.EndTime[i] + ToolUtil.changeTime[i][id-1][preId-1] + message.DuringTime.Last())
                            {
                                message.EndTime.Add(message.EndTime.Last() + ToolUtil.productCapacityDict[id][i].RunTime);
                            }
                            else
                            {
                                message.EndTime.Add(x.Gene[preBatch].Message.EndTime[i] + ToolUtil.changeTime[i][id-1][preId-1] + message.DuringTime.Last());
                            }
                            message.BeginTime.Add(message.EndTime.Last() - message.DuringTime.Last());
                        }
                    }
                }
                if (message.EndTime.Max() > (b[batchNo].BatchDetail.Deadline - DateTime.Now).TotalMinutes)
                    x.IsReasonable = false;
                time = time > message.EndTime.Max() ? time : message.EndTime.Max();
                x.Gene[batchNo].Message = message;
                preId = id;
                preBatch = batchNo;
            }
            return time;
        }


        /// <summary>
        /// 两个染色体交叉
        /// </summary>
        /// <param name="populations">全部种群</param>
        /// <param name="index1">要交叉的两个种群index1</param>
        /// <param name="index2">要交叉的两个种群index2</param>
        /// <param name="b">批次list</param>
        /// <returns></returns>
        public List<Population> crossPopulations(List<Population> populations, int index1, int index2, List<Batch> b)
        {
            List<Population> re = new List<Population>();
            Population p1 = new Population();
            p1.RandomLine = new List<List<double>>();

            Population p2 = new Population();
            p2.RandomLine = new List<List<double>>();
            for (int i = 0; i < populations[index1].RandomLine.Count; i++)
            {
                List<double> t1 = new List<double>();
                for (int j = 0; j < populations[index1].RandomLine[i].Count; j++)
                    t1.Add(populations[index1].RandomLine[i][j]);
                p1.RandomLine.Add(t1);

                List<double> t2 = new List<double>();
                for (int j = 0; j < populations[index2].RandomLine[i].Count; j++)
                    t2.Add(populations[index2].RandomLine[i][j]);
                p2.RandomLine.Add(t2);
            }

            p1.Gene = new List<GenePart>();
            p2.Gene = new List<GenePart>();
            // 基因交叉
            int select = getRandom(0, b.Count-1);
            for (int i = 0; i < b.Count; i++)
            {
                GenePart temp1 = new GenePart();
                temp1.Id = i;
                temp1.LineSelect = populations[index1].Gene[i].LineSelect;

                GenePart temp2 = new GenePart();
                temp2.Id = i;
                temp2.LineSelect = populations[index2].Gene[i].LineSelect;

                if (select < i)
                {
                    p1.Gene.Add(temp1);
                    p2.Gene.Add(temp2);
                }
                else
                {
                    p1.Gene.Add(temp2);
                    p2.Gene.Add(temp1);
                }
            }
            p1.Cost = fitness(p1, b);
            p2.Cost = fitness(p2, b);

            re.Add(p1);
            re.Add(p2);
            return re;
        }

        /// <summary>
        /// 染色体变异
        /// </summary>
        /// <param name="populations">全部基因</param>
        /// <param name="b">批次</param>
        public List<Population> nutatePopulation(List<Population> populations, List<Batch> b)
        {
            int select_population = getRandom(1, populations.Count()-1);   //随机选择一个种群（不可以选择最优种群）
            int select = getRandom(0, b.Count-1); //选择批次
            populations[select_population].Gene[select].LineSelect = getRandom(0, 2);   //重新选择生产线
            populations[select_population].Cost = fitness(populations[select_population], b);
            return populations;
        }

        /// <summary>
        /// 克隆population
        /// </summary>
        /// <param name="populations">种群</param>
        /// <param name="index">被克隆index</param>
        /// <param name="b">批次list</param>
        /// <returns></returns>
        public Population clonePopulation(List<Population> populations, int index, List<Batch> b)
        {
            List<Population> re = new List<Population>();
            Population p = new Population();
            p.IsReasonable = populations[index].IsReasonable;
            p.RandomLine = new List<List<double>>();
            for (int i = 0; i < populations[index].RandomLine.Count; i++)
            {
                List<double> t1 = new List<double>();
                for (int j = 0; j < populations[index].RandomLine[i].Count; j++)
                    t1.Add(populations[index].RandomLine[i][j]);
                p.RandomLine.Add(t1);
            }

            p.Gene = new List<GenePart>();
            for (int i = 0; i < b.Count; i++)
            {
                GenePart temp1 = new GenePart();
                temp1.Id = i;
                temp1.LineSelect = populations[index].Gene[i].LineSelect;
                p.Gene.Add(temp1);
            }
            p.Cost = fitness(p, b);
            return p;
        }
    }
}
