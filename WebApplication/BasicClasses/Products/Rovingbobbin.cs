using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses.Products
{
    [Serializable]
    //粗砂管
    class Rovingbobbin : Product
    {
        public Rovingbobbin(string id, string name, int type, double wetquantity, double dryquantity) : base(id, name, type, wetquantity, dryquantity)
        {
        }
    }
}
