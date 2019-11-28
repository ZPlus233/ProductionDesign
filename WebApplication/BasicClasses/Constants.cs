using System;
using System.Data;

namespace WebApplication.BasicClasses
{
    class Constants
    {
        public static int MBLOWINGCARDING_NUMS = 34;
        public static int MPREDRAWING_NUMS = 8;
        public static int MSTRIPTOROLL_NUMS = 5;
        public static int MCOMBER_NUMS = 30;
        public static int MENDDRAWING_NUMS = 12;
        public static int MBOBBINER_NUMS = 12;
        public static int MSPINNER_NUMS = 82;

        public static int AVG_NUMS = 6;

        public static void Init()
        {
            DataSet dataSet = Util.ExcelToDS(MainDeal.path + "1工厂机器数量配置表模板.xls", 1);
            MBLOWINGCARDING_NUMS = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[1]);
            MPREDRAWING_NUMS = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[2]);
            MSTRIPTOROLL_NUMS = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[3]);
            MCOMBER_NUMS = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[4]);
            MENDDRAWING_NUMS = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[5]);
            MBOBBINER_NUMS = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[6]);
            MSPINNER_NUMS = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[7]);
        }
    }
}
