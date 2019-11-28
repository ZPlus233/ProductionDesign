using System.Collections.Generic;

namespace WebApplication.GA.CommonClass
{
    /// <summary>
    /// 设备参数
    /// </summary>
    class MachineMessage
    {
        List<int> ids;
        List<string> machineNames;
        List<int> machineNum;
        List<double> yieldRates;
        List<double> runninGrates;
        List<double> workingHours;
        List<double> outputLength;
        List<double> outputSpeed;
        List<int> inputPortAmount;
        List<int> outputPortAmount;
        List<double> inPrepareTime;
        List<double> outRemoveTime;
        List<EveryMachineLimit> machineSelectLimits;
        List<double> unitouttime;


        /// <summary>
        /// 设备工序id
        /// </summary>
        public List<int> Ids { get => ids; set => ids = value; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public List<string> MachineNames { get => machineNames; set => machineNames = value; }
        /// <summary>
        /// 设备台数
        /// </summary>
        public List<int> MachineNum { get => machineNum; set => machineNum = value; }
        /// <summary>
        /// 制成率
        /// </summary>
        public List<double> YieldRates { get => yieldRates; set => yieldRates = value; }
        /// <summary>
        /// 设备运转率
        /// </summary>
        public List<double> RunninGrates { get => runninGrates; set => runninGrates = value; }
        /// <summary>
        /// 设备每日运行时间(h)
        /// </summary>
        public List<double> WorkingHours { get => workingHours; set => workingHours = value; }
        /// <summary>
        /// 每台设备输出长度(m/(管/筒))
        /// </summary>
        public List<double> OutputLength { get => outputLength; set => outputLength = value; }
        /// <summary>
        /// 每台设备输出速度(m/min)
        /// </summary>
        public List<double> OutputSpeed { get => outputSpeed; set => outputSpeed = value; }
        /// <summary>
        /// 输入数量(几个输入口)
        /// </summary>
        public List<int> InputPortAmount { get => inputPortAmount; set => inputPortAmount = value; }
        /// <summary>
        /// 输出数量(几个输出口)
        /// </summary>
        public List<int> OutputPortAmount { get => outputPortAmount; set => outputPortAmount = value; }
        /// <summary>
        /// 输入准备时间
        /// </summary>
        public List<double> InPrepareTime { get => inPrepareTime; set => inPrepareTime = value; }
        /// <summary>
        /// 输出原料移除时间
        /// </summary>
        public List<double> OutRemoveTime { get => outRemoveTime; set => outRemoveTime = value; }
        /// <summary>
        /// 设备选择约束情况
        /// </summary>
        public List<EveryMachineLimit> MachineSelectLimits { get => machineSelectLimits; set => machineSelectLimits = value; }
        /// <summary>
        /// 落纱（输出）单位时间
        /// </summary>
        public List<double> Unitouttime { get => unitouttime; set => unitouttime = value; }

    }
}

class EveryMachineLimit
{
    int id;
    List<EveryMachineGroupLimit> machineLimits;

    /// <summary>
    /// 工序id（0~6）
    /// </summary>
    public int Id { get => id; set => id = value; }
    /// <summary>
    /// 对应工序设备约束
    /// </summary>
    public List<EveryMachineGroupLimit> MachineLimits { get => machineLimits; set => machineLimits = value; }
}

class EveryMachineGroupLimit
{
    int groupId;
    int nums;
    int nextGruopId;

    /// <summary>
    /// 设备组号
    /// </summary>
    public int GroupId { get => groupId; set => groupId = value; }
    /// <summary>
    /// 组设备数目
    /// </summary>
    public int Nums { get => nums; set => nums = value; }
    /// <summary>
    /// 下一道工序对应组号
    /// </summary>
    public int NextGruopId { get => nextGruopId; set => nextGruopId = value; }
}
