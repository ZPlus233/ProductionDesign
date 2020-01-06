using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.GA;
using WebApplication.GA.CommonClass.GA;
using WebApplication.GA.CommonClass.Produce;

namespace WebApplication.NewMethod
{
    public class NewTest
    {
        static AlgorithmCommon common;
        /// <summary>
        /// 新方法
        /// </summary>
        /// <returns></returns>
        public static string New()
        {
            string message = "";
            try
            {
                ToolUtil.Init();
                List<Order> orders = ToolUtil.OrderTableReader();
                List<Batch> b = ToolUtil.fromOrderToBatch(orders);
                common = new AlgorithmCommon(b);
                Population p = new Population();
                p.Gene = new List<GenePart>();
                p.RandomLine = common.line;
                p.IsReasonable = true;
                p.ChangeTimes = 0;

                //依次处理每个批次
                for (int i = 0; i < b.Count; i++)
                {
                    GenePart g = new GenePart();
                    g.Id = i;
                    g.SpinnerSelect = new List<int>();

                    int n = (int)Math.Ceiling(common.macnineRunningNumsDict[i][6]);
                    var temp = common.spinnerCurrentMessage.Where(o => o.Param.Type == b[i].BatchDetail.Id).OrderBy(o => o.Id).OrderBy(o => o.Param.EndTime);
                    if (n < temp.Count())    //开n台细纱机
                        g.SpinnerSelect.AddRange(temp.Take(n).Select(o => o.Id));   //选取时间最近的几台细纱机
                    else    //细纱机不够
                    {
                        g.SpinnerSelect.AddRange(temp.Select(o => o.Id));   //选取时间最近的几台细纱机
                        temp = common.spinnerCurrentMessage.Where(o => o.Param.Type != b[i].BatchDetail.Id).OrderBy(o => o.Id).OrderBy(o => o.Param.EndTime);
                        g.SpinnerSelect.AddRange(temp.Take(n - g.SpinnerSelect.Count()).Select(o => o.Id));   //选取时间最近的几台细纱机
                    }
                    int lineSelect = 0;
                    for (int j = 0; j < common.line.Count; j++)      //选取最先释放的生产线
                        if (common.produceLineMessage[j, 5].Param.EndTime < common.produceLineMessage[lineSelect, 5].Param.EndTime)
                            lineSelect = j;
                    g.LineSelect = lineSelect;

                    bool a = getBatchCost(g, b);
                    if (!a)
                    {
                        string re = "没有可行的排产方案！";
                        return re;
                    }

                    p.Gene.Add(g);
                    p.ChangeTimes += g.ChangeTime;
                }
                p.Cost = p.Gene.OrderByDescending(o => o.Message.EndTime.Last()).First().Message.EndTime.Last();
                message = p.toString(b);
            }
            catch { }
            return message;
        }

        /// <summary>
        /// 单个批次进行计算
        /// </summary>
        /// <returns></returns>
        static bool getBatchCost(GenePart g, List<Batch> b)
        {
            int produceType = b[g.Id].BatchDetail.Id;    //本次生产产品类型
            int lineSelect = g.LineSelect;

            ProduceMessage message = new ProduceMessage();
            message.BeginTime = new List<double>();
            message.EndTime = new List<double>();
            message.DuringTime = new List<double>();
            message.InTimes = new List<int>();
            message.OutTimes = new List<int>();
            List<double> macnineRunningNums = common.macnineRunningNumsDict[g.Id];  //每个工序运行次数

            //7道工序
            //前6道工序
            for (int i = 0; i < 6; i++)
            {
                message.InTimes.Add((int)Math.Ceiling(macnineRunningNums[i] / common.line[lineSelect][i]));
                message.OutTimes.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][i].DoffingTimes);
                message.DuringTime.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][i].RunTime);

                if (common.produceLineMessage[lineSelect, i].Param.Type == 0)
                    common.produceLineMessage[lineSelect, i].Param.Type = produceType;

                if (i == 0)
                {
                    message.BeginTime.Add(common.produceLineMessage[lineSelect, i].Param.EndTime
                        + ToolUtil.changeTime[i][produceType - 1][common.produceLineMessage[lineSelect, i].Param.Type - 1]);
                    message.EndTime.Add(message.BeginTime.Last() + message.DuringTime.Last());
                }
                else
                {
                    int t = (int)Math.Ceiling((common.line[lineSelect][i] * ToolUtil.productCapacityDict[produceType][i].InputNum)
                        / (common.line[lineSelect][i - 1] * ToolUtil.productCapacityDict[produceType][i - 1].OutputNum));

                    double temp = message.BeginTime.Last() + t * ToolUtil.productCapacityDict[produceType][i - 1].RunTime
                        > common.produceLineMessage[lineSelect, i].Param.EndTime
                        ? message.BeginTime.Last() + t * ToolUtil.productCapacityDict[produceType][i - 1].RunTime :
                        common.produceLineMessage[lineSelect, i].Param.EndTime;

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

                common.produceLineMessage[lineSelect, i].Param.Type = produceType;
                common.produceLineMessage[lineSelect, i].Param.BeginTime = message.BeginTime.Last();
                common.produceLineMessage[lineSelect, i].Param.EndTime = message.EndTime.Last();
            }

            //第7道工序begin
            int num = g.SpinnerSelect.Count;  //选取细纱机台数
            message.InTimes.Add((int)Math.Ceiling(macnineRunningNums[6] / num));
            message.OutTimes.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][6].DoffingTimes);
            message.DuringTime.Add(message.InTimes.Last() * ToolUtil.productCapacityDict[produceType][6].RunTime);

            double endTimeTemp = common.spinnerCurrentMessage.Where(o => g.SpinnerSelect.Contains(o.Id)).Min(o => o.Param.EndTime);
            int t_7 = (int)Math.Ceiling((num * ToolUtil.productCapacityDict[produceType][6].InputNum)
                / (common.line[lineSelect][5] * ToolUtil.productCapacityDict[produceType][5].OutputNum));
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
            foreach (var k in g.SpinnerSelect)
            {
                if (produceType != common.spinnerCurrentMessage[k].Param.Type)
                    g.ChangeTime++;
                common.spinnerCurrentMessage[k].Param.Type = produceType;
                common.spinnerCurrentMessage[k].Param.BeginTime = message.BeginTime.Last();
                common.spinnerCurrentMessage[k].Param.EndTime = message.EndTime.Last();
            }
            //第7道工序end

            if (message.EndTime.Max() > (b[g.Id].BatchDetail.Deadline - DateTime.Now).TotalMinutes)    //不合理
                return false;
            else
            {
                g.Message = message;
                return true;
            }
        }
    }
}