using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    abstract class Product
    {
        private string id;
        private string name;//产品名称
        private int type;//产品类：1.梳棉筒2.预并筒3.条并卷4.精梳筒5.末并筒6.粗砂管7.细纱管
        private double wetquantity;//单位湿重
        private double dryquantity;//单位干重
        private DateTime begintime;//生产开始时间
        private DateTime finishtime;//生产完成时间

        protected Product(string id,string name, int type, double wetquantity, double dryquantity)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.wetquantity = wetquantity;
            this.dryquantity = dryquantity;
        }

        public string Id { get => id; set => id = value; }
        public int Type { get => type; set => type = value; }
        public double Wetquantity { get => wetquantity; set => wetquantity = value; }
        public double Dryquantity { get => dryquantity; set => dryquantity = value; }
        public DateTime Begintime { get => begintime; set => begintime = value; }
        public DateTime Finishtime { get => finishtime; set => finishtime = value; }
        public string Name { get => name; set => name = value; }
    }
}
