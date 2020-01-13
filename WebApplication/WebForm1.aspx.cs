using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.BasicClasses;
using WebApplication.GA;
using WebApplication.GA.GA;
using WebApplication.NewMethod;

namespace WebApplication
{
    public partial class WebForm1 : Page
    {
        public static Dictionary<string, List<TableRow>> dic = new Dictionary<string, List<TableRow>>();
        protected void Page_Load(object sender, EventArgs e)
        {
            Table1.Visible = false;
            Table2.Visible = false;
            Table3.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "1工厂机器数量配置表模板";
            getTable(name);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "2生产线配置表模板";
            getTable(name);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "3各工序制成率_设备运转率表模板";
            getTable(name);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "4机器生产参数配置表模板";
            getTable(name);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "5产品干湿重g_m模板";
            getTable(name);
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "6各工序切换产品所需时间表模板";
            getTable(name);
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "7各机器预计工作时长模板";
            getTable(name);
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            dic = new Dictionary<string, List<TableRow>>();
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = "7订单表模板";
            getTable(name);
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            Table1.Visible = false;
            Table3.Visible = false;
            Table2.Visible = true;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
            string name = Button8.Text;
            Table2.Caption = name;
        }

        /// <summary>
        /// 开始排产
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button9_Click(object sender, EventArgs e)
        {
            Table1.Visible = false;
            Table3.Visible = false;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = true;
            resultShow.Visible = false;
            Button14.Visible = false;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button10_Click(object sender, EventArgs e)
        {
            if (this.M.Text == "" || this.N.Text == "" || this.Pm.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "onekey", "alert('参数未填写完整！')", true);
                Table2.Visible = true;
                return;
            }

            var M = int.Parse(this.M.Text);
            var N = int.Parse(this.N.Text);
            var Pm = int.Parse(this.Pm.Text);
            if (Pm == 0 || Pm > 100)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "onekey",
                    "alert('变异概率的值不合法！输入值必须在0~100之间！')", true);
                return;
            }

            GeneticAlgorithm.M = M;
            GeneticAlgorithm.N = N;
            GeneticAlgorithm.Parent_N = N / 2;
            GeneticAlgorithm.Pm = Pm;

