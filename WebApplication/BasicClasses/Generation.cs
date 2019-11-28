using System;

namespace TextileDemo.BasicClasses
{
    class Generation
    {
        private List<Gene> Genes;//种群全体成员
        private double[] Scores;//每个个体的分数
        private double BestScore;//本代中的最高分
        private Gene BestGene; //本代中的最优解

        public Generation()
        {
            this.Genes = new List<Gene>();
            this.Scores = new double[];
            this.BestScore = -1;
            this.BestGene = new Gene();
        }

        internal List<Gene> Genes { get => lines; set => lines = value; }
        internal double Score { get => score; set => score = value; }
    }
}
