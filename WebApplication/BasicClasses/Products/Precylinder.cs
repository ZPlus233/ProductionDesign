using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses.Products
{
    [Serializable]
    //预并筒
    class Precylinder : Product
    {
        public Precylinder(string id, string name, int type, double wetquantity, double dryquantity) : base(id, name, type, wetquantity, dryquantity)
        {
        }
    }
}
