using System;

namespace TextileDemo.BasicClasses
{
    class Gene
    {
        Factory factory;
        internal Factory Factory { get => factory; set => factory = value; }
        private List<Line> lines;
        private double score;

        public Gene()
        {
            this.lines = new List<Line>();

            this.score = -1;
        }

        internal List<Line> Lines{ get => lines; set => lines = value; }
        internal double Score { get => score; set => score = value; }

    }
}
