using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
//using SpreadsheetLight;
using System.Data;

namespace Common
{
    public class ExcelHelper
    {
        public DataTable ExcelToDatatable(string FileName)
        {
            try
            {
                if (FileName.EndsWith(".xls"))
                    return XlsToDataTable(FileName);
                else if (FileName.EndsWith(".xlsx"))
                    return XlsxToDataTable(FileName);
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable XlsToDataTable(string fileName)
        {
            DataTable dt = new DataTable();
            Stream stream = null;
            try
            {
                stream = File.OpenRead(fileName);
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
                HSSFSheet hssfsheet = (HSSFSheet)hssfworkbook.GetSheetAt(hssfworkbook.ActiveSheetIndex);
                HSSFRow hssfrow = (HSSFRow)hssfsheet.GetRow(0);
                int lastCellNum = (int)hssfrow.LastCellNum;
                for (int i = (int)hssfrow.FirstCellNum; i < lastCellNum; i++)
                {
                    DataColumn column = new DataColumn(hssfrow.GetCell(i).StringCellValue);
                    dt.Columns.Add(column);
                }
                dt.TableName = hssfsheet.SheetName;
                int lastRowNum = hssfsheet.LastRowNum;
                //列名后,从TABLE第二行开始进行填充数据
                for (int i = hssfsheet.FirstRowNum + 1; i < hssfsheet.LastRowNum; i++)//
                {
                    HSSFRow hssfrow2 = (HSSFRow)hssfsheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    for (int j = (int)hssfrow2.FirstCellNum; j < lastCellNum; j++)//
                    {
                        dataRow[j] = hssfrow2.GetCell(j);//
                    }
                    dt.Rows.Add(dataRow);
                }
                stream.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return dt;
        }

        public DataTable XlsxToDataTable(string fileName)
        {
            DataTable dt = new DataTable();

            Stream stream = null;
            try
            {
                stream = File.OpenRead(fileName);
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(stream);
                XSSFSheet xssfsheet = (XSSFSheet)xssfworkbook.GetSheetAt(xssfworkbook.ActiveSheetIndex);
                XSSFRow hssfrow = (XSSFRow)xssfsheet.GetRow(0);
                int lastCellNum = (int)hssfrow.LastCellNum;
                for (int i = (int)hssfrow.FirstCellNum; i < lastCellNum; i++)
                {
                    DataColumn column = new DataColumn(hssfrow.GetCell(i).StringCellValue);
                    dt.Columns.Add(column);
                }
                dt.TableName = xssfsheet.SheetName;
                int lastRowNum = xssfsheet.LastRowNum;
                //列名后,从TABLE第二行开始进行填充数据
                for (int i = xssfsheet.FirstRowNum + 1; i < xssfsheet.LastRowNum; i++)//
                {
                    XSSFRow hssfrow2 = (XSSFRow)xssfsheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    for (int j = (int)hssfrow2.FirstCellNum; j < lastCellNum; j++)//
                    {
                        dataRow[j] = hssfrow2.GetCell(j);//
                    }
                    dt.Rows.Add(dataRow);
                }
                stream.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return dt;
        }
    }
}
