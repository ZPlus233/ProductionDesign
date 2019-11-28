using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI.WebControls;
using WebApplication.BasicClasses;

namespace WebApplication
{
    public class MainDeal
    {
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory + "UpLoad\\";
        static FactoryOperate factoryOperator = new FactoryOperate();
        public static int numOfGeneration = 5;//迭代次数
        public static int populationSize = 10;//种群规模
        public static double mutationProb = 0.7;//单个个体的变异概率
        public static int mutationTrialNum = 5;//单个个体变异时的尝试次数
        public static double scoreParam = 1;//总分数=总提前时间-参数*总切换时间
        public static int allowedTrialNum = 2000;//在产生初始种群阶段，若尝试allowedTrialNum次数后仍然未产生新解，结束并返回当前种群

        /// <summary>
        /// 主要处理步骤（WebApplication main里面代码）
        /// </summary>
        public static void mainDealStep()
        {
            FactoryOperate factoryoperator = new FactoryOperate();

            List<Order> orders = factoryoperator.OrderTableReader();//读入订单，订单存在/bin/Debug/Tables中，表的名字在orderTableReader中手动改

            foreach (Order order in orders)
            {
                Console.WriteLine(order.ToString());//打印当前订单配置信息
            }
            Console.WriteLine("订单配置读取完成");

            factoryoperator.Init();


            double[] yieldrates = new double[7];
            double[] runningrates = new double[7];
            double[] workinghours = new double[7];
            factoryoperator.ProcedureRateInit(yieldrates, runningrates, workinghours);//读取其他配置表


            //使用过程：1.建立订单2.根据订单内容建立批次3.为批次分配生产线4.计算各生产线上的批次时间5.计算截止时间生产每天生产数量


            double[,] orderddltable = factoryoperator.OrderDeadlineTable(DateTime.Now, orders.ToList());//生成订单截止日期表
            List<Batch> initialBatchs = factoryoperator.CreateInitialBatch(orders.ToArray(), false);//将订单内单品放入初始单品列表（initialbatches），不做可行性检查。
            Console.WriteLine("开始检查订单可行性");
            Boolean ifFeasible = factoryoperator.CheckOrderFeasibility(initialBatchs, orderddltable, yieldrates, runningrates, workinghours);//检查三条线满产能做单品能否完成
            Console.WriteLine("从订单提取并拆分单品，开始, 只打印需被拆分单品");
            List<Batch> batches = factoryoperator.CreateInitialBatch(orders.ToArray(), true);
            Console.WriteLine("从订单提取并拆分单品，结束");
            Console.WriteLine("全部待分配单品列表： ");
            foreach (Batch batch in batches)
            {
                Console.WriteLine(batch.ToString());//打印全部待分配单品
            }

            /* 遗传算法
            参数：NumOfGeneration：一共产生多少代
                 PopulationSize: 每代多少个体
                 IterParam：每次变异迭代多少次
                 mutationProb: 每个基因有多少概率突变*/

            List<double> eliteScoreList = new List<double>();
            List<List<Line>> eliteSolnList = new List<List<Line>>();

            int mutCounter = 1;//当前所在代数
            List<double> generationScore = new List<double>();
            List<List<Line>> firstGeneration = factoryoperator.ProducePossibileSoln(batches, populationSize, allowedTrialNum, orderddltable, yieldrates, runningrates, workinghours);//产生初始种群


            if (firstGeneration.Count == 0)
            {
                Console.WriteLine("经过" + allowedTrialNum + " 次尝试, 未能找到可行解");
                Console.ReadLine();


            }
            foreach (List<Line> gene in firstGeneration)
            {
                double geneScore = factoryoperator.CalculateScore(gene, orderddltable, scoreParam);
                generationScore.Add(geneScore);
            }

            int eliteIndex = factoryoperator.EliteScoreIndex(generationScore);
            eliteSolnList.Add(firstGeneration[eliteIndex]);
            eliteScoreList.Add(generationScore[eliteIndex]);
            //Console.WriteLine(generationScore[eliteIndex]);

            List<List<Line>> oldGeneration = firstGeneration;
            Console.WriteLine("遗传算法开始，从第1代开始 \n");
            while (mutCounter < numOfGeneration)
            {
                mutCounter++;
                Console.WriteLine("\n当前代数:" + mutCounter + "\n");
                List<List<Line>> newGeneration = new List<List<Line>>();
                foreach (List<Line> gene in oldGeneration)
                {//mutation产生下一代

                    List<Line> newGene = factoryoperator.Mutation(gene, mutationProb, mutationTrialNum, orderddltable, scoreParam, yieldrates, runningrates, workinghours);
                    newGeneration.Add(newGene);
                }
                //0903 深拷贝
                oldGeneration = new List<List<Line>>();
                foreach (List<Line> oneGene in newGeneration)
                {
                    oldGeneration.Add(Clone<List<Line>>(oneGene));
                }

                //oldGeneration = newGeneration;// upadate old generation

                generationScore = new List<double>();

                // Console.WriteLine(mutCounter + "generation:");
                foreach (List<Line> gene in newGeneration)
                {
                    double geneScore = factoryoperator.CalculateScore(gene, orderddltable, scoreParam);
                    generationScore.Add(geneScore);

                    //Console.WriteLine(geneScore.ToString());
                }

                eliteIndex = factoryoperator.EliteScoreIndex(generationScore);
                eliteSolnList.Add(newGeneration[eliteIndex]);
                eliteScoreList.Add(generationScore[eliteIndex]);


            }
            Console.WriteLine("\n遗传算法结束\n");
            // 遗传结束
            int bestSolutionIndex = -1;
            double bestScore = -1;
            for (int i = 0; i < eliteScoreList.Count; i++)
            {
                if (eliteScoreList[i] > bestScore)
                {
                    bestScore = eliteScoreList[i];
                    bestSolutionIndex = i;
                }
            }

            if (File.Exists(MainDeal.path + "/sample.xlsx"))
            {
                //如果存在则删除
                File.Delete(MainDeal.path + "/sample.xlsx");
            }
            Util.Create(MainDeal.path + "/sample.xlsx", eliteSolnList[bestSolutionIndex]);
            
            //将遗传算法结果打印到控制台并保存
            FileStream fs = new FileStream(path + "GeneticLog.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write("迭代次数: " + numOfGeneration + "\n种群规模:" + populationSize + "\n单个个体变异概率: " + mutationProb + "\n变异尝试次数: " + mutationTrialNum + "\n产生初始解尝试次数: " + allowedTrialNum + "\n");
            Console.WriteLine("最终最优解:\n");
            sw.Write("最终最优解:\n");
            foreach (Line line in eliteSolnList[bestSolutionIndex])
            {
                Console.WriteLine(line.ToString());
                sw.Write(line.ToString() + "\n");
                foreach (Batch batch in line.Batches)
                {
                    Console.WriteLine(batch.ToString());
                    sw.Write(batch.ToString() + "\n");
                }
            }
            Console.WriteLine("最优解分数: " + bestScore);
            sw.WriteLine("最优解分数: " + bestScore);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        //Clone方法用于List的深复制
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
    }
}