using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebApplication.GA.CommonClass.Produce;
using WebApplication.GA.GA;

namespace WebApplication.GA.CommonClass.GA
{
    class Population
    {
        List<GenePart> gene;
        double cost;
        bool isReasonable;
        List<List<double>> randomLine;

        /// <summary>
        /// 代价函数
        /// </summary>
        public double Cost { get => cost; set => cost = value; }
        /// <summary>
        /// 是否合理
        /// </summary>
        public bool IsReasonable { get => isReasonable; set => isReasonable = value; }
        /// <summary>
        /// 基因
        /// </summary>
        public List<GenePart> Gene { get => gene; set => gene = value; }
        /// <summary>
        /// 随机生产线
        /// </summary>
        public List<List<double>> RandomLine { get => randomLine; set => randomLine = value; }

        public string toString(List<Batch> b)
        {
            StringBuilder re = new StringBuilder();
            re.Append("<h3>生产线上机器台数：</h3>" + "<br/>");
            foreach (List<double> line in randomLine)
            {
                List<string> l = line.Select(o => o + "").ToList();
                re.Append("<b>生产线" + (randomLine.IndexOf(line) + 1) + "：</b>" + join(l));
            }
            re.Append("<br/>遗传算法参数：遗传进化迭代次数：" + GeneticAlgorithm.M + ";种群规模：" + GeneticAlgorithm.N + ";变异概率：" + GeneticAlgorithm.Pm + "<br/>");
            re.Append("<b>最短天数：" + Math.Round(cost/60/24, 2) + "天</b><br/><br/>");
            DateTime now = DateTime.Now;
            foreach (GenePart g in gene)
            {
                List<string> begin = g.Message.BeginTime.Select(o => now.AddDays((int)Math.Ceiling(o / 60 / 24)).ToLongDateString()).ToList();
                List<string> end = g.Message.EndTime.Select(o => now.AddDays((int)Math.Ceiling(o / 60 / 24)).ToLongDateString()).ToList();
                List<string> input = g.Message.InTimes.Select(o => o + "").ToList();
                List<string> output = g.Message.OutTimes.Select(o => o + "").ToList();

                re.Append("<h3>批次号：" + g.Id + " 生产线选择：" + (g.LineSelect + 1) + "</h3><br/>");
                re.Append("<b>所属订单号：" + b[g.Id].OrderId + "; 产品名称：" + b[g.Id].BatchDetail.Name + "; </b>产品目标重量：" + b[g.Id].BatchDetail.Weight
                    + "; 产品完成时间：" + b[g.Id].BatchDetail.Deadline.ToShortDateString() + "<br/>");
                re.Append("<b>各工序开始时间：</b><br/>");
                re.Append(join(begin));
                re.Append("<b>各工序结束时间：</b><br/>" );
                re.Append(join(end));
                re.Append("各工序输入次数（输入次数*生产线该工序设备台数*输入端口 = 总输入数目）：" + "<br/>");
                re.Append(join(input));
                re.Append("各工序输出次数（输出次数*生产线该工序设备台数*输出端口 = 总输出数目）：" + "<br/>");
                re.Append(join(output) + "<br/>");
            }

            return re+"";
        }

        public string join(List<string> L)
        {
            StringBuilder re = new StringBuilder();
            for (int i = 0; i < 7; i++)
                re.Append(ToolUtil.machineMessageDict[1].MachineNames[i] + ":" + L[i] + "  ");
            return re.Append("<br/>")+"";
        }
    }

    /// <summary>
    /// 基因片段
    /// </summary>
    class GenePart
    {
        int id;
        int lineSelect;
        ProduceMessage message;
        bool isTwoTypes;
        int roveNum;
        int yarnNum;

        /// <summary>
        /// batch id
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// 生产线选择
        /// </summary>
        public int LineSelect { get => lineSelect; set => lineSelect = value; }
        /// <summary>
        /// 各工序生产信息
        /// </summary>
        public ProduceMessage Message { get => message; set => message = value; }
        /// <summary>
        /// 是否为两种同时生产
        /// </summary>
        public bool IsTwoTypes { get => isTwoTypes; set => isTwoTypes = value; }
        /// <summary>
        /// 粗纱机数目
        /// </summary>
        public int RoveNum { get => roveNum; set => roveNum = value; }
        /// <summary>
        /// 细纱机数目
        /// </summary>
        public int YarnNum { get => yarnNum; set => yarnNum = value; }
    }

    class ProduceMessage
    {
        List<double> beginTime;
        List<double> endTime;
        List<double> duringTime;
        List<int> inTimes;
        List<int> outTimes;

        /// <summary>
        /// 各工序开始时间
        /// </summary>
        public List<double> BeginTime { get => beginTime; set => beginTime = value; }
        /// <summary>
        /// 各工序结束时间
        /// </summary>
        public List<double> EndTime { get => endTime; set => endTime = value; }
        /// <summary>
        /// 各工序持续时间
        /// </summary>
        public List<double> DuringTime { get => duringTime; set => duringTime = value; }
        /// <summary>
        /// 各工序输入次数（针对一台设备）
        /// </summary>
        public List<int> InTimes { get => inTimes; set => inTimes = value; }
        /// <summary>
        /// 各工序输出次数（落纱次数，针对一台设备）
        /// </summary>
        public List<int> OutTimes { get => outTimes; set => outTimes = value; }
    }
}
