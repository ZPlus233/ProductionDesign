using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebApplication.BasicClasses
{   
    [Serializable]
    class Line
    {
        private int id;//生产线编号1,2,3
        private double[] machines;
        private List<Batch> batches;

        public Line(int id)
        {
            this.Id = id;
            this.Batches = new List<Batch>();
        }

        public Line(int id, double[] machines)
        {
            this.Id = id;
            this.Machines = machines;
            this.Batches = new List<Batch>();
        }

        public int Id { get => id; set => id = value; }
        public double[] Machines { get => machines; set => machines = value; }
        public List<Batch> Batches { get => batches; set => batches = value; }

        public override string ToString()
        {
            string info = "Line " + (id+1) + ":\n";
            foreach (Batch batch in Batches)
            {
                double timeinDays = batch.Predicttime / 23 / 60;
                //info += "[Type: " + batch.Type.ToString() + " Weight: " + batch.Ordervirtualweight.ToString() + " ]";
                info += "[单品种类: " + batch.Type.ToString() + " 计划生产质量: " + batch.Ordervirtualweight.ToString() +"(kg）"+ "预计完成所需时间:" +
                    Math.Round(timeinDays,2) + " 天 ]\n";
            }
            return info;
        }
    }
}
