using System;

namespace WebApplication.GA.CommonClass.GA
{
    [Serializable]
    class Machine
    {
        int id;  //细纱机的设备id
        MachineParam param;   //针对每一台细纱机的参数信息（历史轨迹）
        //int changeTimes;

        /// <summary>
        /// 细纱机的设备id
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// 针对每一台细纱机的参数信息
        /// </summary>
        public MachineParam Param { get => param; set => param = value; }
        ///// <summary>
        ///// 设备切换次数
        ///// </summary>
        //public int ChangeTimes { get => changeTimes; set => changeTimes = value; }
    }

    [Serializable]
    class MachineParam
    {
        int type;//生产产品种类
        double beginTime;   //当前产品（批次）生产细纱机开始时间
        double endTime; //当前产品（批次）生产细纱机结束时间

        /// <summary>
        /// 生产产品种类
        /// </summary>
        public int Type { get => type; set => type = value; }
        /// <summary>
        /// 当前产品（批次）生产细纱机开始时间
        /// </summary>
        public double BeginTime { get => beginTime; set => beginTime = value; }
        /// <summary>
        /// 当前产品（批次）生产细纱机结束时间
        /// </summary>
        public double EndTime { get => endTime; set => endTime = value; }

    }
}
