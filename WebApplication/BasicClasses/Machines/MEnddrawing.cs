using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses.Machines
{
    [Serializable]
    //末并条机
    class MEnddrawing : Machine
    {
        public MEnddrawing(string id, string name, int type, int status) : base(id, name, type, status)
        {
        }

        public override void Produce()
        {
            throw new NotImplementedException();
        }
    }
}
