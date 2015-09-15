using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKC.JBus.Helpers
{
    public static class ExcelHelper
    {
        public static void DumpXlsx(HttpResponseBase response, DataTable table, string fileName, string title = "")
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                int headerRowNo = 1;
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                if (title.IsNullOrEmpty())
                {
                    headerRowNo = 2;
                }
                else
                {
                    headerRowNo = 3;
                }

                //var t = sw.Elapsed;
                //System.Diagnostics.Debug.WriteLine(string.Format("1. {0}M {1}S, {2}MS", t.Minutes, t.Seconds, t.Milliseconds));
                //sw.Restart();

                ws.Cells[string.Format("A{0}", headerRowNo)].LoadFromDataTable(table, true);

                //t = sw.Elapsed;
                //System.Diagnostics.Debug.WriteLine(string.Format("2. {0}M {1}S, {2}MS", t.Minutes, t.Seconds, t.Milliseconds));
                //sw.Restart();

                int colCount = table.Columns.Count;
                string lastColName = GetExcelColumnName(colCount);
                ExcelRange cols = ws.Cells[string.Format("A:{0}", lastColName)];
                cols.Style.Font.Name = "Tahoma";
                cols.Style.Font.Size = 10;

                //Format the header for column 1-3
                using (ExcelRange rng = ws.Cells[string.Format("A{0}:{1}{0}", headerRowNo, lastColName)]) // "A1:{0}1"
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                //t = sw.Elapsed;
                //System.Diagnostics.Debug.WriteLine(string.Format("3. {0}M {1}S, {2}MS", t.Minutes, t.Seconds, t.Milliseconds));
                //sw.Restart();

                ws.Cells[headerRowNo, 1, headerRowNo + 10, colCount].AutoFitColumns();

                //t = sw.Elapsed;
                //System.Diagnostics.Debug.WriteLine(string.Format("4. {0}M {1}S, {2}MS", t.Minutes, t.Seconds, t.Milliseconds));
                //sw.Restart();

                string createDate = string.Format("{0}: {1}", "สร้างเมื่อ", DateTime.Now.ToString("d MMMM yyyy เวลา H:mm น.", AppUtils.ThaiCulture));
                if (!title.IsNullOrEmpty())
                {
                    ws.Cells[1, 1].Value = title;
                    // ws.SetValue(1, 1, title);

                    using (ExcelRange rng = ws.Cells["A1:A1"])
                    {
                        rng.Style.Font.Bold = true;
                    }
                    ws.Cells[2, 1].Value = createDate;
                }
                else
                {
                    ws.Cells[1, 1].Value = createDate;
                }
                //Example how to Format Column 1 as numeric
                //using (ExcelRange col = ws.Cells[2, 1, 2 + tbl.Rows.Count, 1])
                //{
                //    col.Style.Numberformat.Format = "#,##0.00";
                //    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //}

                //t = sw.Elapsed;
                //System.Diagnostics.Debug.WriteLine(string.Format("5. {0}M {1}S, {2}MS", t.Minutes, t.Seconds, t.Milliseconds));
                //sw.Restart();

                //Write it back to the client
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", AppUtils.CleanFileName(fileName)));
                response.BinaryWrite(pck.GetAsByteArray());

                //t = sw.Elapsed;
                //System.Diagnostics.Debug.WriteLine(string.Format("6. {0}M {1}S, {2}MS", t.Minutes, t.Seconds, t.Milliseconds));
                //sw.Restart();
            }
        }

        public static void DumpXls(HttpResponseBase response, DataTable table, string fileName)
        {
            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();

            response.ClearContent();
            response.Buffer = true;
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", fileName));
            response.ContentType = "application/ms-excel";

            response.Charset = "";
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            response.Output.Write(sw.ToString());
            response.Flush();
            response.End();
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = "";
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}