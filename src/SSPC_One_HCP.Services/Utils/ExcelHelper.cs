using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Utils
{
    public class ExcelHelper : IDisposable
    {
        private string fileName = ""; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;
        private string[] _fieldsNot = { "PageIndex", "PageSize", "IsImport", "IsExport" };
        public ExcelHelper(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                this.fileName = fileName;
                if (!File.Exists(fileName))
                    File.Create(fileName).Close();
                disposed = false;
            }
        }
        public bool Export<T>(List<T> _list, List<string> _headers) where T : class
        {
            var _t = typeof(T);
            var _props = _t.GetProperties().Where(c => /*c.GetMethod.IsVirtual == false && */!_fieldsNot.Contains(c.Name) && _headers.Contains(c.Name)).ToArray();
            DataTable _dt = new DataTable();
            var _displayName = "";
            var _dicHeaderNames = new Dictionary<string, string>();
            // 遍历字段名
            //foreach (var item in _props)
            //{
            //    _displayName = item.GetCustomAttribute<DisplayAttribute>()?.Name;
            //    _dt.Columns.Add(item.Name);
            //}
            foreach (var item in _headers)
            {
                var _propItem = _props.FirstOrDefault(c => c.Name == item);
                _displayName = _propItem.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                _dt.Columns.Add(_propItem.Name);
                _dicHeaderNames.Add(_propItem.Name, _displayName);
            }
            //数据转换成datatable
            foreach (var item in _list)
            {
                DataRow _dr = _dt.NewRow();
                for (int i = 0; i < _dt.Columns.Count; i++)
                {
                    var _value = _props[i].GetValue(item);
                    if (_props[i].Name == "IsDeleted")
                    {
                        _dr[_props[i].Name] = _value.ToString() == "0" ? "未删除" : "已删除";
                        continue;
                    }
                    _dr[_props[i].Name] = _value;
                }
                _dt.Rows.Add(_dr);
            }
            for (int i = 0; i < _dt.Columns.Count; i++)
            {
                _dt.Columns[i].ColumnName = _dicHeaderNames[_dt.Columns[i].ColumnName];
            }
            int _r = DataTableToExcel(_dt, "Sheet1", true);
            return _r > 0;
        }
        public List<T> Import<T>(List<string> _files) where T : class, new()
        {
            List<T> _list = new List<T>();
            var _props = typeof(T).GetProperties().Where(c => c.GetMethod.IsVirtual == false && !_fieldsNot.Contains(c.Name)).ToArray();
            var _dic = new Dictionary<string, string>();
            var _displayName = "";
            foreach (var item in _props)
            {
                _displayName = item.GetCustomAttribute<DisplayAttribute>()?.Name;
                if (string.IsNullOrEmpty(_displayName))
                    _displayName = item.Name;
                _dic.Add(_displayName, item.Name);
            }
            foreach (var item in _files)
            {
                ExcelHelper _excelHelper = new ExcelHelper(item);
                var _dt = _excelHelper.ExcelToDataTable();
                if (_dt != null)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        T _t = new T();
                        for (int j = 0; j < _dt.Columns.Count; j++)
                        {
                            if (_dic[_dt.Columns[j].ColumnName.Trim()] != null)
                            {
                                var _value = _dt.Rows[i][_dt.Columns[j].ColumnName];
                                var _pop = _t.GetType().GetProperty(_dic[_dt.Columns[j].ColumnName.Trim()].ToString());
                                _t.GetType().GetProperty(_dic[_dt.Columns[j].ColumnName.Trim()].ToString()).SetValue(_t, _value);
                            }
                        }
                        _list.Add(_t);
                    }
                }
            }
            return _list;
        }
        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();
            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }
                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    ICellStyle style = workbook.CreateCellStyle();
                    //style.FillBackgroundColor=NPOI.HSSF.Util.HSSFColor.Green.Index;
                    style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
                    style.FillPattern = FillPattern.SolidForeground;
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                        row.Cells[j].CellStyle = style;
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }
                for (i = 0; i < data.Rows.Count; ++i)
                {
                    if (data.Rows[i][0].ToString() != "")
                    {
                        IRow row = sheet.CreateRow(count);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                        }
                        ++count;
                    }
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName = "Sheet1", bool isFirstRowColumn = true)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                            data.Columns.Add(column);
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        if (row.GetCell(0) == null || row.GetCell(0).ToString() == "") continue;
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }
                fs = null;
                disposed = true;
            }
        }
    }
}
