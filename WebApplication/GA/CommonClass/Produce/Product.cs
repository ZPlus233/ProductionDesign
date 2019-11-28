using System.Collections.Generic;

namespace WebApplication.GA.CommonClass
{
    /// <summary>
    /// 产品参数
    /// </summary>
    class Product
    {
        int id;
        string name;
        List<double> wetQuantity;
        List<double> dryQuantity;


        /// <summary>
        /// 产品编号
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 单位湿重(g/m)
        /// </summary>
        public List<double> WetQuantity { get => wetQuantity; set => wetQuantity = value; }
        /// <summary>
        /// 单位干重(g/m)
        /// </summary>
        public List<double> DryQuantity { get => dryQuantity; set => dryQuantity = value; }
    }
}
