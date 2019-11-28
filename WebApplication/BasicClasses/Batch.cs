using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    class Batch//批次
    {
        private string id;
        private List<Order> orders;//批次中包含的订单
        private int type;//批次应当生产的纺纱类型
        private Line line;//批次所用的生产线集合
        private List<Procedure> procedures;//批次中包含的工序
        private DateTime deadline;//批次要求截止时间=？
        private DateTime finishtime;//批次实际完成时间
        private double predicttime;//批次预计完成时间
        private double orderinweight;//批次预期输入质量= 最终输出产品质量（由全部订单中该种类型棉实际生产上限之和求出）不断根据制成率反向计算得出
        private double orderweight;//订单要求批次输出质量=Textile的weight
        private double ordersumweight;//该批次最大输出的产品质量=各订单中该产品的质量之和*（1+上限率）
        private double ordervirtualweight;//该批次实际输出的产品质量=小于最大输出的质量
        private double[] changetime;//生产线上切换到该批次需要的时间
        private double startday;//批次第几天开始生产
        private double dayspan;//批次生产天数


        public Batch(string id, int type)
        {
            this.id = id;
            this.Type = type;
            orders = new List<Order>();
            procedures = new List<Procedure>();
            Changetime = new double[7];
        }
        public Batch(string id)
        {
            this.id = id;
            orders = new List<Order>();
            procedures = new List<Procedure>();
            Changetime = new double[7];
        }

        public string Id { get => id; set => id = value; }
        public List<Order> Orders { get => orders; set => orders = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public DateTime Finishtime { get => finishtime; set => finishtime = value; }
        public double Predicttime { get => predicttime; set => predicttime = value; }
        public List<Procedure> Procedures { get => procedures; set => procedures = value; }
        public int Type { get => type; set => type = value; }
        public double Ordersumweight { get => ordersumweight; set => ordersumweight = value; }
        public double Ordervirtualweight { get => ordervirtualweight; set => ordervirtualweight = value; }
        public double Orderinweight { get => orderinweight; set => orderinweight = value; }
        public Line Line { get => line; set => line = value; }
        public double[] Changetime { get => changetime; set => changetime = value; }
        public double Startday { get => startday; set => startday = value; }
        public double Dayspan { get => dayspan; set => dayspan = value; }
        public double Orderweight { get => orderweight; set => orderweight = value; }

        public override string ToString()
        {
            string info = "批次编号：" + Id + "\n" +
                "批次纺纱类型：" + Type + "\n" +
                "要求生产质量：" + Ordersumweight + " kg\n" +
                "包含订单：";
            foreach (Order order in Orders)
            {
                info += order.Id + "; ";
            }
            double timeinDays = Predicttime / 23 / 60;
            info += "\n预计完成需要时间:" + Math.Round(timeinDays, 2) + " Days\n";
            info += "切换时间：" + Changetime + "mins\n";
            return info;
        }
    }
}