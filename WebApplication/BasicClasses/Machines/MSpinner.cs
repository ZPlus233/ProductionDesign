using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses.Machines
{
    [Serializable]
    //细纱机
    class MSpinner : Machine
    {
        public MSpinner(string id, string name, int type, int status) : base(id, name, type, status)
        {
        }

        public override void Produce()
        {
            throw new NotImplementedException();
        }
    }
}
