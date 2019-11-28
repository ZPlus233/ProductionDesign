using System;
using System.Collections.Generic;
using WebApplication.GA.CommonClass.Produce;
using WebApplication.GA.GA;

namespace WebApplication.GA
{
    public class GAMainDeal
    {
        public static string GA()
        {
            string message = "";
            //try
            //{
                ToolUtil.Init();
                List<Order> orders = ToolUtil.OrderTableReader();
                List<Batch> b = ToolUtil.fromOrderToBatch(orders);
                GeneticAlgorithm ga = new GeneticAlgorithm();
                message = ga.GA(b);
            //}
            //catch (Exception e)
            //{
            //    message = "运行出现异常，请按照示例填写上传的表格内容，重新上传后再次运行";
            //}
            return message;
        }
    }
}  