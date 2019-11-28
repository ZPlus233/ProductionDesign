using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    //梳棉筒
    class Cottoncardingcylinder : Product
    {
        public Cottoncardingcylinder(string id, string name, int type, double wetquantity, double dryquantity) : base(id, name, type, wetquantity, dryquantity)
        {
        }
    }
}
