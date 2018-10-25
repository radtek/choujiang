using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace XGTech.Utility
{
    public class ExcelHelper
    {
        /// <summary>
        /// 导出DataTable到Excel中(不输出表头)
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="filepath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool DataTableToExcelNoHead(DataTable datatable, string filepath, out string error)
        {
            error = string.Empty;

            try
            {
                if (datatable == null)
                {
                    error = "DataTableToExcel:datatable 为空";
                    return false;
                }

                IWorkbook workbook;
                string fileExt = Path.GetExtension(filepath).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook();
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook();
                else
                    workbook = null;

                if (workbook == null)
                    throw new Exception("文件后缀名错误");

                var sheet = workbook.CreateSheet("Sheet1");
                int nRow = 0;

                foreach (DataRow dr in datatable.Rows)
                {
                    try
                    {
                        IRow row = sheet.CreateRow(nRow);

                        for (int i = 0; i < datatable.Columns.Count; i++)
                        {
                            if (dr[i].GetType().ToString() == "System.Drawing.Bitmap")
                            {
                                //------插入图片数据-------
                                System.Drawing.Image image = (System.Drawing.Image)dr[i];
                                System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                                image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                int pictureIndex = workbook.AddPicture(mstream.GetBuffer(), PictureType.PNG);
                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, i, nRow);
                                IPicture pict = patriarch.CreatePicture(anchor, pictureIndex);
                            }
                            else
                                row.CreateCell(i).SetCellValue(dr[i].ToString());
                        }

                        nRow++;
                    }
                    catch (System.Exception e)
                    {
                        error = error + " DataTableToExcel: " + e.Message;
                    }
                }

                var file = new FileStream(filepath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                return true;
            }
            catch (System.Exception e)
            {
                error = error + " DataTableToExcel: " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// 导出DataTable到Excel中
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="filepath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool DataTableToExcel(DataTable datatable, string filepath, out string error)
        {
            error = "";
            try
            {
                if (datatable == null)
                {
                    error = "DataTableToExcel:datatable 为空";
                    return false;
                }

                IWorkbook workbook;
                string fileExt = Path.GetExtension(filepath).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook();
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook();
                else
                    workbook = null;

                if (workbook == null)
                    throw new Exception("文件后缀名错误");

                var sheet = workbook.CreateSheet("Sheet1");
                int nRow = 0;

                IRow rowColumn = sheet.CreateRow(nRow);
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    rowColumn.CreateCell(i).SetCellValue(datatable.Columns[i].ColumnName);
                }

                nRow = 1;

                foreach (DataRow dr in datatable.Rows)
                {
                    try
                    {
                        IRow row = sheet.CreateRow(nRow);

                        for (int i = 0; i < datatable.Columns.Count; i++)
                        {
                            if (dr[i].GetType().ToString() == "System.Drawing.Bitmap")
                            {
                                //------插入图片数据-------
                                System.Drawing.Image image = (System.Drawing.Image)dr[i];
                                System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                                image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                int pictureIndex = workbook.AddPicture(mstream.GetBuffer(), PictureType.PNG);
                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, i, nRow);
                                IPicture pict = patriarch.CreatePicture(anchor, pictureIndex);
                            }
                            else if (dr[i].GetType().ToString() == "System.Decimal")
                            {
                                DecimalFormat f = new DecimalFormat(".##");//小数只有两位
                                String value = f.Format(dr[i].ToString());

                                row.CreateCell(i).SetCellValue(value);
                            }
                            else
                            {
                                row.CreateCell(i).SetCellValue(dr[i].ToString());
                            }
                        }

                        nRow++;
                    }
                    catch (System.Exception e)
                    {
                        error = error + " DataTableToExcel: " + e.Message;
                    }
                }

                var file = new FileStream(filepath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                return true;
            }
            catch (System.Exception e)
            {
                error = error + " DataTableToExcel: " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// 导出DataTable列到Excel中
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="filepath"></param>
        /// <param name="error"></param>
        /// <param name="hiddens">需隐藏的列逗号隔开</param>
        /// <returns></returns>
        public static bool DataTableToExcelWithHiddens(DataTable datatable, string filepath, out string error, string hiddens)
        {
            if (!string.IsNullOrEmpty(hiddens))
            {
                string[] _hiddens = hiddens.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (_hiddens.Length > 0)
                {
                    for (int i = 0; i < _hiddens.Length; i++)
                    {
                        datatable.Columns.Remove(_hiddens[i]);

                    }
                }
            }
            error = "";
            return DataTableToExcel(datatable, filepath, out error);

        }
        #region 社保分摊列表-含合并单元格

        /// <summary>
        /// 导出DataTable到Excel中
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="filepath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool DataTableToExcelByShebaoft(DataTable datatable, string filepath, out string error)
        {
            error = "";
            try
            {
                if (datatable == null)
                {
                    error = "DataTableToExcel:datatable 为空";
                    return false;
                }

                IWorkbook workbook;
                string fileExt = Path.GetExtension(filepath).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook();
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook();
                else
                    workbook = null;

                if (workbook == null)
                    throw new Exception("文件后缀名错误");

                var sheet = workbook.CreateSheet("Sheet1");
                int nRow = 0;

                //需要时取消注释
                //ICellStyle style = workbook.CreateCellStyle();
                //style.Alignment = HorizontalAlignment.Center;
                //style.WrapText = true;

                //IFont font = workbook.CreateFont();
                //font.FontHeightInPoints = 10;
                //font.Boldweight = (short)FontBoldWeight.Bold;
                //font.FontName = "標楷體";
                //style.SetFont(font);//HEAD 样式

                //ICellStyle cellstyle = workbook.CreateCellStyle();//设置垂直居中格式
                //cellstyle.VerticalAlignment = VerticalAlignment.Center;


                IRow rowColumn = sheet.CreateRow(nRow);
                rowColumn.HeightInPoints = 30;
                ICell cell;
                //var rowCount = datatable.Columns.Count;
                sheet.AddMergedRegion(new CellRangeAddress(0, 2, 0, 0));
                cell = rowColumn.CreateCell(0);
                cell.SetCellValue("一级部门");
                sheet.AddMergedRegion(new CellRangeAddress(0, 2, 1, 1));
                cell = rowColumn.CreateCell(1);
                cell.SetCellValue("二级部门");

                sheet.AddMergedRegion(new CellRangeAddress(0, 2, 6, 6));
                cell = rowColumn.CreateCell(6);
                cell.SetCellValue("个人承担");

                sheet.AddMergedRegion(new CellRangeAddress(0, 2, 7, 7));
                cell = rowColumn.CreateCell(7);
                cell.SetCellValue("公司实际承担");

                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 5));
                cell = rowColumn.CreateCell(2);
                cell.SetCellValue("企业承担");

                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 3));
                IRow rowColumn1 = sheet.CreateRow(1);
                cell = rowColumn1.CreateCell(2);
                cell.SetCellValue("申报社保");

                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 4, 5));
                cell = rowColumn1.CreateCell(4);
                cell.SetCellValue("仅申报工伤、生育");

                IRow rowColumn2 = sheet.CreateRow(2);
                cell = rowColumn2.CreateCell(2);
                cell.SetCellValue("人数");
                cell = rowColumn2.CreateCell(3);
                cell.SetCellValue("金额");
                cell = rowColumn2.CreateCell(4);
                cell.SetCellValue("人数");
                cell = rowColumn2.CreateCell(5);
                cell.SetCellValue("金额");
                //cell.CellStyle = style;

                //rowColumn.CreateCell(4).SetCellValue("仅申报工伤、生育");
                //sheet.AddMergedRegion(new CellRangeAddress(1, 0, 4, 5));

                //nRow = 1;
                nRow = 3;

                foreach (DataRow dr in datatable.Rows)
                {
                    try
                    {
                        IRow row = sheet.CreateRow(nRow);

                        for (int i = 0; i < datatable.Columns.Count; i++)
                        {
                            if (dr[i].GetType().ToString() == "System.Drawing.Bitmap")
                            {
                                //------插入图片数据-------
                                System.Drawing.Image image = (System.Drawing.Image)dr[i];
                                System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                                image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                int pictureIndex = workbook.AddPicture(mstream.GetBuffer(), PictureType.PNG);
                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, i, nRow);
                                IPicture pict = patriarch.CreatePicture(anchor, pictureIndex);
                            }
                            else
                            {
                                cell = row.CreateCell(i);
                                cell.SetCellValue(dr[i].ToString());
                            }
                        }

                        nRow++;
                    }
                    catch (System.Exception e)
                    {
                        error = error + " DataTableToExcel: " + e.Message;
                    }
                }

                var file = new FileStream(filepath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                return true;
            }
            catch (System.Exception e)
            {
                error = error + " DataTableToExcel: " + e.Message;
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 导出List到Excel中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLiat"></param>
        /// <param name="filepath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ListToExcel<T>(List<T> dataLiat, string filepath, out string error, string exportcolumns = null)
        {
            error = "";
            try
            {
                if (dataLiat == null)
                {
                    error = "ListToExcel:dataLiat 为空";
                    return false;
                }

                //获得反射的入口
                Type type = typeof(T);
                IWorkbook workbook;
                string fileExt = Path.GetExtension(filepath).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook();
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook();
                else
                    workbook = null;

                if (workbook == null)
                    throw new Exception("文件后缀名错误");

                var sheet = workbook.CreateSheet("Sheet1");
                int nRow = 0;

                IRow rowColumn = sheet.CreateRow(nRow);
                int columnIndex = 0;

                foreach (PropertyInfo pro in type.GetProperties())
                {
                    string displayName = GetAttrName(pro);
                    if (displayName == "")
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(exportcolumns) && !("," + exportcolumns + ",").Contains("," + displayName + ","))
                    {
                        continue;
                    }

                    rowColumn.CreateCell(columnIndex).SetCellValue(displayName);
                    columnIndex++;
                }

                nRow = 1;

                foreach (var item in dataLiat)
                {
                    try
                    {
                        IRow row = sheet.CreateRow(nRow);
                        int proIndex = 0;

                        foreach (PropertyInfo pro in type.GetProperties())
                        {
                            string displayName = GetAttrName(pro);
                            if (displayName == "")
                            {
                                continue;
                            }
                            if (!string.IsNullOrEmpty(exportcolumns) && !("," + exportcolumns + ",").Contains("," + displayName + ","))
                            {
                                continue;
                            }

                            if (pro.PropertyType.ToString() == "System.Drawing.Bitmap")
                            {
                                //------插入图片数据-------
                                System.Drawing.Image image = (System.Drawing.Image)pro.GetValue(item, null);
                                System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                                image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                int pictureIndex = workbook.AddPicture(mstream.GetBuffer(), PictureType.PNG);
                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, proIndex, nRow);
                                IPicture pict = patriarch.CreatePicture(anchor, pictureIndex);
                            }
                            else
                            {
                                var value = pro.GetValue(item, null);
                                row.CreateCell(proIndex).SetCellValue(value == null ? string.Empty : value.ToString());
                            }

                            proIndex++;
                        }

                        nRow++;
                    }
                    catch (System.Exception e)
                    {
                        error = error + " ListToExcel: " + e.Message;
                    }
                }

                var file = new FileStream(filepath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                return true;
            }
            catch (System.Exception e)
            {
                error = error + " ListToExcel: " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// 导出List到Excel中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLiat"></param>
        /// <param name="filepath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ListToExcel<T>(List<T> dataLiat, string filepath, out string error)
        {
            error = "";
            try
            {
                if (dataLiat == null)
                {
                    error = "ListToExcel:dataLiat 为空";
                    return false;
                }

                //获得反射的入口
                Type type = typeof(T);
                IWorkbook workbook;
                string fileExt = Path.GetExtension(filepath).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook();
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook();
                else
                    workbook = null;

                if (workbook == null)
                    throw new Exception("文件后缀名错误");

                var sheet = workbook.CreateSheet("Sheet1");
                int nRow = 0;

                IRow rowColumn = sheet.CreateRow(nRow);
                int columnIndex = 0;

                foreach (PropertyInfo pro in type.GetProperties())
                {
                    string displayName = GetAttrName(pro);
                    if (displayName == "")
                    {
                        continue;
                    }
                    rowColumn.CreateCell(columnIndex).SetCellValue(displayName);
                    columnIndex++;
                }

                nRow = 1;

                foreach (var item in dataLiat)
                {
                    try
                    {
                        IRow row = sheet.CreateRow(nRow);
                        int proIndex = 0;

                        foreach (PropertyInfo pro in type.GetProperties())
                        {
                            string displayName = GetAttrName(pro);
                            if (displayName == "")
                            {
                                continue;
                            }
                            if (pro.PropertyType.ToString() == "System.Drawing.Bitmap")
                            {
                                //------插入图片数据-------
                                System.Drawing.Image image = (System.Drawing.Image)pro.GetValue(item, null);
                                System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                                image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                int pictureIndex = workbook.AddPicture(mstream.GetBuffer(), PictureType.PNG);
                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, proIndex, nRow);
                                IPicture pict = patriarch.CreatePicture(anchor, pictureIndex);
                            }
                            else
                            {
                                var value = pro.GetValue(item, null);
                                row.CreateCell(proIndex).SetCellValue(value == null ? string.Empty : value.ToString());
                            }

                            proIndex++;
                        }

                        nRow++;
                    }
                    catch (System.Exception e)
                    {
                        error = error + " ListToExcel: " + e.Message;
                    }
                }

                var file = new FileStream(filepath, FileMode.Create);
                workbook.Write(file);
                file.Close();
                return true;
            }
            catch (System.Exception e)
            {
                error = error + " ListToExcel: " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// 读取Excel文档
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static DataTable ReadExcel(string strFileName)
        {
            IWorkbook workbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                string fileExt = Path.GetExtension(strFileName).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook(file);
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook(file);
                else
                    workbook = null;
            }

            if (workbook == null)
                throw new Exception("Excel文件格式错误");

            ISheet sheet = workbook.GetSheetAt(0);
            DataTable dtNew = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
            {
                dtNew.Columns.Add(headerRow.GetCell(i).StringCellValue);
            }

            for (int i = sheet.FirstRowNum + 1; i < sheet.LastRowNum + 1; i++)
            {
                IRow row = sheet.GetRow(i);

                if (row == null)
                    continue;

                DataRow drNew = dtNew.NewRow();

                for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (j < dtNew.Columns.Count)
                        {
                            row.GetCell(j).SetCellType(CellType.String);
                            drNew[j] = row.GetCell(j).StringCellValue;
                        }
                    }
                    //drNew[j] = row.GetCell(j).StringCellValue;
                }

                dtNew.Rows.Add(drNew);
            }

            return dtNew;
        }

        /// <summary>
        /// 读取Excel文档
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="SheetName"></param>
        /// <returns></returns>
        public static DataTable ReadExcel(string strFileName, string SheetName)
        {
            IWorkbook workbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                string fileExt = Path.GetExtension(strFileName).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook(file);
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook(file);
                else
                    workbook = null;
            }

            if (workbook == null)
                throw new Exception("Excel文件格式错误");

            ISheet sheet = workbook.GetSheet(SheetName);
            DataTable dtNew = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
            {
                dtNew.Columns.Add(headerRow.GetCell(i).StringCellValue);
            }

            for (int i = sheet.FirstRowNum + 1; i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                if (row == null)
                    continue;

                DataRow drNew = dtNew.NewRow();

                for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                {
                    drNew[j] = row.GetCell(j).StringCellValue;
                }

                dtNew.Rows.Add(drNew);
            }

            return dtNew;
        }

        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static string[] ReadExcelTitle(string strFileName)
        {
            IWorkbook workbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                string fileExt = Path.GetExtension(strFileName).ToLower();

                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook(file);
                else if (fileExt == ".xls")
                    workbook = new HSSFWorkbook(file);
                else
                    workbook = null;
            }

            if (workbook == null)
                throw new Exception("Excel文件格式错误");

            ISheet sheet = workbook.GetSheetAt(0);
            IRow headerRow = sheet.GetRow(0);

            string[] array = new string[headerRow.LastCellNum];
            for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
            {
                array[i] = headerRow.GetCell(i).StringCellValue;
            }

            return array;
        }

        /// <summary>
        /// 导出DataTable到Excel中（可以隐藏字段）
        /// </summary>
        /// <param name="hiddens">隐藏列</param>
        /// <param name="isNoHead">是否隐藏表头</param>
        public static bool ExcelOutResult(string strFileName, DataTable dt, string hiddens, out string error, bool isNoHead = false)
        {
            if (!string.IsNullOrEmpty(hiddens))
            {
                string[] hiddens_ = hiddens.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                List<DataColumn> dcList = new List<DataColumn>();

                foreach (DataColumn item in dt.Columns)
                {
                    if (hiddens_.Contains(item.ColumnName))
                        dcList.Add(item);
                }

                foreach (DataColumn item in dcList)
                {
                    dt.Columns.Remove(item);
                }
            }

            string FilePath = AppDomain.CurrentDomain.BaseDirectory + strFileName;
            error = string.Empty;
            if (!isNoHead)
            {
                return DataTableToExcel(dt, FilePath, out error);
            }
            else
            {
                return DataTableToExcelNoHead(dt, FilePath, out error);
            }
        }

        /// <summary>
        /// list转dataTable
        /// </summary>
        public static DataTable ListToDataTable<T>(List<T> dataLiat)
        {
            return ListToDataTable<T>(dataLiat, false);
        }
        /// <summary>
        /// list转dataTable
        /// </summary>
        /// <param name="isEN">是否显示未标示[Display(Name = "")]的列</param>
        public static DataTable ListToDataTable<T>(List<T> dataLiat, bool isEN)
        {
            //创建属性的集合    
            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            foreach (PropertyInfo pro in type.GetProperties())
            {
                string displayName = GetAttrName(pro, isEN);
                if (!string.IsNullOrEmpty(displayName))
                {
                    if (pro.PropertyType.Name == "DateTime")
                    {
                        propertyList.Add(pro);
                        dt.Columns.Add(displayName, typeof(string));
                    }
                    else if (pro.PropertyType.Name == "Nullable`1")
                    {
                        dt.Columns.Add(displayName, typeof(string));
                    }
                    else
                    {
                        propertyList.Add(pro);
                        dt.Columns.Add(displayName, pro.PropertyType);
                    }
                }
            }

            foreach (var item in dataLiat)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                foreach (PropertyInfo pro in propertyList)
                {
                    string displayName = GetAttrName(pro, isEN);
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        if (pro.PropertyType.Name == "DateTime")
                        {
                            string timeValue = pro.GetValue(item, null).ToString();
                            row[displayName] = DateTime.Parse(timeValue).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                            row[displayName] = pro.GetValue(item, null);
                    }
                }
                //加入到DataTable    
                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// 获得[Display(Name = "")]的值
        /// </summary>
        public static string GetAttrName(PropertyInfo pro, bool isEN = false)
        {
            var disAttr = pro.GetCustomAttribute(typeof(DisplayAttribute), true) as DisplayAttribute;
            if (isEN)
            {
                return disAttr != null ? disAttr.Name : pro.Name;
            }
            else
            {
                return disAttr != null ? disAttr.Name : "";
            }
        }
        public static string GetExportName<T>(bool isEN = false)
        {
            string excdata = "";
            Type inputDTO = typeof(T);
            if (inputDTO != null)
            {
                foreach (PropertyInfo pro in inputDTO.GetProperties())
                {
                    string displayName = ExcelHelper.GetAttrName(pro, isEN);
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        excdata += displayName + ",";
                    }
                }
            }
            if (excdata != "")
            {
                excdata = excdata.Substring(0, excdata.Length - 1);
            }
            return excdata;
        }
        //public void DataTabletoExcel(System.Data.DataTable tmpDataTable, string strFileName)
        //{

        //    ///先得到datatable的行数
        //    int rowNum = tmpDataTable.Rows.Count;
        //    ///列数
        //    int columnNum = tmpDataTable.Columns.Count;
        //    ///声明一个应用程序类实例
        //    Application xlApp = new ApplicationClass();

        //    //xlApp.DefaultFilePath = "";  ///默认文件路径，将其设置路径后发现没什么变化。导出excel的路径还是在参数strFileName里设置
        //    //xlApp.DisplayAlerts = true;
        //    //xlApp.SheetsInNewWorkbook = 1;///返回或设置 Microsoft Excel 自动插入到新工作簿中的工作表数目。Long 类型，可读写。设置为2之后没发现什么区别
        //    //创建一个新工作簿
        //    Workbook xlBook = xlApp.Workbooks.Add();
        //    ///在工作簿中得到sheet。
        //    _Worksheet oSheet = (_Worksheet)xlBook.Worksheets[1];
        //    #region 绘制列
        //    ///自定义方法，想sheet中绘制列
        //    RangeBuild(oSheet, "A1", "A2", "编号");
        //    RangeBuild(oSheet, "B1", "B2", "畜主");
        //    RangeBuild(oSheet, "C1", "C2", "地址");
        //    RangeBuild(oSheet, "D1", "D2", "区划");
        //    RangeBuild(oSheet, "E1", "E2", "规模");
        //    RangeBuild(oSheet, "F1", "H1", "总存栏量");
        //    RangeBuild(oSheet, "F2", "F2", "期初");
        //    RangeBuild(oSheet, "G2", "G2", "期末");
        //    RangeBuild(oSheet, "H2", "H2", "变更");
        //    RangeBuild(oSheet, "I1", "K1", "母猪");
        //    RangeBuild(oSheet, "I2", "I2", "期初");
        //    RangeBuild(oSheet, "J2", "J2", "期末");
        //    RangeBuild(oSheet, "K2", "K2", "变更");
        //    RangeBuild(oSheet, "L1", "N1", "肉猪");
        //    RangeBuild(oSheet, "L2", "L2", "期初");
        //    RangeBuild(oSheet, "M2", "M2", "期末");
        //    RangeBuild(oSheet, "N2", "N2", "变更");
        //    RangeBuild(oSheet, "O1", "Q1", "仔猪");
        //    RangeBuild(oSheet, "O2", "O2", "期初");
        //    RangeBuild(oSheet, "P2", "P2", "期末");
        //    RangeBuild(oSheet, "Q2", "Q2", "变更");
        //    RangeBuild(oSheet, "R1", "T1", "公猪");
        //    RangeBuild(oSheet, "R2", "R2", "期初");
        //    RangeBuild(oSheet, "S2", "S2", "期末");
        //    RangeBuild(oSheet, "T2", "T2", "变更");
        //    RangeBuild(oSheet, "U1", "W1", "总面积");
        //    RangeBuild(oSheet, "U2", "U2", "期初");
        //    RangeBuild(oSheet, "V2", "V2", "期末");
        //    RangeBuild(oSheet, "W2", "W2", "变更");
        //    RangeBuild(oSheet, "X1", "Z1", "批建");
        //    RangeBuild(oSheet, "X2", "X2", "期初");
        //    RangeBuild(oSheet, "Y2", "Y2", "期末");
        //    RangeBuild(oSheet, "Z2", "Z2", "变更");
        //    RangeBuild(oSheet, "AA1", "AC1", "未批建");
        //    RangeBuild(oSheet, "AA2", "AA2", "期初");
        //    RangeBuild(oSheet, "AB2", "AB2", "期末");
        //    RangeBuild(oSheet, "AC2", "AC2", "变更");
        //    #endregion
        //    //将DataTable中的数据导入Excel中
        //    for (int i = 0; i < rowNum; i++)
        //    {

        //        for (int j = 0; j < columnNum; j++)
        //        {
        //            ///excel中的列是从1开始的
        //            xlApp.Cells[i + 2, j + 1] = tmpDataTable.Rows[i][j].ToString();
        //        }
        //    }
        //    ///保存,路径一块穿进去。否则回到一个很奇妙的地方，貌似是system32里 temp下....
        //    oSheet.SaveAs(@"D:\a\" + strFileName);
        //}

        //private static void RangeBuild(_Worksheet oSheet, string startcell, string endcell, string value)
        //{
        //    ///创建一个区域对象。第一个参数是开始格子号，第二个参数是终止格子号。比如选中A1——D3这个区域。
        //    Range range = (Range)oSheet.get_Range(startcell, endcell);
        //    ///合并方法，0的时候直接合并为一个单元格
        //    range.Merge(0);
        //    ///合并单元格之后，设置其中的文本
        //    range.Value = value;
        //    //横向居中
        //    range.HorizontalAlignment = XlVAlign.xlVAlignCenter;
        //    ///字体大小
        //    range.Font.Size = 18;
        //    ///字体
        //    range.Font.Name = "黑体";
        //    ///行高
        //    range.RowHeight = 24;
        //    //自动调整列宽
        //    range.EntireColumn.AutoFit();
        //    //填充颜色
        //    range.Interior.ColorIndex = 20;
        //    //设置单元格边框的粗细
        //    range.Cells.Borders.LineStyle = 1;
        //}

        #region

    


        /// <summary>
        /// Delete special symbol
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string DelQuota(string str)
        {
            string result = str;
            string[] strQuota = { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "`", ";", "'", ",", ".", "/", ":", "/,", "<", ">", "?" };
            for (int i = 0; i < strQuota.Length; i++)
            {
                if (result.IndexOf(strQuota[i]) > -1)
                    result = result.Replace(strQuota[i], "");
            }
            return result;
        }
        #endregion
    }
}
