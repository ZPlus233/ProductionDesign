using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.GA.CommonClass.Produce
{
    class Order
    {
        string id;
        string partyAname;
        List<ProductDetail> detail;

        /// <summary>
        /// 订单id
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// 甲方名字
        /// </summary>
        public string PartyAname { get => partyAname; set => partyAname = value; }
        /// <summary>
        /// 订单详情
        /// </summary>
        public List<ProductDetail> Detail { get => detail; set => detail = value; }

        public Order(string id, string partyAname)
        {
            this.id = id;
            this.partyAname = partyAname;
            this.detail = new List<ProductDetail>();
        }

        public Order()
        {
        }
    }

    class ProductDetail
    {
        int id;
        string name;
        double weight;
        double upperlimitrate;
        DateTime deadline;

        /// <summary>
        /// 产品id（1~4）
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 目标产品重量
        /// </summary>
        public double Weight { get => weight; set => weight = value; }
        /// <summary>
        /// 允许上限
        /// </summary>
        public double Upperlimitrate { get => upperlimitrate; set => upperlimitrate = value; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime Deadline { get => deadline; set => deadline = value; }

        public ProductDetail(int id, string name, DateTime deadline)
        {
            this.id = id;
            this.Name = name;
            this.Deadline = deadline;
        }

        public ProductDetail()
        {
        }
    }

    class Batch
    {
        ProductDetail batchDetail;
        string orderId;

        /// <summary>
        /// 所属订单id
        /// </summary>
        public string OrderId { get => orderId; set => orderId = value; }
        /// <summary>
        /// 批次信息详情
        /// </summary>
        public ProductDetail BatchDetail { get => batchDetail; set => batchDetail = value; }


    }
}
