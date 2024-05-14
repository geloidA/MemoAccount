using System.IO;
using ClosedXML.Excel;
using MemoAccount.Models;

namespace MemoAccount.Services;

/// <summary>
/// Класс, отвечающий за создание отчета в формате Excel по служебным запискам.
/// </summary>
public class ReportService
{
    private const int DefaultRowHeight = 20;
    private const int DefaultColumnWidth = 22;

    public static void GenerateReport(IEnumerable<Memo> memos)
    {
        var downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        var filePath = Path.Combine(downloadsFolder, $"Служебные записки_{DateTime.Now:dd-MM-yyyy}.xlsx");

        using var workbook = new XLWorkbook();

        var worksheet = workbook.AddWorksheet("Отчет по служебным запискам");

        workbook.Style.Font.SetFontName("Times New Roman");

        worksheet.Cells().Style
            .Font.SetFontSize(12);

        worksheet.RowHeight = DefaultRowHeight;
        worksheet.ColumnWidth = DefaultColumnWidth;

        worksheet.Style
            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            .Fill.SetBackgroundColor(XLColor.White);

        var row = GenerateHeader(worksheet);

        memos.OrderBy(x => x.CreatedDate)
            .Aggregate(row, (current, memo) => GenerateMemoPart(worksheet, memo, current));

        workbook.SaveAs(filePath);
    }

    private static int GenerateHeader(IXLWorksheet worksheet)
    {
        worksheet
            .Range("A1:F1")
            .Merge()
            .SetValue("Отчет по служебным запискам")
            .Style.Font.SetBold(true)
            .Font.SetFontSize(20)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        worksheet.Row(1).Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        worksheet.Row(1).Height = 60;

        worksheet.Cell(1, 7).Value = $"Отчет за {DateTime.Now:dd-MM-yyyy}";

        worksheet.Row(3).Style
            .Font.SetBold(true)
            .Font.SetFontSize(14)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        worksheet.Range(3, 1, 3, 7).Style
            .Border.SetBottomBorder(XLBorderStyleValues.Thin);

        worksheet.Row(3).Height = 40;

        worksheet.Cell(3, 1).Value = "Номер";
        worksheet.Cell(3, 2).Value = "Статус";
        worksheet.Cell(3, 3).Value = "Содержание";
        worksheet.Cell(3, 4).Value = "Дата создания";
        worksheet.Cell(3, 5).Value = "Дата выполнения";
        worksheet.Cell(3, 6).Value = "Изъято со склада";
        worksheet.Cell(3, 7).Value = "Исполнитель";

        return 4;
    }

    private static int GenerateMemoPart(IXLWorksheet worksheet, Memo memo, int row)
    {
        worksheet.Cell(row, 1).Value = memo.Number;
        worksheet.Cell(row, 2).Value = memo.Status.GetDescription();
        worksheet.Cell(row, 2).Style
            .Fill.SetBackgroundColor(memo.Status == MemoStatus.Closed ? XLColor.LightGreen : XLColor.White);
        GeneratePossibleLongValue(worksheet, memo.Content, row, 3);
        worksheet.Cell(row, 4).Value = memo.CreatedDate.ToShortDateString();
        worksheet.Cell(row, 5).Value = memo.CompletionDate?.ToShortDateString() ?? "-";
        GeneratePossibleLongValue(worksheet, memo.ItemsWithdrawn ?? "-", row, 6);
        worksheet.Cell(row, 7).Value = memo.User!.Id == 0 ? "-" : memo.User.DisplayName;

        worksheet.Range(row, 1, row, 7)
            .Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Border.SetBottomBorderColor(XLColor.LightGray);

        return row + 1;
    }

    private static void GeneratePossibleLongValue(IXLWorksheet worksheet, string value, int row, int column)
    {
        worksheet.Cell(row, column).Value = value;
        if (value.Length <= 50) return;
        // ReSharper disable once PossibleLossOfFraction
        worksheet.Row(row).Height = value.Length / 50 * DefaultRowHeight;
        worksheet.Row(row).Style.Alignment.SetWrapText(true);
        // ReSharper disable once PossibleLossOfFraction
        worksheet.Column(column).Width= value.Length / 50 * DefaultColumnWidth;
    }
}