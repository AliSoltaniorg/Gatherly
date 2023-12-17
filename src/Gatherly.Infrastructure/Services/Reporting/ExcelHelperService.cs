using Gatherly.Application.Abstractions.Reporting;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Gatherly.Infrastructure.Services.Reporting
{
  internal class ExcelApplication :
    Microsoft.Office.Interop.Excel.ApplicationClass,
    Microsoft.Office.Interop.Excel._Application,
    Microsoft.Office.Interop.Excel.Application,
    Microsoft.Office.Interop.Excel.AppEvents_Event,
    IDisposable
  {
    private readonly object _objLock;
    public ExcelApplication() : base()
    {
      _objLock = new object();
    }

    ~ExcelApplication()
    {
      Dispose();
    }

    private bool _disposed = false;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    protected virtual void Dispose(bool disposing)
    {
      if (_disposed)
        return;

      lock (_objLock)
      {
        if (_disposed)
          return;
        if (disposing)
          try
          {
            if (Worksheets != null)
              Marshal.ReleaseComObject(Worksheets);
            if (Workbooks != null)
            {
              Workbooks.Close();
              Marshal.ReleaseComObject(Workbooks);
            }
            base.Quit();
            Marshal.ReleaseComObject(this);
          }
          catch
          {
            throw;
          }
        _disposed = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

  }
  internal class ExcelHelperService : IExcelHelperService
  {
    private readonly ExcelApplication application;
    private readonly Workbook workbook;
    public ExcelHelperService()
    {
      application = new ExcelApplication();
      workbook = application.Workbooks.Add(Missing.Value);
      workbook.Worksheets.Add();
      Worksheet? worksheet = workbook.ActiveSheet as Worksheet;
      worksheet.Name = "";
      application.Visible = true;
    }

    public void Export(ExcelBuilder action)
    {
      for (int i = 0; i < action.Worksheets.Count; i++)
      {
        DataSheet table = action.Worksheets.ElementAt(i);

        Worksheet? worksheet =
          i == 0 ?
            workbook.ActiveSheet as Worksheet :
            workbook.Worksheets.Add() as Worksheet;

        if (worksheet == null)
          break;

        worksheet.Name = table.SheetName;

        for(int c = 0; c < table.Columns.Count; c++)
        {
          worksheet.Cells[1, c + 1] = table.Columns[i].ColumnName;
        }
        int num2 = 2;
        DataRow[] array = table.Select();
        foreach (DataRow dataRow in array)
        {
          for (int j = 0; j < dataRow.ItemArray.Length; j++)
          {
            worksheet.Cells[num2, j + 1] = dataRow[j].ToString();
          }

          num2++;
        }

        worksheet.get_Range((object)"A1", (object)"Z1").Font.Bold = true;

        //for (int k = 0; k < hiddenRows.Length; k++)
        //{
        //  worksheet.get_Range((object)("A" + hiddenRows[k]), (object)("Z" + hiddenRows[k])).Hidden = true;
        //}

        worksheet.get_Range((object)"A1", (object)"Z1").VerticalAlignment = XlVAlign.xlVAlignCenter;
        worksheet.get_Range((object)"A1", (object)"Z1").EntireColumn.AutoFit();
      }
      application.Dispose();
    }
  }

  internal class ExcelBuilder
  {
    private string _saveFilePath = string.Empty;

    private readonly IList<DataSheet> _workSheets;
    public ExcelBuilder()
    {
      _workSheets = new List<DataSheet>();
    }
    public IReadOnlyCollection<DataSheet> Worksheets => _workSheets.AsReadOnly();

    public ExcelBuilder AddWorkSheet(DataSheet dataTable, bool includeColumns, string sheetName = null)
    {
      _workSheets.Add(dataTable);
      return this;
    }

    public ExcelBuilder SetFilePath(string saveFilePath)
    {
      _saveFilePath = saveFilePath;
      return this;
    }
  }

  public class DataSheet : System.Data.DataTable
  {
    public DataSheet(string tableName)
      :base(tableName)
    {
      SheetName = TableName;
      IncludeColumns = Columns.Count > 0;
    }
    public string SheetName { get; set; }
    public bool IncludeColumns { get; set; }
  }
}
