using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses.Products
{
    [Serializable]
    //筒纱
    class Barrelyarn : Product
    {
        public Barrelyarn(string id, string name, int type, double wetquantity, double dryquantity) : base(id, name, type, wetquantity, dryquantity)
        {
        }
    }
}
