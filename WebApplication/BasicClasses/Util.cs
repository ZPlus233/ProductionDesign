using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.BasicClasses
{
    using Excel = Microsoft.Office.Interop.Excel;
    class Util
    {
        //Path是读取路径，在tables文件夹内，title为0表示没有表头
        public static DataSet ExcelToDS(string Path, int title)
        {
            FileStream stream = File.Open(Path, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader;

            if (!Path.Contains("xlsx"))
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else
            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx) 
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);


            DataSet result;
            if (title == 0)
            {
                result = excelReader.AsDataSet();
            }
            else
            {
                result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
            }
            excelReader.Close();
            //string strConn;
            //if (title == 0)
            //{
            //    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\"";
            //}
            //else
            //{
            //    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=\"Excel 8.0;IMEX=1\"";
            //}
            //OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //string strExcel = "";
            //OleDbDataAdapter myCommand = null;
            //DataSet ds = null;
            //strExcel = "select * from [sheet1$]";
            //myCommand = new OleDbDataAdapter(strExcel, strConn);
            //ds = new DataSet();
            //myCommand.Fill(ds, "table1");
            return result;
        }

        //Path是读取路径，在tables文件夹内，title为0表示没有表头,sheet代表第几页
        public static DataSet ExcelToDS(string Path, int title, string sheetname)
        {
            FileStream stream = File.Open(Path, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader;

            //Choose one of either 1 or 2
            if (!Path.Contains("xlsx"))
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx) 
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result;
            if (title == 0)
            {
                result = excelReader.AsDataSet();
            }
            else
            {
                result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
            }
            excelReader.Close();
            return result;
            //string strConn;
            //if (title == 0)
            //{
            //    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=\"Excel 8.0;HDR=NO\"";
            //}
            //else
            //{
            //    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            //}
            //OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //string strExcel = "";
            //OleDbDataAdapter myCommand = null;
            //DataSet ds = null;
            //strExcel = "select * from [" + sheetname + "$]";
            //myCommand = new OleDbDataAdapter(strExcel, strConn);
            //ds = new DataSet();
            //myCommand.Fill(ds, "table1");
            //return ds;
        }

        //将DataSet写入excel
        public static void ExportExcel(DataSet dataSet)
        {
            Excel.Application excelApp = new Excel.Application();
            if (excelApp == null)
            {
                return;
            }
            excelApp.Visible = false;
            Excel.Workbooks workbooks = excelApp.Workbooks;
            Excel.Workbook workbook = workbooks.Add(true);
            for (int index = 0; index < dataSet.Tables.Count; index++)
            {
                DataTable dataTable = dataSet.Tables[index];
                Excel.Worksheet worksheet = workbook.Worksheets["Sheet1"];

                //创建一个单元格
                Excel.Range range;
                int rowIndex = 1;
                int colIndex = 1;
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    //设置第一行，即列名
                    worksheet.Cells[rowIndex, colIndex + i] = dataTable.Columns[i].ColumnName;
                    //获取第一行的每个单元格
                    range = worksheet.Cells[rowIndex, colIndex + i];
                    //字体加粗
                    range.Font.Bold = true;
                    //设置为黑色
                    range.Font.Color = 0;
                    //设置为宋体
                    range.Font.Name = "Arial";
                    //设置字体大小
                    range.Font.Size = 12;
                    //水平居中
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    //垂直居中
                    range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                }
                rowIndex++;
                //写入数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[rowIndex + i, colIndex + j] = dataTable.Rows[i][j].ToString();

                        range = worksheet.Cells[rowIndex + i, colIndex + j];
                        range.Interior.Color = System.Drawing.Color.Yellow;
                        range.Cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        range.Borders.Weight = Excel.XlBorderWeight.xlHairline;//边框常规粗细 
                    }
                }
                //设置所有单元格列宽为自动列宽
                worksheet.Cells.Columns.AutoFit();
            }
            //保存写入的数据，这里还没有保存到磁盘
            workbook.Saved = true;
            try
            {
                System.IO.File.Delete(System.IO.Directory.GetCurrentDirectory() + "/sample.xlsx");
            }
            catch (Exception e)
            {

            }
            workbook.SaveAs(System.IO.Directory.GetCurrentDirectory() + "/sample.xlsx");
            workbook.Close();
            excelApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            workbook = null;
            excelApp = null;
            GC.Collect();
        }


        public static void Create(string filePath, List<Line> lines)
        {
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            WorksheetPart worksheetpart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetpart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = workbookpart.Workbook.AppendChild<Sheets>(new Sheets());
            Sheet sheet = new Sheet();
            sheet.Id = workbookpart.GetIdOfPart(worksheetpart);
            sheet.SheetId = 1;
            sheet.Name = "sheet";
            sheets.Append(sheet);

            SheetData sheetdata = worksheetpart.Worksheet.GetFirstChild<SheetData>();

            string columnName = "";
            Cell cell = null;

            DateTime nowdate = DateTime.Now;
            //求出每个batch开始日及经过天数！！！！！！！
            foreach (var line in lines)
            {
                for (int i = 0; i < line.Batches.Count; i++)
                {
                    line.Batches[i].Startday = line.Batches[i].Procedures[0].Starttime / 60 / line.Batches[i].Procedures[0].Workinghours;
                    line.Batches[i].Dayspan = (line.Batches[i].Procedures[6].Starttime - line.Batches[i].Procedures[0].Starttime) / 60 / line.Batches[i].Procedures[0].Workinghours
                            + line.Batches[i].Procedures[6].Sumproceduretime / 60 / line.Batches[i].Procedures[6].Workinghours;
                }
            }
            //找到最晚的完成日期
            double lastday = 0;
            foreach (var line in lines)
            {
                if (line.Batches.Count >= 1 && lastday < (line.Batches.Last().Startday + line.Batches.Last().Dayspan))
                {
                    lastday = line.Batches.Last().Startday + line.Batches.Last().Dayspan;
                }
            }
            for (int i = 1; i < lastday + 1; i++)
            {
                DateTime date = nowdate.AddDays(i);
                columnName = ConvertToChar(i + 1);
                cell = InsertCellInWorksheet(columnName, 1, sheetdata);
                cell.CellValue = new CellValue(date.ToShortDateString());
                cell.DataType = new EnumValue<CellValues>(CellValues.String);
            }

            System.Drawing.Color[] colors = {System.Drawing.Color.FromArgb(146, 208, 80), System.Drawing.Color.FromArgb(250,192,5), System.Drawing.Color.FromArgb(144,144,144)
                                ,System.Drawing.Color.FromArgb(255,33,33),System.Drawing.Color.FromArgb(47,139,241),System.Drawing.Color.FromArgb(198,68,220),System.Drawing.Color.FromArgb(90,198,167)};

            Excel.XlPattern[] xlPatterns = { Excel.XlPattern.xlPatternGray8, Excel.XlPattern.xlPatternGray16, Excel.XlPattern.xlPatternLinearGradient, Excel.XlPattern.xlPatternCrissCross
            ,Excel.XlPattern.xlPatternGrid,Excel.XlPattern.xlPatternLightVertical,Excel.XlPattern.xlPatternLightDown};
            //求出有多少个订单
            HashSet<String> orderidset = new HashSet<string>();
            foreach (var line in lines)
            {
                foreach (var batch in line.Batches)
                {
                    foreach (var order in batch.Orders)
                    {
                        orderidset.Add(order.Id);
                    }
                }
            }
            string[] orderidlist = orderidset.ToArray();

            //有多少个批次，就有多少行
            int sumrow = 0;
            foreach (var line in lines)
            {
                sumrow += line.Batches.Count;
            }
            int rownum = 0;
            int rowIndex = 2;
            int colIndex = 1;
            for (int i = 0; i < lines.Count; i++)
            {
                int batchindex = 0;
                for (int k = 0; k < lines[i].Batches.Count; k++, rownum++)
                {
                    int j = 0;
                    while (j <= lastday + 1)
                    {
                        if (j == 0)//第一列为生产线标识
                        {
                            columnName = ConvertToChar(colIndex + j);
                            cell = InsertCellInWorksheet(columnName, (uint)(rowIndex + rownum), sheetdata);
                            cell.CellValue = new CellValue("生产线" + (i + 1));
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                            j++;
                        }
                        else
                        {
                            //如果该生产线上没有批次或者是超出范围
                            if (lines[i].Batches.Count == 0 || batchindex + 1 > lines[i].Batches.Count)
                            {
                                break;
                            }
                            Batch tempbatch = lines[i].Batches[batchindex];
                            string orderid = "";
                            string partyAname = "";
                            foreach (var order in tempbatch.Orders)
                            {
                                orderid += order.Id + " ";
                                partyAname += order.PartyAname + " ";
                            }
                            int textiletype = tempbatch.Type;
                            string textilename = "";
                            foreach (var textile in tempbatch.Orders[0].Textiles)
                            {
                                if (textiletype == textile.Type)
                                {
                                    textilename = textile.Name;
                                    break;
                                }
                            }
                            double weight = tempbatch.Orderweight;
                            //根据batch的startday和dayspan合并单元格
                            if (j == Convert.ToInt32(Math.Floor(tempbatch.Startday + 1)))
                            {
                                //单元格内填充文字
                                string msg = orderid + "/" + partyAname + "-" + textilename + "-" + Math.Round(weight / 1000, 2).ToString() + "t" + "-" + Math.Ceiling(tempbatch.Dayspan) + "天";
                                //合并单元格
                                columnName = ConvertToChar(colIndex + j);
                                cell = InsertCellInWorksheet(columnName, (uint)(rowIndex + rownum), sheetdata);
                                cell.CellValue = new CellValue(msg);
                                cell.DataType = new EnumValue<CellValues>(CellValues.String);

                                MergeCells mergeCells;

                                if (worksheetpart.Worksheet.Elements<MergeCells>().Count() > 0)
                                    mergeCells = worksheetpart.Worksheet.Elements<MergeCells>().First();
                                else
                                {
                                    mergeCells = new MergeCells();

                                    // Insert a MergeCells object into the specified position.  
                                    if (worksheetpart.Worksheet.Elements<CustomSheetView>().Count() > 0)
                                        worksheetpart.Worksheet.InsertAfter(mergeCells, worksheetpart.Worksheet.Elements<CustomSheetView>().First());
                                    else
                                        worksheetpart.Worksheet.InsertAfter(mergeCells, worksheetpart.Worksheet.Elements<SheetData>().First());
                                }

                                // Create the merged cell and append it to the MergeCells collection.  
                                string cellname1 = ConvertToChar(colIndex + j) + (rowIndex + rownum);
                                string cellname2 = ConvertToChar(colIndex + Convert.ToInt32(Math.Floor(tempbatch.Startday + tempbatch.Dayspan + 1))) + (rowIndex + rownum);
                                MergeCell mergeCell = new MergeCell()
                                {
                                    Reference =
                                    new StringValue(cellname1 + ":" + cellname2)
                                };
                                mergeCells.Append(mergeCell);
                                //range = worksheet.Range[worksheet.Cells[rowIndex + rownum, colIndex + j], worksheet.Cells[rowIndex + rownum, colIndex + Convert.ToInt32(Math.Floor(tempbatch.Startday + tempbatch.Dayspan + 1))]];
                                //range.Interior.Color = colors[textiletype % 7];
                                //for (int index = 0; index < orderidlist.Length; index++)
                                //{
                                //    if ((orderidlist[index] + " ").Equals(orderid))
                                //    {
                                //        range.Interior.Pattern = xlPatterns[index % 7];
                                //        break;
                                //    }
                                //}
                                //range.Merge(true);
                                //range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                //range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                batchindex++;
                                break;
                            }
                            j++;
                        }
                    }
                }
            }
            spreadsheetDocument.Close();
        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, SheetData sheetData)
        {
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            var count = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count();
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);
                return newCell;
            }
        }

        private static string ConvertToChar(int value)
        {
            string rtn = string.Empty;
            List<int> iList = new List<int>();

            //To single Int
            while (value / 26 != 0 || value % 26 != 0)
            {
                iList.Add(value % 26);
                value /= 26;
            }

            //Change 0 To 26
            for (int j = 0; j < iList.Count - 1; j++)
            {
                if (iList[j] == 0)
                {
                    iList[j + 1] -= 1;
                    iList[j] = 26;
                }
            }
            //Remove 0 at last
            if (iList[iList.Count - 1] == 0)
            {
                iList.Remove(iList[iList.Count - 1]);
            }

            //To String
            for (int j = iList.Count - 1; j >= 0; j--)
            {
                char c = (char)(iList[j] + 64);
                rtn += c.ToString();
            }

            return rtn;
        }
    }
}