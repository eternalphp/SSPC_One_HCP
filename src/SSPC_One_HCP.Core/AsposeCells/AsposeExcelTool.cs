using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Bot.Tool
{
    /// <summary>
    /// execl操作
    /// </summary>
    public class AsposeExcelTool
    {
        /// <summary>
        ///导入execl 并转化为   DataTable
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="showTitle">是否导出列名称 默认true 导出 </param>
        /// <returns></returns>
        public static DataTable ExportToDataTableAsString(string excelFilePath, bool showTitle = true)
        {
#if NETSTANDARD2_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

            Workbook wb = new Workbook(excelFilePath);
            WorkbookDesigner book = new WorkbookDesigner(wb);
            var sheet = book.Workbook.Worksheets[0];
            var cells = sheet.Cells;
            DataTable dt = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, showTitle);
            return dt;
        }
        /// <summary>
        ///导入execl 并转化为   DataTable 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="showTitle"></param>
        /// <returns></returns>
        public static DataTable ExportToDataTableAsString(Stream stream, bool showTitle = true)
        {
#if NETSTANDARD2_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            Workbook wb = new Workbook(stream);
            WorkbookDesigner book = new WorkbookDesigner(wb);
            Cells cells = book.Workbook.Worksheets[0].Cells;
            DataTable dt = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, showTitle);//showTitle
            return dt;
        }

        /// <summary>
        /// 根据模板导出execl文件
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="variable"></param>
        /// <param name="sheetName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ExportTemplate(string excelFilePath, string variable, string sheetName, object obj)
        {
#if NETSTANDARD2_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            Workbook wb = new Workbook(excelFilePath);
            WorkbookDesigner book = new WorkbookDesigner(wb);
            book.SetDataSource(variable, obj);
            book.Process();
            book.Workbook.Worksheets[0].Name = sheetName;
            MemoryStream stream = new MemoryStream();
            book.Workbook.Save(stream, SaveFormat.Xlsx);
            return stream.ToArray();
        }

        public static byte[] ExportTemplate(string excelFilePath, Dictionary<string, object> keyValues, string[] sheetName)
        {
#if NETSTANDARD2_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            Workbook wb = new Workbook(excelFilePath);
            WorkbookDesigner book = new WorkbookDesigner(wb);
            foreach (var item in keyValues)
                book.SetDataSource(item.Key, item.Value);
            book.Process();
            for (int i = 0; i < sheetName.Length; i++)
                book.Workbook.Worksheets[i].Name = sheetName[i];
            MemoryStream stream = new MemoryStream();
            book.Workbook.Save(stream, SaveFormat.Xlsx);
            return stream.ToArray();
        }
    }
}
