using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using WebApplication.GA.CommonClass;
using WebApplication.GA.CommonClass.Produce;
using Order = WebApplication.GA.CommonClass.Produce.Order;
using Product = WebApplication.GA.CommonClass.Product;

namespace WebApplication.GA
{
    class ToolUtil
    {
        public static Dictionary<int, Product> productsDict = new Dictionary<int, Product>();
        public static Dictionary<int, MachineMessage> machineMessageDict = new Dictionary<int, MachineMessage>();
        public static Dictionary<int, List<ProductCapacity>> productCapacityDict = new Dictionary<int, List<ProductCapacity>>();
        public static List<List<List<double>>> changeTime = new List<List<List<double>>>();   //生产线切换产品耗时
        public static int productCount;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            machineInit();
            productInit();
            changeTimeInit();
            productCapacityDict = new Dictionary<int, List<ProductCapacity>>();
            for (int i = 1; i <= productCount; i++)
            {
                productCapacityDict.Add(i, getProductCapacity(i));
            }
        }

        /// <summary>
        /// 获取设备前后工序约束
        /// </summary>
        /// <returns></returns>
        //public static List<EveryMachineLimit> getLimit()
        //{
        //    List<EveryMachineLimit> MachineSelectLimit = new List<EveryMachineLimit>();
        //    int id = 0;
        //    MachineSelectLimit.Add(new EveryMachineLimit()
        //    {
        //        Id = id++,
        //        MachineLimits = new List<EveryMachineGroupLimit>()
        //        {
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 1,
        //                Nums = 12,
        //                NextGruopId = 1
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 2,
        //                Nums = 10,
        //                NextGruopId = 2
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 3,
        //                Nums = 12,
        //                NextGruopId = 3
        //            }
        //        }
        //    });
        //    MachineSelectLimit.Add(new EveryMachineLimit()
        //    {
        //        Id = id++,
        //        MachineLimits = new List<EveryMachineGroupLimit>()
        //        {
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 1,
        //                Nums = 4,
        //                NextGruopId = 1
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 2,
        //                Nums = 2,
        //                NextGruopId = 2
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 3,
        //                Nums = 2,
        //                NextGruopId = 3
        //            }
        //        }
        //    });
        //    MachineSelectLimit.Add(new EveryMachineLimit()
        //    {
        //        Id = id++,
        //        MachineLimits = new List<EveryMachineGroupLimit>()
        //        {
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 1,
        //                Nums = 2,
        //                NextGruopId = 1
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 2,
        //                Nums = 1,
        //                NextGruopId = 2
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 3,
        //                Nums = 2,
        //                NextGruopId = 3
        //            }
        //        }
        //    });
        //    MachineSelectLimit.Add(new EveryMachineLimit()
        //    {
        //        Id = id++,
        //        MachineLimits = new List<EveryMachineGroupLimit>()
        //        {
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 1,
        //                Nums = 12,
        //                NextGruopId = 1
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 2,
        //                Nums = 7,
        //                NextGruopId = 2
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 3,
        //                Nums = 11,
        //                NextGruopId = 2
        //            }
        //        }
        //    });
        //    MachineSelectLimit.Add(new EveryMachineLimit()
        //    {
        //        Id = id++,
        //        MachineLimits = new List<EveryMachineGroupLimit>()
        //        {
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 1,
        //                Nums = 5,
        //                NextGruopId = 1
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 2,
        //                Nums = 7,
        //                NextGruopId = 2
        //            }
        //        }
        //    });
        //    MachineSelectLimit.Add(new EveryMachineLimit()
        //    {
        //        Id = id++,
        //        MachineLimits = new List<EveryMachineGroupLimit>()
        //        {
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 1,
        //                Nums = 5,
        //                NextGruopId = 1
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 2,
        //                Nums = 7,
        //                NextGruopId = 2
        //            }
        //        }
        //    });
        //    MachineSelectLimit.Add(new EveryMachineLimit()
        //    {
        //        Id = id++,
        //        MachineLimits = new List<EveryMachineGroupLimit>()
        //        {
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 1,
        //                Nums = 19,
        //                NextGruopId = 1
        //            },
        //            new EveryMachineGroupLimit()
        //            {
        //                GroupId = 2,
        //                Nums = 22,
        //                NextGruopId = 2
        //            }
        //        }
        //    });
        //    return MachineSelectLimit;
        //}