            Table2.Visible = true;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button12_Click(object sender, EventArgs e)
        {
            string filename = FileUpload1.PostedFile.FileName;

            string name = Table1.Caption +"模板";

            if (filename.Length <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "onekey",
                        "alert('请选择文件')", true);

                Table1.Visible = true;
                Table3.Visible = true;
                Table2.Visible = false;
                Table4.Visible = false;
                Table5.Visible = false;
                resultShow.Visible = false;
                getTable(name);
                return;
            }
            int index = filename.IndexOf("(");
            if (index != -1)
            {
                filename = filename.Substring(0, index - 3);
            }
            else
            {
                filename = filename.Substring(0, filename.Length - 6);
            }
            filename += "模板.xls";
            if (FileUpload1.PostedFile.ContentLength > 0)
            {
                if (File.Exists(MainDeal.path + filename))
                {
                    File.Delete(MainDeal.path + filename);
                }
                FileUpload1.PostedFile.SaveAs(MainDeal.path + filename);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "onekey",
                        "alert('文件上传失败！请选择文件')", true);
            }

            getTable(filename);

            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button13_Click(object sender, EventArgs e)
        {
            string name = MainDeal.path + Table1.Caption + "示例.xls";
            //System.Diagnostics.Process.Start(name);
            DownloadFile(name);
            Table1.Rows.AddRange(dic["Table1"].ToArray());
            if (dic.ContainsKey("Table4"))
            {
                Table4.Rows.AddRange(dic["Table4"].ToArray());
                Table4.Visible = true;
            }
            Table1.Visible = true;
            Table3.Visible = true;
            Table2.Visible = false;
            Table5.Visible = false;
            resultShow.Visible = false;
        }

        /// <summary>
        /// 下载排产结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button14_Click(object sender, EventArgs e)
        {
            string name = MainDeal.path + "result.txt";
            DownloadFile(name);
            Table5.Visible = true;
            resultShow.Visible = true;
        }

        /// <summary>
        /// 点击开始排产
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button15_Click(object sender, EventArgs e)
        {
            Table1.Visible = false;
            Table3.Visible = false;
            Table2.Visible = false;
            Table4.Visible = false;
            Table5.Visible = true;
            resultShow.Visible = false;

            //string outMessage = GAMainDeal.GA();
            string outMessage = NewTest.New();
            ScriptManager.RegisterStartupScript(Page, GetType(), "onekey",
                    "alert('排产结束！')", true);
            resultShow.Text = outMessage;

            FileStream fs = new FileStream(MainDeal.path + "result.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(outMessage.Replace("<br/>", "\r\n"));
            sw.Flush();
            sw.Close();
            fs.Close();

            Button14.Visible = true;
            resultShow.Visible = true;
        }

        /// <summary>
        /// 获取并显示表单信息
        /// </summary>
        /// <returns></returns>
        public void getTable(string name)
        {
            var fileLocation = MainDeal.path + name + ".xls";

            if (name.Contains(".xls")|| name.Contains(".xlsx"))
            {
                fileLocation = MainDeal.path + name;
            }
            if (!File.Exists(fileLocation))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "onekey",
                       "alert('无配置文件，请上传配置文件')", true);
                return;
            }

            DataSet data = Util.ExcelToDS(fileLocation, 0);
            List<TableRow> rowList = new List<TableRow>();

            for (int k = 0; k < data.Tables.Count; k++)
            {
                int row = data.Tables[k].Rows.Count;
                for (int i = 0; i < row; i++)
                {
                    TableRow r = new TableRow();
                    List<TableCell> cellList = new List<TableCell>();
                    int col = data.Tables[k].Rows[i].ItemArray.Length;
                    for (int j = 0; j < col; j++)
                    {
                        TableCell c = new TableCell();
                        c.Text = "" + data.Tables[k].Rows[i].ItemArray[j];
                        c.Width = Unit.Percentage(100 / col);
                        c.Height = Unit.Pixel(30);
                        if (i % 2 == 0)
                        {
                            c.BackColor = System.Drawing.Color.LightGray;
                        }
                        cellList.Add(c);
                    }
                    r.Cells.AddRange(cellList.ToArray());
                    rowList.Add(r);
                }
                Table1.Rows.AddRange(rowList.ToArray());
            }
            Table1.Caption = name.Substring(0, name.Length - 2);
            if (dic.ContainsKey("Table1"))
                dic["Table1"] = rowList;
            else
                dic.Add("Table1", rowList);
        }

        /// <summary>
        /// 获取并显示表单信息
        /// </summary>
        /// <returns></returns>
        public void getTableFromPath(string path)
        {
            DataSet data = Util.ExcelToDS(path, 0);
            List<TableRow> rowList = new List<TableRow>();

            for (int k = 0; k < data.Tables.Count; k++)
            {
                int row = data.Tables[k].Rows.Count;
                for (int i = 0; i < row; i++)
                {
                    TableRow r = new TableRow();
                    List<TableCell> cellList = new List<TableCell>();
                    int col = data.Tables[k].Rows[i].ItemArray.Length;
                    for (int j = 0; j < col; j++)
                    {
                        TableCell c = new TableCell();
                        c.Text = "" + data.Tables[k].Rows[i].ItemArray[j];
                        cellList.Add(c);
                    }
                    r.Cells.AddRange(cellList.ToArray());
                    rowList.Add(r);
                }
                Table4.Rows.AddRange(rowList.ToArray());
            }
            Table4.Caption = path;
            if (dic.ContainsKey("Table4"))
                dic["Table4"] = rowList;
            else
                dic.Add("Table4", rowList);
        }

        protected void DownloadFile(string url)
        {
            // 定义文件路径
            // 定义文件名
            string fileName = "";
            // 取得地址中的文件名

            #region 取得地址中的文件名
            // 判断获取的是否为地址，而非文件名
            if (url.IndexOf("\\") > -1)
            {
                // 获取文件名
                fileName = url.Substring(url.LastIndexOf("\\") + 1);//获取文件名

            }
            else
            {
                // url为文件名时，直接获取文件名
                fileName = url;
            }
            #endregion

            // 流方式下载文件 
            #region 流方式下载文件[]
            try
            { // 以字符流的方式下载文件
                FileStream fileStream = new FileStream(url, FileMode.Open);
                byte[] bytes = new byte[(int)fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                Response.ContentType = "application/octet-stream";
                // 通知浏览器下载而不是打开
                //Response.AddHeader("Content-Disposition",
                //    "attachment; filename=" +
                //    HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                Response.AddHeader("Content-Disposition",
                   "attachment; filename=" + fileName);
                //Response.Charset = "UTF-8";
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {

            }

            #endregion

        }
    }
}