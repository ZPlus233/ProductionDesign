using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.BasicClasses;

namespace TextileDemo.BasicClasses
{
    class AGV
    {
        private string id;
        private int status;
        private Machine targetmachine;

        public AGV(string id, int status)
        {
            this.id = id;
            this.status = status;
        }

        public string Id { get => id; set => id = value; }
        public int Status { get => status; set => status = value; }
        public Machine Targetmachine { get => targetmachine; set => targetmachine = value; }
    }
}