        /// <summary>
        /// 设备参数初始化
        /// </summary>
        public static void machineInit()
        {
            //List<EveryMachineLimit> MachineSelectLimit = getLimit();

            BasicClasses.FactoryOperate factoryoperator = new BasicClasses.FactoryOperate();
            double[] yieldrates = new double[7];
            double[] runningrates = new double[7];
            double[] workinghours = new double[7];
            factoryoperator.ProcedureRateInit(yieldrates, runningrates, workinghours);//读取其他配置表

            DataSet dataSet = BasicClasses.Util.ExcelToDS(MainDeal.path + "4机器生产参数配置表模板.xls", 0);
            DataSet dataSet_1 = BasicClasses.Util.ExcelToDS(MainDeal.path + "1工厂机器数量配置表模板.xls", 0);
            List<int> MachineNum = new List<int>();
            for (int i = 1; i <= 7; i++)
                MachineNum.Add((int)(double)dataSet.Tables[0].Rows[1].ItemArray[i]);


            machineMessageDict = new Dictionary<int, MachineMessage>();
            for (int i = 1; i <= dataSet.Tables[0].Rows.Count / 8; i++)
            {
                MachineMessage temp = new MachineMessage();
                temp.Ids = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
                temp.MachineNames = new List<string> { "清梳联", "预并条", "条并卷", "精梳机", "末并条机", "粗纱机", "细纱机" };
                temp.MachineNum = MachineNum;
                temp.YieldRates = yieldrates.ToList();
                temp.RunninGrates = runningrates.ToList();
                temp.WorkingHours = workinghours.ToList();

                temp.OutputLength = new List<double>();
                temp.OutputSpeed = new List<double>();
                temp.InputPortAmount = new List<int>();
                temp.OutputPortAmount = new List<int>();
                temp.InPrepareTime = new List<double>();
                temp.OutRemoveTime = new List<double>();
                temp.Unitouttime = new List<double>();

                for (int ii = 1; ii <= 7; ii++)
                {
                    temp.OutputLength.Add((double)dataSet.Tables[0].Rows[8 * (i - 1) + ii].ItemArray[1]);
                    temp.OutputSpeed.Add((double)dataSet.Tables[0].Rows[8 * (i - 1) + ii].ItemArray[2]);
                    temp.InputPortAmount.Add((int)(double)dataSet.Tables[0].Rows[8 * (i - 1) + ii].ItemArray[3]);
                    temp.OutputPortAmount.Add((int)(double)dataSet.Tables[0].Rows[8 * (i - 1) + ii].ItemArray[4]);
                    temp.InPrepareTime.Add((double)dataSet.Tables[0].Rows[8 * (i - 1) + ii].ItemArray[6]);
                    temp.OutRemoveTime.Add((double)dataSet.Tables[0].Rows[8 * (i - 1) + ii].ItemArray[7]);
                    temp.Unitouttime.Add(temp.OutputLength.Last() / temp.OutputSpeed.Last());
                }
                //temp.MachineSelectLimits = MachineSelectLimit;

                machineMessageDict.Add(i, temp);
            }
        }

        /// <summary>
        /// 产品参数初始化
        /// </summary>
        public static void productInit()
        {
            DataSet dataSet = BasicClasses.Util.ExcelToDS(MainDeal.path + "5产品干湿重g_m模板.xls", 0);
            productsDict = new Dictionary<int, Product>();

            for(int i = 1; i < dataSet.Tables[0].Rows.Count; i++)
            {
                List<double> w = new List<double>();
                List<double> q = new List<double>();
                for(int j = 1; j <= 7; j++)
                {
                    w.Add((double)dataSet.Tables[0].Rows[i].ItemArray[2 * j]);
                    q.Add((double)dataSet.Tables[0].Rows[i].ItemArray[2 * j + 1]);
                }

                Product temp = new Product()
                {
                    Id = i,
                    Name = (string)dataSet.Tables[0].Rows[i].ItemArray[1],
                    WetQuantity = w,
                    DryQuantity = q
                };
                productsDict.Add(i, temp);
            }
            productCount = productsDict.Count;
        }

        public static void changeTimeInit()
        {
            DataSet dataSet = BasicClasses.Util.ExcelToDS(MainDeal.path + "6各工序切换产品所需时间表模板.xls", 0);
            for (int i = 0; i < 7; i++)
            {
                // 7*4*4
                changeTime.Add(new List<List<double>>());
                for (int j = 1; j <= productCount; j++)
                {
                    changeTime.Last().Add(new List<double>());
                    for (int k = 1; k <= productCount; k++)
                        changeTime.Last().Last().Add(Math.Round(Convert.ToDouble(dataSet.Tables[i].Rows[j].ItemArray[k]), 2) * 60);
                }
            }
        }

