using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    abstract class Machine
    {
        private string id;
        private string name;
        private int type;//种类
        private int status;//状态---0，未工作；1，工作中；-1，维护中
        private Product inproduct;//输入产品
        private int inputamount;//输入数量
        private double inspeed;
        private int inperiod;
        private double inputpreparetime;//输入原料准备时间
        private Product outproduct;//输出产品
        private int outputamount;//每次输出数量
        private int length;//输出产品长度
        private double wetquantity;//输出单位湿重,湿重和干重在配置工序时设置数值
        private double dryquantity;//输出单位干重
        private double outspeed;//输出速度
        private double unitouttime;//单位输出时间(min/台)
        private double outtime;//输出时间
        private double outremovetime;//输出移出时间
        private double gaptime;//产品切换时间

        protected Machine(string id, string name, int type, int status)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.status = status;
        }

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Type { get => type; set => type = value; }
        public int Status { get => status; set => status = value; }
        public int Inputamount { get => inputamount; set => inputamount = value; }
        public double Inspeed { get => inspeed; set => inspeed = value; }
        public int Inperiod { get => inperiod; set => inperiod = value; }
        public double Inputpreparetime { get => inputpreparetime; set => inputpreparetime = value; }
        public int Outputamount { get => outputamount; set => outputamount = value; }
        public double Outspeed { get => outspeed; set => outspeed = value; }
        public double Outremovetime { get => outremovetime; set => outremovetime = value; }
        public double Gaptime { get => gaptime; set => gaptime = value; }
        public double Outtime { get => outtime; set => outtime = value; }
        public int Length { get => length; set => length = value; }
        public double Wetquantity { get => wetquantity; set => wetquantity = value; }
        public double Dryquantity { get => dryquantity; set => dryquantity = value; }
        public double Unitouttime { get => unitouttime; set => unitouttime = value; }
        public Product Inproduct { get => inproduct; set => inproduct = value; }
        public Product Outproduct { get => outproduct; set => outproduct = value; }
        public abstract void Produce();

    }
}
