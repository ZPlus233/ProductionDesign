using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    class Textile
    {
        private int type;//纱种类从1开始计数
        private string name;//纱名
        private string orderid;//订单id
        private DateTime deadline;//交付时间
        private double weight;//要求产品质量
        private double upperlimitrate;//额外生产上限率
        private double upperlimit;//实际产品质量上限=weight*upperlimitrate
        private double lowerlimitrate;//额外生产下限率
        private double lowerlimit;//实际产品质量下限=weight*lowerlimitrate

        public Textile(int type, string name,DateTime deadline)
        {
            this.Type = type;
            this.Name = name;
            this.Deadline = deadline;
            this.weight = 0;
        }

        public Textile(int type, string name)
        {
            this.Type = type;
            this.Name = name;
            this.weight = 0;
        }

        public string Name { get => name; set => name = value; }
        public double Upperlimit { get => upperlimit; set => upperlimit = value; }
        public double Lowerlimit { get => lowerlimit; set => lowerlimit = value; }
        public double Weight { get => weight; set => weight = value; }
        public double Upperlimitrate { get => upperlimitrate; set => upperlimitrate = value; }
        public double Lowerlimitrate { get => lowerlimitrate; set => lowerlimitrate = value; }
        public int Type { get => type; set => type = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public string Orderid { get => orderid; set => orderid = value; }
    }
}