        /// <summary>
        /// 品种单台设备产能
        /// </summary>
        /// <param name="id">产品序号</param>
        public static List<ProductCapacity> getProductCapacity(int id)
        {
            List<ProductCapacity> productCapacity = new List<ProductCapacity>();
            for (int i = 0; i < 7; i++)
            {
                ProductCapacity temp = new ProductCapacity();
                temp.Id = id;
                temp.MachineName = machineMessageDict[id].MachineNames[i];
                temp.MachineId = i;
                temp.OutTubWeight = machineMessageDict[id].OutputLength[i] * productsDict[id].WetQuantity[i] / 1000;
                temp.InputNum = machineMessageDict[id].InputPortAmount[i];
                if (i == 0)
                {
                    temp.OutputNum = machineMessageDict[id].OutputPortAmount[i];
                    temp.InputWeight = temp.OutTubWeight * temp.OutputNum / machineMessageDict[id].YieldRates[i];
                }
                else
                {
                    temp.InputWeight = machineMessageDict[id].InputPortAmount[i] * machineMessageDict[id].OutputLength[i - 1]
                    * (productsDict[id].WetQuantity[i - 1] / 1000);
                    temp.OutputNum = (int)(temp.InputWeight * machineMessageDict[id].YieldRates[i] / temp.OutTubWeight);
                }
                temp.DoffingTimes = (int)Math.Ceiling(temp.OutputNum / Convert.ToDouble(machineMessageDict[id].OutputPortAmount[i]));
                temp.OutputWeight = Math.Round(temp.OutputNum * temp.OutTubWeight, 4);
                if (i == 0)
                    temp.Yield = 1;
                else
                    temp.Yield = temp.OutputWeight / temp.InputWeight;

                if (i == 6)
                {
                    double speed = machineMessageDict[id].OutputSpeed[i];
                }
                temp.RunTime = Math.Round(machineMessageDict[id].InPrepareTime[i] + machineMessageDict[id].OutRemoveTime[i]
                    + temp.DoffingTimes * (machineMessageDict[id].OutputLength[i] / machineMessageDict[id].OutputSpeed[i]), 4) 
                    / machineMessageDict[id].RunninGrates[i];
                productCapacity.Add(temp);
            }

            Console.WriteLine("生产产品" + productsDict[id].Name + "数据：");
            foreach (var t in productCapacity)
                t.toString();
            return productCapacity;
        }

        /// <summary>
        /// 倒推求解生产目标重量，各工序所需设备运行次数
        /// </summary>
        /// <param name="targetWeigth">目标重量kg</param>
        /// <param name="id">产品序号</param>
        public static List<double> getMachineRunningNum(double targetWeigth, int id)
        {
            List<ProductCapacity> productCapacity = productCapacityDict[id];
            List<double> machineNeedProduceNum = new List<double>();
            double n = -1;
            double temp = targetWeigth;
            for (int i = 6; i >= 0; i--)
            {
                if (i != 4)
                {
                    n = (int)Math.Floor(temp / productCapacity[i].OutputWeight);
                    while (n * productCapacity[i].OutputWeight < temp)
                        n++;
                }
                else   //末并分单双眼
                {
                    n = (int)Math.Floor(temp / productCapacity[i].OutputWeight);
                    while (n * productCapacity[i].OutputWeight < temp)
                        n += 0.5;
                }
                
                temp = n * productCapacity[i].InputWeight;
                machineNeedProduceNum.Insert(0, n);
            }

            Console.WriteLine("生产" + targetWeigth + " kg" + productsDict[id].Name + " 各工序所需设备台数：");
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine(machineMessageDict[id].MachineNames[i] + ":生产" + machineNeedProduceNum[i] + "次"
                    + " 输入管数：" + machineNeedProduceNum[i] * productCapacity[i].InputNum + "  "
                    + " 输出管数：" + machineNeedProduceNum[i] * productCapacity[i].OutputNum);
                Thread.Sleep(100);
            }
            return machineNeedProduceNum;
        }

        /// <summary>
        /// 将订单List转换为批次List
        /// </summary>
        /// <returns></returns>
        public static List<Batch> fromOrderToBatch(List<Order> orders)
        {
            List<Batch> batches = new List<Batch>();
            foreach(Order o in orders)
            {
                foreach (ProductDetail b in o.Detail)
                {
                    Batch batch_temp = new Batch();
                    batch_temp.OrderId = o.Id;
                    batch_temp.BatchDetail = b;
                    batches.Add(batch_temp);
                }
            }
            //优先级按照：1、时间优先；2、时间相同的情况下，小订单优先;3、按品种序号排序（1、2相似；3、4相似）
            batches = batches.OrderBy(o => o.BatchDetail.Id).OrderBy(o => o.BatchDetail.Weight).OrderBy(o => o.BatchDetail.Deadline).ToList();
            return batches;
        }

        /// <summary>
        /// 读取订单表
        /// </summary>
        /// <returns></returns>
        public static List<Order> OrderTableReader()
        {
            IFormatProvider ifp = new CultureInfo("zh-CN", true);

            List<Order> orders = new List<Order>();
            DataSet dataSet = BasicClasses.Util.ExcelToDS(MainDeal.path + "7订单表模板.xls", 0);
            int rowlength = dataSet.Tables[0].Rows.Count;//表的行数
            int i = 0;
            while (i < rowlength)
            {
                if (Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) == "订单编号")
                {
                    Order order = new Order(Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]), Convert.ToString(dataSet.Tables[0].Rows[i + 1].ItemArray[1]));
                    i += 3;//跳至两行后
                    List<ProductDetail> details = new List<ProductDetail>();
                    //开始读取订单内产品信息
                    while (Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) != "订单结束")
                    {
                        ProductDetail t = new ProductDetail(Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]),
                            Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]),
                            DateTime.ParseExact(Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[4]), "yyyyMMdd", ifp));
                        t.Weight = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[2]);
                        t.Upperlimitrate = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[3]);
                        i++;
                        details.Add(t);
                    }
                    order.Detail = details;
                    orders.Add(order);
                }
                i++;
            }
            return orders;
        }
    }
}
