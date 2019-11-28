using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    class Procedure
    {
        double starttime;//工序开始时间
        double machinenum;//工序所用机器套数
        Product inproduct;//工序输入原料
        double inputweight;//工序输入原料质量
        double yieldrate;//制成率
        double runningrate;//设备运转率
        double workinghours;//设备运转时间
        Product outproduct;//工序输出产品
        double outputweight;//工序输出产品质量
        int outamount;//工序输出产品数量
        int productionnum;//平均每台机器生产次数=工序输出产品数量/机器每次输出产品数量/所用机器套数
        double surplus;//剩余量
        int newstandardnum;//应采用新标准生产的桶数
        int normalnum;//采用正常标准生产的桶数
        double sumnewstandardweight;//应采用新标准生产的产品总质量
        double avgnewstandardweight;//应采用新标准生产的产品单位质量
        double outtime;//生产所需时间
        double suminpreparetime;//准备原料总时间
        double sumoutremovetime;//总输出移出时间
        double sumproceduretime;//工序所需总时间
        List<Machine> machines;//该工序所包含机器

        public double Starttime { get => starttime; set => starttime = value; }
        public double Machinenum { get => machinenum; set => machinenum = value; }
        public double Inputweight { get => inputweight; set => inputweight = value; }
        public double Yieldrate { get => yieldrate; set => yieldrate = value; }
        public double Outputweight { get => outputweight; set => outputweight = value; }
        public int Outamount { get => outamount; set => outamount = value; }
        public double Surplus { get => surplus; set => surplus = value; }
        public int Newstandardnum { get => newstandardnum; set => newstandardnum = value; }
        public int Normalnum { get => normalnum; set => normalnum = value; }
        public double Sumnewstandardweight { get => sumnewstandardweight; set => sumnewstandardweight = value; }
        public double Avgnewstandardweight { get => avgnewstandardweight; set => avgnewstandardweight = value; }
        public double Outtime { get => outtime; set => outtime = value; }
        public double Suminpreparetime { get => suminpreparetime; set => suminpreparetime = value; }
        public double Sumoutremovetime { get => sumoutremovetime; set => sumoutremovetime = value; }
        public double Sumproceduretime { get => sumproceduretime; set => sumproceduretime = value; }
        public int Productionnum { get => productionnum; set => productionnum = value; }
        public double Runningrate { get => runningrate; set => runningrate = value; }
        public double Workinghours { get => workinghours; set => workinghours = value; }
        public Product Inproduct { get => inproduct; set => inproduct = value; }
        public Product Outproduct { get => outproduct; set => outproduct = value; }
        public List<Machine> Machines { get => machines; set => machines = value; }

        public override string ToString()
        {
            //1.梳棉筒2.预并筒3.条并卷4.精梳筒5.末并筒6.粗砂管7.细纱管
            string str = "";
            switch (Outproduct.Type)
            {
                case 1: str = "清梳联"; break;
                case 2: str = "预并"; break;
                case 3: str = "条并卷"; break;
                case 4: str = "精梳"; break;
                case 5: str = "末并"; break;
                case 6: str = "粗纱"; break;
                case 7: str = "细纱"; break;
                default: str = "error"; break;
            }

            string data = "过程:" + str +
                "\n输入质量:" + Inputweight +
                "kg\n本批次接受的输出质量:" + Outputweight +
                "kg\n开始时间:" + starttime +
                "min\n生产耗时:" + Outtime +
                "min\n原料准备耗时:" + Suminpreparetime +
                "min\n产品移出耗时:" + Sumoutremovetime + "min\n";
            return data;
        }
    }
}
