using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    class MachineGroup
    {
        private int type;//该机器组对应的品种的编号
        private int status;
        private List<Machine> machines;

        public MachineGroup(int type, int status)
        {
            this.type = type;
            this.status = status;
            machines = new List<Machine>();
        }

        public MachineGroup(int type, int status, List<Machine> machines)
        {
            this.Type = type;
            this.status = status;
            this.machines = machines;
        }

        public int Status { get => status; set => status = value; }
        public int Type { get => type; set => type = value; }
        public List<Machine> Machines { get => machines; set => machines = value; }

        public void addMachine(Machine newmachine) { machines.Add(newmachine); }
        public void removeMachine(Machine machine) { machines.Remove(machine); }
    }
}
