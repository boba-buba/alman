using Alman;
using Alman.SharedModels;
using DbAccess.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace DbAccess;

public partial class DbConnection : DbBase
{
    public Task<string> ExportToExcel()
    {
        string folderName = AlmanConfig.GetConfigString("ExcelDumping:FlderPath");
        var filePath = $"{folderName}{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.xlsx";

        // Load data from the database
        using (var context = ConnectToDb())
        {
            IWorkbook workbook = new XSSFWorkbook();
            
            ExportTable(context.GetDeclaredDbSet<Child>(), workbook.CreateSheet(nameof(Child)));
            ExportTable(context.GetDeclaredDbSet<Activity>(), workbook.CreateSheet(nameof(Activity)));
            ExportTable(context.GetDeclaredDbSet<ContractFee>(), workbook.CreateSheet(nameof(ContractFee)));
            ExportTable(context.GetDeclaredDbSet<FinalPayment>(), workbook.CreateSheet(nameof(FinalPayment)));
            ExportTable(context.GetDeclaredDbSet<Precontract>(), workbook.CreateSheet(nameof(Precontract)));
            ExportTable(context.GetDeclaredDbSet<StaffActivity>(), workbook.CreateSheet(nameof(StaffActivity)));
            ExportTable(context.GetDeclaredDbSet<StaffMember>(), workbook.CreateSheet(nameof(StaffMember)));
            ExportTable(context.GetDeclaredDbSet<YearMonthActivity>(), workbook.CreateSheet(nameof(YearMonthActivity)));
            ExportTable(context.GetDeclaredDbSet<Expense>(), workbook.CreateSheet(nameof(Expense)));
            ExportTable(context.GetDeclaredDbSet<YearMonthOther>(), workbook.CreateSheet(nameof(YearMonthOther)));
            ExportTable(context.GetDeclaredDbSet<YearMonthStaffActivity>(), workbook.CreateSheet(nameof(YearMonthStaffActivity)));
            ExportTable(context.GetDeclaredDbSet<YearResult>(), workbook.CreateSheet(nameof(YearResult)));
            ExportTable(context.GetDeclaredDbSet<YearSub>(), workbook.CreateSheet(nameof(YearSub)));

            // Save the workbook to a file
            using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
        return Task.FromResult(filePath);
    }


    private void ExportTable<TEntity>(DbSet<TEntity> items, ISheet sheet) where TEntity : class
    {
        // Load data from the database
        
        var data = items.ToList();

        // Create header row
        IRow headerRow = sheet.CreateRow(0);
        var properties = typeof(TEntity).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            bool isICollection = properties[i].PropertyType.IsGenericType &&
                                properties[i].PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>);
            
            bool isClass = typeof(IIdentifier).IsAssignableFrom(properties[i].PropertyType);

            if (!isICollection && !isClass)
            {
                headerRow.CreateCell(i).SetCellValue(properties[i].Name);
            }
        }

        // Fill data rows
        for (int i = 0; i < data.Count; i++)
        {
            IRow row = sheet.CreateRow(i + 1);
            for (int j = 0; j < properties.Length; j++)
            {
                bool isICollection = properties[j].PropertyType.IsGenericType &&
                                properties[j].PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>);
                bool isClass = typeof(IIdentifier).IsAssignableFrom(properties[j].PropertyType);

                if (!isICollection && !isClass)
                {
                    var value = properties[j].GetValue(data[i])?.ToString();
                    row.CreateCell(j).SetCellValue(value);
                }
            }
        }

        
        
    }
}
