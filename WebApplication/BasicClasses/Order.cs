using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    [Serializable]
    class Order
    {

        private string id;
        private string partyAname;//甲方名称
        private List<Textile> textiles;//订单中包含的纺纱类型
        private int partyAlevel;//甲方等级=？
        private DateTime predicttime;//订单预期完成时间=预计订单中最后生产的那种纱的生产完成时间
        private DateTime realfinishtime;//订单实际完成时间=实际订单中最后生产的那种纱的生产完成时间
        private double profit;//该订单预期利润=？

        public Order(string id, string partyAname)
        {
            this.id = id;
            this.partyAname = partyAname;
            this.Textiles = new List<Textile>();
        }

        public string Id { get => id; set => id = value; }
        public string PartyAname { get => partyAname; set => partyAname = value; }
        public int PartyAlevel { get => partyAlevel; set => partyAlevel = value; }
        public DateTime Predicttime { get => predicttime; set => predicttime = value; }
        public DateTime Realfinishtime { get => realfinishtime; set => realfinishtime = value; }
        public double Profit { get => profit; set => profit = value; }
        public List<Textile> Textiles { get => textiles; set => textiles = value; }
        public void addTextile(Textile newtextile) { textiles.Add(newtextile); }
        public void removeTextile(Textile textile) { textiles.Remove(textile); }

        public override string ToString()
        {
            string info = "订单ID：" + Id + "\n" +
                "甲方名称：" + PartyAname + "\n" +
                "订单内容：\n";
            for (int i = 0; i < Textiles.Count; i++)
            {
                info += Textiles[i].Name + "：" + Textiles[i].Weight + " kg；交付时间："+Textiles[i].Deadline+"\n";
            }
            return info;
        }
    }
}
