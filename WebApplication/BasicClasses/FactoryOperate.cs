using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebApplication.BasicClasses.Machines;
using WebApplication.BasicClasses.Products;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TextileDemo.BasicClasses;

namespace WebApplication.BasicClasses
{
    class FactoryOperate
    {
        Factory factory;

        public Factory Factory { get => factory; set => factory = value; }
        private List<DataSet> batchChangeTimeTable;

        public void Init()
        {
            FactoryInit();
            Constants.Init();
            MachineInit();
            AVGInit();
            ProductInit();
            LineInit();
            BatchChangeTimeTableInit();
        }

        public void FactoryInit()
        {
            Factory = new Factory();
        }
        //生成并配置机器
        public void MachineInit()
        {
            List<MachineGroup> machineGroups = new List<MachineGroup>();
            DataSet dataSet = Util.ExcelToDS(MainDeal.path + "4机器生产参数配置表模板.xls", 0);
            //建立机器组list，不同品种的机器配置进入不同的机器组，
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i += 8)
            {//每八行一个品种
                int type = i / 8;

                MachineGroup machineGroup = new MachineGroup(type, 0);
                //清梳联
                MBlowingcarding mBlowingcarding = new MBlowingcarding("QSL_" + type, "清梳联" + type, 1, 0);
                mBlowingcarding.Length = Convert.ToInt32(dataSet.Tables[0].Rows[i + 1].ItemArray[1]);
                mBlowingcarding.Outspeed = Convert.ToDouble(dataSet.Tables[0].Rows[i + 1].ItemArray[2]);
                mBlowingcarding.Inputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 1].ItemArray[3]);
                mBlowingcarding.Outputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 1].ItemArray[4]);
                mBlowingcarding.Unitouttime = mBlowingcarding.Length / mBlowingcarding.Outspeed;
                mBlowingcarding.Inputpreparetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 1].ItemArray[6]);
                mBlowingcarding.Outremovetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 1].ItemArray[7]);
                machineGroup.Machines.Add(mBlowingcarding);
                //预并条机
                MPredrawing mPredrawing = new MPredrawing("YBT_" + type, "预并条" + type, 2, 0);
                mPredrawing.Length = Convert.ToInt32(dataSet.Tables[0].Rows[i + 2].ItemArray[1]);
                mPredrawing.Outspeed = Convert.ToDouble(dataSet.Tables[0].Rows[i + 2].ItemArray[2]);
                mPredrawing.Inputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 2].ItemArray[3]);
                mPredrawing.Outputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 2].ItemArray[4]);
                mPredrawing.Unitouttime = mPredrawing.Length / mPredrawing.Outspeed;
                mPredrawing.Inputpreparetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 2].ItemArray[6]);
                mPredrawing.Outremovetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 2].ItemArray[7]);
                machineGroup.Machines.Add(mPredrawing);
                //条并卷机
                MStriptoroll mStriptoroll = new MStriptoroll("TBJ_" + type, "条并卷" + type, 3, 0);
                mStriptoroll.Length = Convert.ToInt32(dataSet.Tables[0].Rows[i + 3].ItemArray[1]);
                mStriptoroll.Outspeed = Convert.ToDouble(dataSet.Tables[0].Rows[i + 3].ItemArray[2]);
                mStriptoroll.Inputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 3].ItemArray[3]);
                mStriptoroll.Outputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 3].ItemArray[4]);
                mStriptoroll.Unitouttime = mStriptoroll.Length / mStriptoroll.Outspeed * mStriptoroll.Outputamount;
                mStriptoroll.Inputpreparetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 3].ItemArray[6]);
                mStriptoroll.Outremovetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 3].ItemArray[7]);
                machineGroup.Machines.Add(mStriptoroll);
                //精梳机
                MComber mComber = new MComber("JSJ_" + type, "精梳机" + type, 4, 0);
                mComber.Length = Convert.ToInt32(dataSet.Tables[0].Rows[i + 4].ItemArray[1]);
                mComber.Outspeed = Convert.ToDouble(dataSet.Tables[0].Rows[i + 4].ItemArray[2]);
                mComber.Inputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 4].ItemArray[3]);
                mComber.Outputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 4].ItemArray[4]);
                mComber.Unitouttime = mComber.Length / mComber.Outspeed;
                mComber.Inputpreparetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 4].ItemArray[6]);
                mComber.Outremovetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 4].ItemArray[7]);
                machineGroup.Machines.Add(mComber);
                //末并条机
                MEnddrawing mEnddrawing = new MEnddrawing("MBT_" + type, "末并条" + type, 5, 0);
                mEnddrawing.Length = Convert.ToInt32(dataSet.Tables[0].Rows[i + 5].ItemArray[1]);
                mEnddrawing.Outspeed = Convert.ToDouble(dataSet.Tables[0].Rows[i + 5].ItemArray[2]);
                mEnddrawing.Inputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 5].ItemArray[3]);
                mEnddrawing.Outputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 5].ItemArray[4]);
                mEnddrawing.Unitouttime = mEnddrawing.Length / mEnddrawing.Outspeed;
                mEnddrawing.Inputpreparetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 5].ItemArray[6]);
                mEnddrawing.Outremovetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 5].ItemArray[7]);
                machineGroup.Machines.Add(mEnddrawing);
                //粗纱机
                MBobbiner mBobbiner = new MBobbiner("CSJ_" + type, "粗纱机" + type, 6, 0);
                mBobbiner.Length = Convert.ToInt32(dataSet.Tables[0].Rows[i + 6].ItemArray[1]);
                mBobbiner.Outspeed = Convert.ToDouble(dataSet.Tables[0].Rows[i + 6].ItemArray[2]);
                mBobbiner.Inputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 6].ItemArray[3]);
                mBobbiner.Outputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 6].ItemArray[4]);
                mBobbiner.Unitouttime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 6].ItemArray[5]);
                mBobbiner.Inputpreparetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 6].ItemArray[6]);
                mBobbiner.Outremovetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 6].ItemArray[7]);
                machineGroup.Machines.Add(mBobbiner);
                //细纱机
                MSpinner mSpinner = new MSpinner("XSJ_" + type, "细纱机" + type, 7, 0);
                mSpinner.Length = Convert.ToInt32(dataSet.Tables[0].Rows[i + 7].ItemArray[1]);
                mSpinner.Outspeed = Convert.ToDouble(dataSet.Tables[0].Rows[i + 7].ItemArray[2]);
                mSpinner.Inputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 7].ItemArray[3]);
                mSpinner.Outputamount = Convert.ToInt32(dataSet.Tables[0].Rows[i + 7].ItemArray[4]);
                mSpinner.Unitouttime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 7].ItemArray[5]);
                mSpinner.Inputpreparetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 7].ItemArray[6]);
                mSpinner.Outremovetime = Convert.ToDouble(dataSet.Tables[0].Rows[i + 7].ItemArray[7]);
                machineGroup.Machines.Add(mSpinner);
                machineGroups.Add(machineGroup);
            }

            Factory.Machinegroups = machineGroups;
            foreach (MachineGroup m in Factory.Machinegroups)
            {
                Console.WriteLine("机器数量" + m.Machines.Count);
            }
        }

        public void AVGInit()
        {
            List<AGV> avgs = new List<AGV>();
            for (int i = 0; i < Constants.AVG_NUMS; i++)
            {
                AGV aVG = new AGV(i.ToString(), 0);
                avgs.Add(aVG);
            }
            Factory.Avgs = avgs;
        }

        //设置产品的参数
        public void ProductInit()
        {
            Factory.Productlists = new List<List<Product>>();
            DataSet dataSet = Util.ExcelToDS(MainDeal.path + "5产品干湿重g_m模板.xls", 1);
            List<double[]> parameters = new List<double[]>();
            int typenum = dataSet.Tables[0].Rows.Count;//产品种类总数
            for (int i = 0; i < typenum; i++)
            {
                double[] p = new double[14];
                for (int j = 0; j < 14; j++)
                {
                    p[j] = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[j + 2]);
                }
                parameters.Add(p);
            }
            for (int i = 0; i < typenum; i++)
            {
                List<Product> products = new List<Product>();
                Cottoncardingcylinder cottoncardingcylinder = new Cottoncardingcylinder("", "梳棉筒", 1, parameters[i][0], parameters[i][1]);
                products.Add(cottoncardingcylinder);
                Precylinder precylinder = new Precylinder("", "预并筒", 2, parameters[i][2], parameters[i][3]);
                products.Add(precylinder);
                Striptoroll striptoroll = new Striptoroll("", "条并卷", 3, parameters[i][4], parameters[i][5]);
                products.Add(striptoroll);
                Combtute combtute = new Combtute("", "精梳筒", 4, parameters[i][6], parameters[i][7]);
                products.Add(combtute);
                Enddrawingtube enddrawingtube = new Enddrawingtube("", "末并筒", 5, parameters[i][8], parameters[i][9]);
                products.Add(enddrawingtube);
                Rovingbobbin rovingbobbin = new Rovingbobbin("", "粗砂管", 6, parameters[i][10], parameters[i][11]);
                products.Add(rovingbobbin);
                Ringbobbin ringbobbin = new Ringbobbin("", "细纱管", 7, parameters[i][12], parameters[i][13]);
                products.Add(ringbobbin);
                Factory.Productlists.Add(products);
            }
        }

        //设置生产线机器数量配置
        public void LineInit()
        {
            DataSet dataSet = Util.ExcelToDS(MainDeal.path + "2生产线配置表模板.xls", 1);
            int rowlength = dataSet.Tables[0].Rows.Count;
            double[,] machinenums = new double[rowlength, 7];

            for (int i = 0; i < rowlength; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    machinenums[i, j] = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[j + 1]);
                }
            }

            List<Line> lines = new List<Line>();
            for (int i = 0; i < rowlength; i++)
            {
                double[] machines = new double[7];
                for (int j = 0; j < 7; j++)
                {
                    machines[j] = machinenums[i, j];
                }
                Line line = new Line(i, machines);
                lines.Add(line);
            }
            Factory.Lines = lines;
        }

        //生成订单(内部写死，不会用到)
        public Order OrderGenerator(string orderid)
        {
            IFormatProvider ifp = new CultureInfo("zh-CN", true);
            Order order = new Order(orderid, "甲方一号");

            //订单内容
            int TEXTILETYPE_NUMS = 3;
            Textile[] textiles = new Textile[TEXTILETYPE_NUMS];

            textiles[0] = new Textile(1, "JC9.7紧澳棉", DateTime.ParseExact("20191010", "yyyyMMdd", ifp));
            textiles[1] = new Textile(2, "JC8.4紧澳棉", DateTime.ParseExact("20191110", "yyyyMMdd", ifp));
            textiles[2] = new Textile(3, "JC9.7紧新疆棉", DateTime.ParseExact("20191210", "yyyyMMdd", ifp));
            for (int i = 0; i < TEXTILETYPE_NUMS; i++)
            {
                textiles[i].Weight = 40000;
                textiles[i].Upperlimitrate = 0.05;
                textiles[i].Upperlimit = Math.Round(textiles[i].Weight * (1 + textiles[i].Upperlimitrate), 2);
            }
            order.Textiles = new List<Textile>(textiles);
            return order;
        }

        //读取订单表
        public List<Order> OrderTableReader()
        {
            IFormatProvider ifp = new CultureInfo("zh-CN", true);

            List<Order> orders = new List<Order>();
            DataSet dataSet = Util.ExcelToDS(MainDeal.path + "7订单表模板.xls", 0);
            //DataSet dataSet = Util.ExcelToDS(Program.path + "订单配置Demo1_tight.xls", 0);
            //DataSet dataSet = Util.ExcelToDS(Program.path + "订单配置Demo2.xls", 0);
            int rowlength = dataSet.Tables[0].Rows.Count;//表的行数
            int i = 0;
            while (i < rowlength)
            {
                if (Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) == "订单编号")
                {
                    Order order = new Order(Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]), Convert.ToString(dataSet.Tables[0].Rows[i + 1].ItemArray[1]));
                    i += 3;//跳至两行后
                    List<Textile> textiles = new List<Textile>();
                    //开始读取订单内产品信息
                    while (Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) != "订单结束")
                    {
                        Textile t = new Textile(Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]),
                            Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]),
                            DateTime.ParseExact(Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[4]), "yyyyMMdd", ifp));
                        t.Weight = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[2]);
                        t.Upperlimitrate = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[3]);
                        t.Upperlimit = Math.Round(t.Weight * (1 + t.Upperlimitrate), 2);
                        t.Orderid = order.Id;
                        i++;
                        textiles.Add(t);
                    }
                    order.Textiles = textiles;
                    orders.Add(order);
                }
                i++;
            }
            return orders;
        }
        //制成率,设备运转率设备每日运行时间
        public void ProcedureRateInit(double[] yieldrates, double[] runningrates, double[] workinghours)
        {
            DataSet dataSet = Util.ExcelToDS(MainDeal.path + "3各工序制成率_设备运转率_设备运行时间表模板.xls", 1);
            for (int i = 0; i < 7; i++)
            {
                yieldrates[i] = Convert.ToDouble(dataSet.Tables[0].Rows[0].ItemArray[i + 1]);
            }
            for (int i = 0; i < 7; i++)
            {
                runningrates[i] = Convert.ToDouble(dataSet.Tables[0].Rows[1].ItemArray[i + 1]);
            }
            for (int i = 0; i < 7; i++)
            {
                workinghours[i] = Convert.ToDouble(dataSet.Tables[0].Rows[2].ItemArray[i + 1]);
            }
        }

        //根据订单集合创建批次(同种类的合并)
        public List<Batch> OrderCreateBatch(Order[] orders)
        {
            List<Batch> batches = new List<Batch>();

            List<Textile> textiles = new List<Textile>();//全部订单中包含的纱的种类及其质量
            foreach (Order order in orders)
            {
                Textile[] ordertextiles = order.Textiles.ToArray();//现在这一订单中包含的纱
                foreach (Textile textile in ordertextiles)
                {
                    if (textiles.Count != 0)
                    {
                        bool flag = true;

                        for (int i = 0; i < textiles.Count; i++)
                        {
                            if (textile.Type == textiles[i].Type)
                            {
                                textiles[i].Weight += textile.Weight;
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            Textile tmp = new Textile(textile.Type, textile.Name);
                            tmp.Weight += textile.Weight;
                            tmp.Upperlimitrate = textile.Upperlimitrate;
                            textiles.Add(tmp);
                        }
                    }
                    else
                    {
                        Textile tmp = new Textile(textile.Type, textile.Name);
                        tmp.Weight += textile.Weight;
                        tmp.Upperlimitrate = textile.Upperlimitrate;
                        textiles.Add(tmp);
                    }
                }
            }
            foreach (Textile textile in textiles)
            {
                Batch batch = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"), textile.Type);
                batch.Ordersumweight = Math.Round(textile.Weight * (1 + textile.Upperlimitrate), 2);
                batches.Add(batch);
            }
            foreach (Batch batch in batches)
            {
                foreach (Order order in orders)
                {
                    foreach (Textile textile in order.Textiles)
                    {
                        if (batch.Type == textile.Type)
                        {
                            batch.Orders.Add(order);
                            break;
                        }
                    }
                }
            }
            return batches;
        }

        // 不合并，boolean代表要不要分订单
        public List<Batch> CreateInitialBatch(Order[] orders, Boolean split)
        {
            LineInit();//0903
            List<Batch> batchlist = new List<Batch>();
            List<Textile> textileList = new List<Textile>();
            foreach (Order singleOrder in orders)    // add all textiles to the big list
            {
                textileList.AddRange(singleOrder.Textiles.ToArray());
            }
            foreach (Textile textile in textileList)
            {
                Batch batch = new Batch("Batch" + DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString(), textile.Type);
                batch.Ordersumweight = Math.Round(textile.Weight * (1 + textile.Upperlimitrate), 2);
                batch.Orderweight = Math.Round(textile.Weight, 2);
                batch.Orders.Add(orders.Where(o => o.Id == textile.Orderid).FirstOrDefault());

                double[] predictTime = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    ArrangeProcedure(batch, i);
                    predictTime[i] = batch.Predicttime;
                }
                LineInit();
                TimeSpan ts = textile.Deadline.Subtract(DateTime.Now);
                double m = Convert.ToInt32(Math.Ceiling(ts.TotalDays));
                double timeToDdl = m * 60 * 24;     //Time to ddl in minutes
                //Console.WriteLine("predict time average(min):" + predictTime.Average());
                //Console.WriteLine("Days from now to ddl(min):" + timeToDdl);
                if (predictTime.Average() > timeToDdl && split)
                {// split order
                    Console.WriteLine("Splits an big batch");
                    //Console.WriteLine(batch.ToString());
                    double newsumweight = batch.Ordersumweight / 2;
                    double newweight = batch.Orderweight / 2;
                    Batch batch1 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"), textile.Type);
                    batch1.Ordersumweight = Math.Round(newsumweight, 2);
                    batch1.Orderweight = Math.Round(newweight, 2);
                    batch1.Orders.Add(batch.Orders[0]);
                    Batch batch2 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"), textile.Type);
                    batch2.Ordersumweight = Math.Round(newsumweight, 2);
                    batch2.Orderweight = Math.Round(newweight, 2);
                    batch2.Orders.Add(batch.Orders[0]);
                    batchlist.Add(batch1);
                    batchlist.Add(batch2);
                    ArrangeProcedure(batch1, 0);
                    ArrangeProcedure(batch2, 0);
                    Console.WriteLine(batch1.ToString());
                    Console.WriteLine(batch2.ToString());
                }
                else
                {
                    batchlist.Add(batch);
                }
            }
            LineInit();
            return batchlist;
        }

        public Boolean CheckOrderFeasibility(List<Batch> initialBatchs, double[,] orderddltable, double[] yieldrates,
                                                double[] runningrates, double[] workinghours)
        {
            foreach (Batch singleBatch in initialBatchs)
            {
                List<Line> templines = new List<Line>();    //针对每一个batch，放在三个生产线上的情况
                for (int i = 0; i < Factory.Lines.Count; i++)
                {
                    Batch batch1 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"));
                    batch1.Type = singleBatch.Type;
                    batch1.Ordersumweight = singleBatch.Ordersumweight / Factory.Lines.Count;
                    Line templine = new Line(i);
                    templine.Machines = Factory.Lines[i].Machines;
                    templines.Add(templine);
                    BatchArrangeLine(batch1, templine);
                    LineArrangeProcedure(templine, yieldrates, runningrates, workinghours);
                }

                //test
                //Line testline = new Line(0);
                //testline.Machines = new double[7];
                //for (int j = 0; j < 7; j++)
                //{

                //    testline.Machines[j] = Factory.Lines[0].Machines[j];
                //}
                //Batch batch1 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"));
                //batch1.Type = singleBatch.Type;
                //batch1.Ordersumweight = singleBatch.Ordersumweight / Factory.Lines.Count;
                //Batch batch2 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"));
                //batch2.Type = singleBatch.Type;
                //batch2.Ordersumweight = singleBatch.Ordersumweight / Factory.Lines.Count;
                //Batch batch3 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"));
                //batch3.Type = singleBatch.Type;
                //batch3.Ordersumweight = singleBatch.Ordersumweight / Factory.Lines.Count;
                //BatchArrangeLine(batch1, testline);
                //BatchArrangeLine(batch2, testline);
                //BatchArrangeLine(batch3, testline);
                //LineArrangeProcedure(testline);
                //for (int i = 0; i< templines.Count;i++)
                //{
                //    for (int j = 0; j< templines[i].Batches.Count; j++)
                //    {
                //        Console.WriteLine(templines[i].Batches[j].ToString());

                //    }

                //}
                //for (int j = 0; j < testline.Batches.Count; j++)
                //{
                //    Console.WriteLine(testline.Batches[j].ToString());

                //}
                //FileStream fs = new FileStream("./testoutput.txt", FileMode.Create);
                //StreamWriter sw = new StreamWriter(fs);
                //double[,] testdata = LineProduceTable(testline, orderddltable.GetLength(1));
                //for (int i = 0; i < testdata.GetLength(0); i++)
                //{
                //    for (int j = 0; j < testdata.GetLength(1); j++)
                //    {
                //        sw.Write(testdata[i, j] + "\t");
                //        Console.Write(testdata[i, j] + " ");
                //    }
                //    sw.Write("\n");
                //    Console.Write("\n");
                //}
                //sw.Flush();
                //sw.Close();
                //fs.Close();
                //FileStream fs = new FileStream("./testoutput.txt", FileMode.Create);
                //StreamWriter sw = new StreamWriter(fs);
                //FileStream fs = new FileStream("./testoutput.txt", FileMode.Create);
                //for (int lineIndex = 0; lineIndex < templines.Count; lineIndex ++)
                //{
                //    //Console.WriteLine(templines[lineIndex].Batches.GetType());
                //    FileStream fs = new FileStream("./testoutput.txt", FileMode.Create);
                //    StreamWriter sw = new StreamWriter(fs);
                //    double[,] testdata = LineProduceTable(testline, orderddltable.GetLength(1));
                //    for (int i = 0; i < testdata.GetLength(0); i++)
                //    {
                //        for (int j = 0; j < testdata.GetLength(1); j++)
                //        {
                //            sw.Write(testdata[i, j] + "\t");
                //            Console.Write(testdata[i, j] + " ");
                //        }
                //        sw.Write("\n");
                //        Console.Write("\n");
                //    }
                //    sw.Flush();
                //    sw.Close();
                //    fs.Close();
                //}

                //Boolean singlefeasible = CheckLinesFeasibility(templines, orderddltable);
                //if (singlefeasible == false) {//只要有一个不过，就都不过
                //    Console.WriteLine("\n产能不足，无法完成本批订单\n");
                //    return false;
                //}
                //Console.WriteLine("Check order feasibility:");
                for (int l = 0; l < Factory.Lines.Count; l++)
                {
                    TimeSpan ts = DateTime.Now.Subtract(singleBatch.Deadline);
                    int allowedTime = Convert.ToInt32(Math.Ceiling(ts.TotalDays));
                    int dueType = templines[l].Batches[0].Type;
                    //Console.WriteLine("line = " + l);
                    //Console.WriteLine("due type = " + dueType);
                    //Console.WriteLine("allowed time = " + allowedTime);
                    //Console.WriteLine("predict time = "+ templines[l].Batches[0].Predicttime);
                    if (allowedTime < templines[l].Batches[0].Predicttime)
                    {
                        Console.WriteLine("\n产能不足，无法完成本批订单\n");
                    }
                }

            }
            LineInit(); //0903
            return true;

        }



        public List<List<Line>> ProducePossibileSoln(List<Batch> batches, int populationSize, int allowedTrialNum, double[,] orderddltable
                                                    , double[] yieldrates, double[] runningrates, double[] workinghours)
        {
            int currentPopulation = 0;
            List<List<Line>> possibleSoln = new List<List<Line>>();
            //0904 counter for trial
            int trialCounter = 0;
            while (currentPopulation < populationSize)
            {
                if (trialCounter > allowedTrialNum)
                {
                    Console.WriteLine("尝试次数超过允许上限，直接返回已生成种群");
                    return possibleSoln;
                }
                //trialCounter++;
                //0903 深拷贝batchlist
                List<Batch> batchPool = new List<Batch>();
                foreach (Batch batch in batches)
                {
                    batchPool.Add(Clone<Batch>(batch));
                }
                List<Line> tempLines = new List<Line>();//一个解, 全是空的生产线
                for (int i = 0; i < Factory.Lines.Count; i++)
                {
                    Line templine1 = new Line(i);
                    templine1.Machines = new double[7];
                    tempLines.Add(templine1);
                }
                for (int l = 0; l < Factory.Lines.Count; l++)//给每条生产线配机器
                {
                    for (int i = 0; i < 7; i++)
                    {
                        tempLines[l].Machines[i] = Factory.Lines[l].Machines[i];
                    }
                }
                //Console.WriteLine("空生产线生成完毕，随机分配任务开始");
                Stopwatch sw = new Stopwatch();
                while (batchPool.Count != 0)
                {//随机挑一个batch，一个line，安排上
                    sw.Start();
                    sw.Stop();
                    int seed = (int)sw.ElapsedTicks;
                    Random rd = new Random(seed);
                    int batchnum = rd.Next(0, batchPool.Count);
                    //Console.WriteLine("剩余未分配任务数：" + batchPool.Count);
                    //Console.WriteLine("选中任务index：" + batchnum);
                    int linenum = rd.Next(0, Factory.Lines.Count);
                    Batch tempBatch = batchPool[batchnum];
                    //Console.WriteLine("新选中的batch："+tempBatch.ToString());
                    //Console.WriteLine("In days:" + tempBatch.Predicttime / 60 / 23);
                    batchPool.RemoveAt(batchnum);
                    BatchArrangeLine(tempBatch, tempLines[linenum]);//这里的问题
                }
                for (int i = 0; i < Factory.Lines.Count; i++)
                {
                    foreach (Batch batch in tempLines[i].Batches)
                    {
                        Console.WriteLine(batch.ToString() + "change time: " + batch.Changetime + "\n");//这里也不对
                    }

                    LineArrangeProcedure(tempLines[i], yieldrates, runningrates, workinghours);
                }

                //Console.WriteLine("任务分配完毕");
                Boolean feasible = CheckLinesFeasibility(tempLines, orderddltable);
                if (feasible == true)
                {
                    Console.WriteLine("*********************************************找到一个可行解，加入初始种群, 本次前尝试次数：" + trialCounter);
                    possibleSoln.Add(tempLines);
                    trialCounter = 0;//试验次数赋回零
                    currentPopulation++;
                    //foreach (Line line in tempLines)
                    //{
                    //    Console.WriteLine(line.ToString());
                    //    foreach (Batch batch in line.Batches)
                    //    {
                    //        Console.WriteLine(batch.ToString());
                    //    }
                    //}
                }
                else
                {
                    //Console.WriteLine("当前解不符合订单约束");
                    trialCounter++;//试验次数加一
                }
            }
            return possibleSoln;
        }



        //为一个批次分配一条生产线,注意批次顺序！！！！，按续分配生产线
        public void BatchArrangeLine(Batch batch, Line line)
        {
            batch.Line = line;
            line.Batches.Add(batch);
            int index = line.Batches.IndexOf(batch);
            //在分配后设置batch之间的切换时间
            if (index != 0)
            {
                BatchChangeTime(line.Batches[index - 1], batch);
            }
        }

        //读取产品切换时间表，决定在某一条line上由batchA切换至batchB需要的时间
        public void BatchChangeTimeTableInit()
        {
            this.batchChangeTimeTable = new List<DataSet>();
            int linenums = this.Factory.Lines.Count;
            //需要读七页，changetime属性应为数组
            //string[] sheetnames = { "清梳联", "预并", "条并卷", "精梳", "末并", "粗纱", "细纱" };
            for (int i = 0; i < 7; i++)
            {
                DataSet dataSet = Util.ExcelToDS(MainDeal.path + "6各工序切换产品所需时间表模板.xls", i);
                this.batchChangeTimeTable.Add(dataSet);
            }
        }


        //决定在某一条line上由batchA切换至batchB需要的时间
        public void BatchChangeTime(Batch batchA, Batch batchB)
        {
            //循环读七页，根据每页对应的位置获取相应工序的切换时间
            int batchAtype = batchA.Type;
            int batchBtype = batchB.Type;
            for (int i = 0; i < 7; i++)
            {
                DataSet dataSet = this.batchChangeTimeTable[i];
                batchB.Changetime[i] = Math.Round(Convert.ToDouble(dataSet.Tables[i].Rows[batchAtype - 1].ItemArray[batchBtype]), 2) * 60;//得到的单位是h，需要转换为min
            }
        }

        //分配好生产线后，为一个具体的批次安排工序并计算出时间(可能不会用到)
        public void ArrangeProcedure(Batch batch, int lineNum)
        {
            BatchArrangeLine(batch, factory.Lines[lineNum]);
            List<Procedure> procedures = new List<Procedure>();
            double[] yieldrates = { 0.77, 0.74, 0.8, 0.95, 0.97, 0.98, 1 };//制成率
            double[] runningrates = { 0.91, 1, 0.92, 0.92, 0.93, 0.93, 0.96 };//设备运转率
            double[] workinghours = { 22.5, 22.5, 22.5, 22.5, 22.5, 22.5, 24 };//设备每日运行时间
            //从后向前算，得出生产量及消耗时间
            //先求出批次每管实际生产质量
            double tubeweight = Factory.Machinegroups[6].Machines[0].Length * Factory.Productlists[batch.Type - 1][6].Wetquantity / 1000;//每台细纱机每次生产1管的质量
            //每批次实际最终产量
            batch.Ordervirtualweight = Math.Round(Math.Floor(batch.Ordersumweight / Factory.Machinegroups[6].Machines[0].Outputamount / tubeweight) * tubeweight * Factory.Machinegroups[6].Machines[0].Outputamount, 2);

            for (int i = 6; i >= 0; i--)
            {
                Procedure procedure = new Procedure();
                procedure.Machinenum = batch.Line.Machines[i];//根据批次所在生产线该工序机器分配数量
                procedure.Machines = Factory.Machinegroups[i].Machines.GetRange(0, Convert.ToInt32(Math.Ceiling(procedure.Machinenum)));
                //设置工序制成率,设备运转率，每日运行时间
                procedure.Yieldrate = yieldrates[i];
                procedure.Runningrate = runningrates[i];
                procedure.Workinghours = workinghours[i];
                //设置当前工序应当输出总质量
                if (i == 6)//如果是细ss纱机
                {
                    procedure.Outputweight = batch.Ordervirtualweight;
                }
                else
                {
                    procedure.Outputweight = procedures[0].Inputweight;
                }
                //设置当前工序当前产品的湿重
                procedure.Outproduct = Factory.Productlists[batch.Type - 1][i];
                double wetweight = procedure.Outproduct.Wetquantity;
                //根据湿重获取当前工序应当生产的管数或筒数
                procedure.Outamount = Convert.ToInt32(Math.Floor(procedure.Outputweight / procedure.Machines[0].Length / wetweight * 1000));
                //求出每台机器生产次数
                int producetimes = Convert.ToInt32(Math.Ceiling((double)procedure.Outamount / procedure.Machinenum / procedure.Machines[0].Outputamount));
                //设置当前工序输入总质量
                procedure.Inputweight = Math.Round(producetimes * procedure.Machinenum * procedure.Machines[0].Outputamount * wetweight * procedure.Machines[0].Length / 1000 / procedure.Runningrate / procedure.Yieldrate, 2);
                //计算各种时间
                procedure.Outtime = Math.Round(procedure.Machines[0].Unitouttime * producetimes, 2);
                procedure.Suminpreparetime = Math.Round(procedure.Machines[0].Inputpreparetime * producetimes, 2);
                procedure.Sumoutremovetime = Math.Round(procedure.Machines[0].Outremovetime * producetimes, 2);
                procedure.Sumproceduretime = procedure.Outtime + procedure.Sumoutremovetime + procedure.Sumoutremovetime;

                procedures.Insert(0, procedure);
            }
            procedures[0].Starttime = 0;
            //计算开始时间
            for (int i = 1; i <= 6; i++)
            {
                //前一工序为了保障下一工序输入，需要的每台机器生产次数
                //int times = Convert.ToInt32(Math.Ceiling((double)procedures[i].Machines[0].Inputamount * procedures[i].Machinenum / procedures[i - 1].Machines[0].Outputamount / procedures[i - 1].Machinenum));
                int times = Convert.ToInt32(Math.Ceiling((double)procedures[i].Machines[0].Inputamount / procedures[i - 1].Machines[0].Outputamount / procedures[i - 1].Machinenum));
                procedures[i].Starttime = Math.Round(procedures[i - 1].Starttime + (procedures[i - 1].Machines[0].Unitouttime + procedures[i - 1].Machines[0].Inputpreparetime + procedures[i - 1].Machines[0].Outremovetime) * times, 2);
            }


            batch.Procedures = procedures;
            batch.Predicttime = procedures[6].Starttime + procedures[6].Sumproceduretime;
        }

        //计算一条生产线上各个批次工序的开始时间，结束时间
        public void LineArrangeProcedure(Line line, double[] yieldrates, double[] runningrates, double[] workinghours)
        {//制成率影响重量，运行率影响时间
            List<Batch> batches = line.Batches;
            for (int i = 0; i < batches.Count; i++)
            {
                int type = batches[i].Type - 1;
                List<Procedure> procedures = new List<Procedure>();
                //从后向前算，得出生产量及消耗时间
                //先求出批次每管实际生产质量
                double tubeweight = Factory.Machinegroups[type].Machines[6].Length * Factory.Productlists[type][6].Wetquantity / 1000;//每台细纱机每次生产1管的质量
                //每批次实际最终产量
                batches[i].Ordervirtualweight = Math.Round(Math.Floor(batches[i].Ordersumweight / Factory.Machinegroups[type].Machines[6].Outputamount / tubeweight) * tubeweight * Factory.Machinegroups[type].Machines[6].Outputamount, 2);
                if (batches[i].Ordervirtualweight < batches[i].Orderweight)
                {
                    batches[i].Ordervirtualweight = Math.Round(Math.Ceiling(batches[i].Ordersumweight / Factory.Machinegroups[type].Machines[6].Outputamount / tubeweight) * tubeweight * Factory.Machinegroups[type].Machines[6].Outputamount, 2);
                }

                for (int j = 6; j >= 0; j--)
                {
                    Procedure procedure = new Procedure();
                    procedure.Machinenum = batches[i].Line.Machines[j];//根据批次所在生产线该工序机器分配数量
                    List<Machine> tempmachines = new List<Machine>();
                    tempmachines.Add(Factory.Machinegroups[type].Machines[j]);
                    procedure.Machines = tempmachines;
                    //设置工序制成率,设备运转率，每日运行时间
                    procedure.Yieldrate = yieldrates[j];
                    procedure.Runningrate = runningrates[j];
                    procedure.Workinghours = workinghours[j];
                    //设置当前工序应当输出总质量
                    if (j == 6)//如果是细纱机
                    {
                        procedure.Outputweight = batches[i].Ordervirtualweight;
                    }
                    else
                    {
                        procedure.Outputweight = procedures[0].Inputweight;
                    }
                    //设置当前工序当前产品的湿重
                    procedure.Outproduct = Factory.Productlists[batches[i].Type - 1][j];
                    double wetweight = procedure.Outproduct.Wetquantity;
                    //根据湿重获取当前工序应当生产的管数或筒数
                    procedure.Outamount = Convert.ToInt32(Math.Ceiling(procedure.Outputweight / procedure.Machines[0].Length / wetweight * 1000));
                    //求出每台机器生产次数
                    int producetimes = Convert.ToInt32(Math.Ceiling((double)procedure.Outamount / procedure.Machinenum / procedure.Machines[0].Outputamount));
                    //设置当前工序输入总质量
                    procedure.Inputweight = Math.Round(producetimes * procedure.Machinenum * procedure.Machines[0].Outputamount * wetweight * procedure.Machines[0].Length / 1000 / procedure.Yieldrate, 2);

                    //计算各种时间
                    procedure.Outtime = Math.Round(procedure.Machines[0].Unitouttime * producetimes * 1.12 / procedure.Runningrate, 2);
                    //原料准备次数=原料总质量/原料每次投入质量
                    int preparetimes = 0;
                    if (j != 0)//如果不是清梳联
                    {
                        preparetimes = (Convert.ToInt32(Math.Ceiling(procedure.Inputweight / Factory.Machinegroups[type].Machines[j - 1].Length / Factory.Productlists[batches[i].Type - 1][j - 1].Wetquantity * 1000 / procedure.Machinenum / procedure.Machines[0].Inputamount)));
                    }
                    procedure.Suminpreparetime = Math.Round(procedure.Machines[0].Inputpreparetime * preparetimes, 2);
                    procedure.Sumoutremovetime = Math.Round(procedure.Machines[0].Outremovetime * producetimes, 2);
                    procedure.Sumproceduretime = procedure.Outtime + procedure.Sumoutremovetime + procedure.Suminpreparetime;

                    procedures.Insert(0, procedure);
                }

                //需要修改！！！！！！批次切换中按照工序决定切换时间
                //从细纱开始，与现在解决方法相同；从头开始，与现在不同，需要改为：紧接着上一批次中同工序继续生产，中间的切换时间间隔未知，不同工序切换时间相同吗？？同工序？？？？
                //切换后工序的机器参数会变化吗，是临时决定还是可以提前预知该批次的机器参数？？？？？
                if (i == 0)//如果是生产线中第一个批次
                {
                    procedures[0].Starttime = 0;
                }
                else//如果是生产线中后续的批次
                {
                    procedures[0].Starttime = batches[i - 1].Procedures[0].Starttime + batches[i - 1].Procedures[0].Sumproceduretime + batches[i].Changetime[0];
                }
                //计算开始时间
                for (int j = 1; j <= 6; j++)
                {
                    double num = Math.Ceiling(procedures[j - 1].Machinenum * 0.22);
                    int times = Convert.ToInt32(Math.Ceiling((double)procedures[j].Machines[0].Inputamount / procedures[j - 1].Machines[0].Outputamount / num));

                    while (times > num * 3)
                    {
                        num += 1;
                        if (num > procedures[j - 1].Machinenum)
                        {
                            num = procedures[j - 1].Machinenum;
                            break;
                        }
                    }
                    times = Convert.ToInt32(Math.Ceiling((double)procedures[j].Machines[0].Inputamount / procedures[j - 1].Machines[0].Outputamount / num));

                    if (i == 0)
                    {
                        //前一工序为了保障下一工序输入，需要的每台机器生产次数
                        procedures[j].Starttime = Math.Round(procedures[j - 1].Starttime + (procedures[j - 1].Machines[0].Unitouttime + procedures[j - 1].Machines[0].Inputpreparetime + procedures[j - 1].Machines[0].Outremovetime) * times, 2);

                    }
                    else
                    {
                        if (batches[i - 1].Procedures[j].Starttime + batches[i - 1].Procedures[j].Sumproceduretime + batches[i].Changetime[j] > procedures[j - 1].Starttime + (procedures[j - 1].Machines[0].Unitouttime + procedures[j - 1].Machines[0].Inputpreparetime + procedures[j - 1].Machines[0].Outremovetime) * times)
                        {
                            procedures[j].Starttime = batches[i - 1].Procedures[j].Starttime + batches[i - 1].Procedures[j].Sumproceduretime + batches[i].Changetime[j];
                        }
                        else
                        {
                            procedures[j].Starttime = Math.Round(procedures[j - 1].Starttime + (procedures[j - 1].Machines[0].Unitouttime + procedures[j - 1].Machines[0].Inputpreparetime + procedures[j - 1].Machines[0].Outremovetime) * times, 2);
                        }
                    }
                }
                batches[i].Procedures = procedures;
                batches[i].Predicttime = procedures[6].Starttime + procedures[6].Sumproceduretime;
            }
        }
        //****************************************************************************
        //检查一个解是否为可行解.输入：一组可能解+订单截止日期表。输出：布尔值
        public Boolean CheckLinesFeasibility(List<Line> templines, double[,] orderddltable)
        {
            /*　orderddltable例子：行数：typesNum+1，列数：daysNum+1
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
            { 0, 0, 0, 0, 50,0, 0, 0, 80,0}
            { 1, 0, 0, 0, 0 ,0, 0, 40,0, 0}
            { 2, 0, 20,0, 0 ,0, 0, 0, 0, 0}
            { 3, 0, 0, 0, 0 ,50,0, 0, 0, 0}*/

            /*　produceTable例子：行数：typesNum+1，列数：daysNum+1
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
            { 0, 0, 0, 0, 50,50,50,50,130,130}
            { 1, 0, 0, 0, 0 ,0, 0, 40,40, 40}
            { 2, 0, 20,20,20,20,20,20,20, 20}
            { 3, 0, 0, 0, 0 ,50,50,50,50, 50}*/

            /*　tempTable例子：行数：此line包含种类数+1，列数：daysNum+1
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
            { 1, 0, 0, 0, 0 ,0, 0, 40,40,40}
            { 3, 0, 0, 0, 0 ,50,50,50,50,50}*/

            int daysNum = orderddltable.GetLength(1) - 1;//列数-1=总天数
            //int typesNum = Factory.Productlists.Count;//行数-1=产品总类型数

            //计算当前schedule下的总产量，建立二维数组，行：type，列：天数
            double[,] produceTable = TotalProduceTable(templines, orderddltable);
            //0904：若orderddltable有这个type，才会查
            int includedTypesNum = orderddltable.GetLength(0) - 1;

            //test2
            //FileStream fs = new FileStream("./test2output.txt", FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);
            //for (int r = 0; r < produceTable.GetLength(0); r++)
            //{
            //    for (int c = 0; c < produceTable.GetLength(1); c++)
            //    {
            //        sw.Write(produceTable[r, c] + "\t");
            //        Console.Write(produceTable[r, c] + " ");
            //    }
            //    sw.Write("\n");
            //    Console.Write("\n");
            //}
            //sw.Flush();
            //sw.Close();
            //fs.Close();
            //test2
            //挨个ddl挨个种类检查，若每一天某一个产品的累计产量不达到orderddltable的要求，fail
            for (int i = 1; i < daysNum + 1; i++)
            {
                for (int j = 1; j <= includedTypesNum; j++)
                {
                    if (orderddltable[j, i] > 0.0)
                    {
                        int dueType = (int)orderddltable[j, 0];
                        double dueAmount = orderddltable[j, i];

                        //Console.WriteLine(dueType);
                        //Console.WriteLine("produced for type " + dueType + " = " + produceTable[dueType, i].ToString());
                        //Console.WriteLine("dueAmount for type " + dueType + " = " + dueAmount);

                        if (produceTable[dueType, i] < dueAmount / 1.05)//该种类总产量小于所需
                        {
                            //Console.WriteLine(produceTable[dueType, i].ToString(), dueAmount);
                            //Console.WriteLine(dueType);
                            //Console.WriteLine("produced for type " + dueType + " = " + produceTable[dueType, i].ToString());
                            //Console.WriteLine("dueAmount for type " + dueType + " = " + dueAmount);
                            return false;//不满足任何一次ddl都会fail
                        }
                        else//若此日此类产量满足，将总产量表内，从此日开始往后的此类产量值，全部减去dueAmount
                        {
                            for (int d = i; d < daysNum + 1; d++)
                            {
                                produceTable[j, d] -= dueAmount / 1.05;
                            }
                        }
                    }
                }
            }
            return true;//全部检查通过才会到这步
        }


        //整个工厂（所有生产线），截至最后的ddl，每类产品的累计产量=所有生产线产量表相加
        public double[,] TotalProduceTable(List<Line> templines, double[,] orderddltable)
        {
            int linesNum = templines.Count;//生产线条数
            int daysNum = orderddltable.GetLength(1) - 1;//列数-1=总天数
            int typesNum = Factory.Productlists.Count;//行数-1=产品总类型数
            //计算当前schedule下的总产量，建立二维数组，行：type，列：天数
            double[,] totalProduceTable = new double[typesNum + 1, daysNum + 1];

            //二维数组初始化，除了第一行（天数）和第一列（种类）

            for (int i = 0; i < typesNum + 1; i++)//全部初始化为0
            {
                for (int j = 0; j < daysNum + 1; j++)
                {
                    if (i == 0 && j != 0)//第一行为天数，第j列对应第j天。
                    {
                        totalProduceTable[i, j] = j;
                    }
                    else if (i != 0 && j == 0)//第一列为种类编号
                    {
                        totalProduceTable[i, j] = i;//第i行对应i种
                    }
                    else
                    {
                        totalProduceTable[i, j] = 0;
                    }
                }
            }
            //累加更新总产量表：同一天同一类，将三条线的产量加起来。
            for (int i = 0; i < linesNum; i++)
            {
                double[,] tempTable = LineProduceTable(templines[i], daysNum);//当前line的产量表

                //查看当前产量表里的种类的每天对应的值，将有值的加入总产量表
                for (int j = 1; j < tempTable.GetLength(0); j++)//行数，第i行对应i-1种
                {
                    for (int k = 1; k < daysNum + 1; k++)//天数，第j列对应第j天。
                    {
                        if (tempTable[j, k] > 0.0)
                        {
                            totalProduceTable[(int)tempTable[j, 0], k] += tempTable[j, k];//用当前line产量表给总产量表赋值
                        }
                    }

                }

            }
            //test: TotalProduceTable
            //for (int i = 0; i < Factory.Lines.Count; i++)
            //{
            //    double[,] testdata = totalProduceTable;
            //    FileStream fs = new FileStream("./totalproducetable.txt", FileMode.Create);
            //    StreamWriter sw = new StreamWriter(fs);
            //    for (int r = 0; r < testdata.GetLength(0); r++)
            //    {
            //        for (int c = 0; c < testdata.GetLength(1); c++)
            //        {
            //            sw.Write(testdata[r, c] + "\t");
            //            Console.Write(testdata[r, c] + " ");
            //        }
            //        sw.Write("\n");
            //        Console.Write("\n");
            //    }
            //    sw.Flush();
            //    sw.Close();
            //    fs.Close();
            //}
            return totalProduceTable;
        }


        public double CalculateScore(List<Line> singlesoln, double[,] orderddltable, double scoreParam)
        {
            int linesNum = singlesoln.Count;
            int daysNum = orderddltable.GetLength(1) - 1;//列数-1=总天数
            int typesNum = orderddltable.GetLength(0) - 1;//行数-1=产品总类型数
            double score = 0.0;
            int finishEarly = 0;
            double switchTime = 0;
            int[] checkStartDay = new int[typesNum];
            int i, j;
            for (i = 0; i < typesNum; i++)
            {
                checkStartDay[i] = 1;//初始化为第一天
            }
            double[,] totalProduceTable = TotalProduceTable(singlesoln, orderddltable);
            //计算总提前完成天数
            for (i = 1; i < daysNum + 1; i++)
            {
                for (j = 1; j < typesNum + 1; j++)
                {
                    if (orderddltable[j, i] > 0.0)//有ddl的天
                    {
                        int textileType = (int)orderddltable[j, 1];
                        double dueAmount = orderddltable[j, i];
                        int d;
                        for (d = checkStartDay[textileType]; d <= i; d++)
                        {
                            if (totalProduceTable[textileType + 1, d] > dueAmount)//此产品总产量，从上次满足上一个ddl产量要求后开始，第一次超过本次ddl所需交货量
                            {
                                checkStartDay[textileType] = d;//更新检查起始日期
                                finishEarly += i - d; //此日ddl所需产量提前了i-d天完成
                                break;
                            }
                        }
                        while (d < daysNum + 1)
                        {
                            totalProduceTable[j, d] -= dueAmount;//从满足产量当日（包括）开始，将此次需要的产量从表中减掉
                            d++;
                        }
                    }
                }
            }
            //计算总切换时间
            //for (i = 0; i < linesNum; i++)
            //{
            //    int batchesNum = singlesoln[i].Batches.Count;
            //    for (j = 0; j < batchesNum; j++)
            //    {
            //        switchTime += singlesoln[i].Batches[j].Changetime/24;
            //    }
            //}
            //score = finishEarly - scoreParam * switchTime;
            score = finishEarly;
            return score;
        }

        public int EliteScoreIndex(List<double> eliteScoreList)
        {
            int eliteIndex = -1;
            double eliteScore = -1;
            for (int i = 0; i < eliteScoreList.Count; i++)
            {
                if (eliteScoreList[i] > eliteScore)
                {
                    eliteScore = eliteScoreList[i];
                    eliteIndex = i;
                }
            }
            return eliteIndex;

        }
        //深复制要用的clone方法
        public static T Clone<T>(T RealObject)

        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
        public List<Line> Mutation(List<Line> oldGene, double mutationProb, int mutatNum, double[,] orderddltable, double scoreParam,
                                    double[] yieldrates, double[] runningrates, double[] workinghours)
        {
            // randomly choose 2 task, change
            //
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            int seed = (int)sw.ElapsedTicks;
            Random rd = new Random(seed);
            double randprob = rd.NextDouble();
            //foreach (Line oldline in oldGene) {
            //    foreach (Batch batch in oldline.Batches) {
            //        Console.WriteLine(batch.ToString() + "change time: " + batch.Changetime + "\n");//这个地方就不对
            //    }
            //}
            //深复制
            List<Line> newGene = new List<Line>();
            foreach (Line line in oldGene)
            {
                Line templine = Clone<Line>(line);
                newGene.Add(templine);
                //for (int j = 0; j < templine.Batches.Count; j++) {
                //    Console.WriteLine("type " + templine.Batches[j].Type + "change time: " + templine.Batches[j].Changetime);
                //}
                // Console.WriteLine(templine.ToString());
                //for (int j = 0; j <templine.Batches.Count; j++)
                //{
                //    BatchArrangeLine(templine.Batches[j], templine); ;
                //}
                //LineArrangeProcedure(templine, yieldrates, runningrates, workinghours);

            }


            //for (int i = 0; i < 3; i++)//重新配三条产线
            //{

            //    Line templine = new Line(i);
            //    templine.Machines = new double[7];
            //    for (int j = 0; j < 7; j++)
            //    {

            //        templine.Machines[j] = Factory.Lines[i].Machines[j];
            //    }
            //    newGene.Add(templine);
            //    for (int k = 0; k < oldGene[i].Batches.Count; k++) {//move old batch to new line
            //        templine.Batches = new List<Batch>();
            //        //templine.Batches.Add(oldGene[i].Batches[k]);
            //        BatchArrangeLine(oldGene[i].Batches[k], templine);

            //    }
            //}

            if (randprob < mutationProb)
            {//开始变异
                int counter = 0;
                while (counter < mutatNum)
                {
                    counter++;
                    LineInit();
                    //Console.WriteLine("Counter: "+counter);
                    int line1 = rd.Next(0, Factory.Lines.Count);
                    while (newGene[line1].Batches.Count == 0)
                    {
                        line1 = rd.Next(0, Factory.Lines.Count);
                    }
                    int line2 = rd.Next(0, Factory.Lines.Count);
                    while (newGene[line2].Batches.Count == 0)
                    {
                        line2 = rd.Next(0, Factory.Lines.Count);
                    }

                    int task1 = rd.Next(0, newGene[line1].Batches.Count);
                    int task2 = rd.Next(0, newGene[line2].Batches.Count);

                    Batch batch1 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"));
                    Batch batch2 = new Batch("Batch" + DateTime.Now.ToString("yyyyMMdd"));
                    batch1 = newGene[line1].Batches[task1];
                    batch2 = newGene[line2].Batches[task2];
                    //Batch batch1 = oldGene[line1].Batches[task1];
                    //Batch batch2 = oldGene[line2].Batches[task2];
                    newGene[line1].Batches[task1] = batch2;
                    newGene[line2].Batches[task2] = batch1;
                    for (int i = 0; i < Factory.Lines.Count; i++)
                    {
                        List<Batch> tempbatches = new List<Batch>();//存这条line的batch
                        for (int j = 0; j < newGene[i].Batches.Count; j++)
                        {
                            tempbatches.Add(Clone<Batch>(newGene[i].Batches[j]));//0903                        }
                            newGene[i].Batches.Clear();//清空这个line的batch
                            for (int k = 0; k < tempbatches.Count; k++)
                            {
                                BatchArrangeLine(tempbatches[k], newGene[i]); // 重新加回去
                            }
                            LineArrangeProcedure(newGene[i], yieldrates, runningrates, workinghours);//重新arrange procedure
                        }

                        Boolean ifFeasible = CheckLinesFeasibility(newGene, orderddltable);
                        if (ifFeasible == true)
                        {
                            double newScore = CalculateScore(newGene, orderddltable, scoreParam);
                            double originalScore = CalculateScore(oldGene, orderddltable, scoreParam);
                            if (newScore <= originalScore)
                            {
                                //newGene = oldGene;
                                newGene = new List<Line>();
                                foreach (Line line in oldGene)
                                {
                                    Line templine = Clone<Line>(line);
                                    newGene.Add(templine);
                                    //for (int j = 0; j < templine.Batches.Count; j++)
                                    //{
                                    //    BatchArrangeLine(templine.Batches[j], templine); ;
                                    //}
                                    // LineArrangeProcedure(templine, yieldrates, runningrates, workinghours);

                                }
                            }
                            else
                            {
                                //return newGene;
                                //0903 print mutation result
                                //foreach (Line line in newGene)
                                //{
                                //    foreach (Batch batch in line.Batches)
                                //    {
                                //        Console.WriteLine(batch.ToString());
                                //        Console.WriteLine("Change time:" + batch.Changetime);
                                //        //Console.WriteLine("1st procedure start time:" + batch.Procedures[0].Starttime);
                                //    }
                                //}
                                //Console.WriteLine("返回new Gene");
                                return newGene;
                            }
                        }
                        else
                        {
                            //newGene = oldGene;
                            newGene = new List<Line>();
                            foreach (Line line in oldGene)
                            {
                                Line templine = Clone<Line>(line);
                                newGene.Add(templine);
                                //for (int j = 0; j < templine.Batches.Count; j++)
                                //{
                                //    BatchArrangeLine(templine.Batches[j], templine); ;
                                //}
                                //LineArrangeProcedure(templine, yieldrates, runningrates, workinghours);

                            }
                        }

                    }
                    //LineInit();
                }

                return oldGene;

            }
            else
            {//0903 don't mutate
                return oldGene;
            }
        }
        //生产线上截至第m天累计产量计算
        public double[,] LineProduceTable(Line line, int m)
        {
            HashSet<int> typeset = new HashSet<int>();
            for (int i = 0; i < line.Batches.Count; i++)
            {
                typeset.Add(line.Batches[i].Type);
            }
            int[] types = typeset.ToArray();//获取生产线中产品的类型编号集合
            int n = types.Length;
            double[,] production = new double[n + 1, m + 1];
            for (int i = 0; i < n + 1; i++)
            {
                for (int j = 0; j < m + 1; j++)
                {
                    if (i == 0 && j != 0)//第一行为天数
                    {
                        production[i, j] = j;
                    }
                    else if (i != 0 && j == 0)//第一列为种类编号
                    {
                        production[i, j] = types[i - 1];
                    }
                    else
                    {
                        production[i, j] = 0;
                    }
                }
            }
            //数组除第一列，均初始化为0，循环读取batch，根据batch的类别，决定在哪一行写入，写入时，判断
            //天数j和batch细纱生产的startday和endday之间的关系，如果j<startday，+=0，如果startday<=j<=stopday，
            //+=dayoutput，如果j>stopday，+=0；得到每天了生产多少
            double qianfangtime = 0;//细纱机开始工作前总的时间
            double formerproceduretime = 0;//细纱机工作总的时间
            double sumchangetime = 0;
            //startday是总前纺时间除去60再除去前纺每日工作时间+(没有本批次的后纺总时间+切换至本批次需用时间)除去60再除去后纺每日工作时间
            //dayspan是本批次总的后纺时间/60/后纺每日工作时间
            //stopday=startday+dayspan-1
            int batchIndex = 0;
            foreach (Batch batch in line.Batches)
            {
                //if (batchIndex == 0)
                //{
                //    qianfangtime += (batch.Procedures[6].Starttime - batch.Procedures[0].Starttime);
                //}
                //sumchangetime += batch.Changetime[6];

                //double startday = qianfangtime / 60 / batch.Procedures[0].Workinghours + (formerproceduretime + sumchangetime) / 60 / batch.Procedures[6].Workinghours;
                //formerproceduretime += batch.Procedures[6].Sumproceduretime;
                double startday = batch.Procedures[6].Starttime / 60 / batch.Procedures[0].Workinghours;
                double dayspan = batch.Procedures[6].Sumproceduretime / 60 / batch.Procedures[6].Workinghours;
                double stopday = startday + dayspan;

                Console.WriteLine("startday:" + startday + "; dayspan:" + dayspan + "; stopday:" + stopday);

                for (int i = 1; i < n + 1; i++)
                {
                    if (production[i, 0] == batch.Type)
                    {
                        for (int j = 1; j < m + 1; j++)
                        {
                            //开始日的具体时刻至24点，24h，结束日0点至具体时刻
                            //生产过程总时间，总产量
                            if (startday <= j && j <= Math.Ceiling(stopday))
                            {
                                //根据当日生产时间在总是生产过程中的比例求出当日产量
                                double dayoutput, tempstartday;
                                if (j == Math.Ceiling(startday))
                                {
                                    tempstartday = startday;
                                }
                                else
                                {
                                    tempstartday = j - 1;
                                }
                                if (j == Math.Ceiling(stopday))
                                {
                                    dayoutput = (stopday - tempstartday) / dayspan * batch.Ordervirtualweight;
                                }
                                else
                                {
                                    dayoutput = (j - tempstartday) / dayspan * batch.Ordervirtualweight;
                                }
                                production[i, j] += Math.Round(dayoutput, 2);
                            }
                            else if (j > stopday)
                            {
                                break;
                            }
                        }
                    }
                }
                batchIndex++;
            }
            //在得到每天生产多少后，累加，得到截至这一天累计生产多少
            for (int i = 1; i < n + 1; i++)
            {
                for (int j = 2; j < m + 1; j++)
                {
                    production[i, j] += production[i, j - 1];
                }
            }
            return production;
        }

        public double[,] OrderDeadlineTable(DateTime startdate, List<Order> orders)
        {
            int n, m;
            DateTime deadline;//订单列表中各纺纱品要求最晚的截止日期
            deadline = orders[0].Textiles[0].Deadline;

            HashSet<int> typeset = new HashSet<int>();
            foreach (Order order in orders)
            {
                foreach (Textile textile in order.Textiles)
                {
                    typeset.Add(textile.Type);
                    if (DateTime.Compare(textile.Deadline, deadline) > 0)
                    {
                        deadline = textile.Deadline;
                    }
                }
            }
            TimeSpan ts = deadline.Subtract(startdate);
            m = Convert.ToInt32(Math.Ceiling(ts.TotalDays));//间隔天数
            int[] types = typeset.ToArray();
            n = types.Length;//纱种类

            double[,] ordertable = new double[n + 1, m + 1];
            for (int i = 0; i < n + 1; i++)//全部初始化为0
            {
                for (int j = 0; j < m + 1; j++)
                {
                    if (i == 0 && j != 0)//第一行为天数
                    {
                        ordertable[i, j] = j;
                    }
                    else if (i != 0 && j == 0)//第一列为种类编号
                    {
                        ordertable[i, j] = types[i - 1];
                    }
                    else
                    {
                        ordertable[i, j] = 0;
                    }
                }
            }
            foreach (Order order in orders)
            {
                foreach (Textile textile in order.Textiles)
                {
                    TimeSpan timeSpan = textile.Deadline.Subtract(startdate);
                    int spandays = Convert.ToInt32(Math.Ceiling(timeSpan.TotalDays));//产品截止日期与要求日期之间间隔天数
                    for (int i = 1; i < n + 1; i++)
                    {
                        if (ordertable[i, 0] == textile.Type)
                        {
                            for (int j = 1; j < m + 1; j++)
                            {
                                if (j == spandays)
                                {
                                    ordertable[i, j] += textile.Upperlimit;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return ordertable;
        }

        //Path是读取路径，在tables文件夹内，title为0表示没有表头
        public DataSet ExcelToDS(string Path, int title)
        {
            string strConn;
            if (title == 0)
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=\"Excel 8.0;HDR=NO\"";
            }
            else
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            }
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            return ds;
        }
    }
}