using System.Collections.Generic;
using TextileDemo.BasicClasses;

namespace WebApplication.BasicClasses
{
    class Factory
    {
        private List<MachineGroup> machinegroups;
        private List<AGV> avgs;
        private List<List<Product>> productlists;
        private List<Line> lines;

        public List<AGV> Avgs { get => avgs; set => avgs = value; }
        public List<MachineGroup> Machinegroups { get => machinegroups; set => machinegroups = value; }
        public List<List<Product>> Productlists { get => productlists; set => productlists = value; }
        public List<Line> Lines { get => lines; set => lines = value; }

        public void addAGV(AGV newavg) { avgs.Add(newavg); }
        public void removeAGV(AGV avg) { avgs.Remove(avg); }
    }
}
