using System;
using System.Threading;

namespace WebApplication.GA.CommonClass
{
    /// <summary>
    /// 品种单台设备产能
    /// </summary>
    class ProductCapacity
    {
        int id;
        string machineName;
        int machineId;
        double inputWeight;
        int inputNum;
        double outTubWeight;
        int outputNum;
        double outputWeight;
        double yield;
        double runTime;
        int doffingTimes;


        /// <summary>
        /// 生产产品id（1~4）
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string MachineName { get => machineName; set => machineName = value; }
        /// <summary>
        /// 工序id（1~7）
        /// </summary>
        public int MachineId { get => machineId; set => machineId = value; }
        /// <summary>
        ///  输入重量(kg)
        /// </summary>
        public double InputWeight { get => inputWeight; set => inputWeight = value; }
        /// <summary>
        /// 输入数量(单位生产材料)
        /// </summary>
        public int InputNum { get => inputNum; set => inputNum = value; }
        /// <summary>
        /// 输出每管重量(kg)
        /// </summary>
        public double OutTubWeight { get => outTubWeight; set => outTubWeight = value; }
        /// <summary>
        /// 输出数量(单位生产材料)
        /// </summary>
        public int OutputNum { get => outputNum; set => outputNum = value; }
        /// <summary>
        /// 输出重量(kg)
        /// </summary>
        public double OutputWeight { get => outputWeight; set => outputWeight = value; }
        /// <summary>
        /// 实际制成率
        /// </summary>
        public double Yield { get => yield; set => yield = value; }
        /// <summary>
        /// 单台设备运行时常(min)
        /// </summary>
        public double RunTime { get => runTime; set => runTime = value; }
        /// <summary>
        /// 落纱次数
        /// </summary>
        public int DoffingTimes { get => doffingTimes; set => doffingTimes = value; }

        //输出string
        public string toString()
        {
            string a = "生产产品id:" + Id
                + " 工序名称:" + MachineName
                + " 工序id:" + MachineId
                + " 输入重量:" + InputWeight
                + " 输入数量:" + InputNum
                + " 输出每管重量：" + OutTubWeight
                + " 输出单位数量:" + OutputNum
                + " 输出重量:" + OutputWeight
                + " 落纱次数:" + DoffingTimes
                + " 实际制成率" + Yield
                + " 单台设备运行时常:" + RunTime;

            Thread.Sleep(100);
            Console.WriteLine(a);
            return a;
        }
    }
}
