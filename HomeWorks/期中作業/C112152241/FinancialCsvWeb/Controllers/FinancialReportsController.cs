using CsvHelper;
using FinancialCsvWeb.Data;
using FinancialCsvWeb.Models;
using FinancialCsvWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace FinancialCsvWeb.Controllers
{
    public class FinancialReportsController : Controller
    {
        private readonly AppDbContext _context;

        public FinancialReportsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? keyword, string? sort)
        {
            var query = _context.FinancialReports.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.CompanyCode.Contains(keyword) ||
                    x.CompanyName.Contains(keyword));
            }

            query = sort switch
            {
                "assets_desc" => query.OrderByDescending(x => x.TotalAssets),
                "assets_asc" => query.OrderBy(x => x.TotalAssets),
                "debt_desc" => query.OrderByDescending(x => x.TotalLiabilities),
                "equity_desc" => query.OrderByDescending(x => x.TotalEquity),
                "networth_desc" => query.OrderByDescending(x => x.NetWorthPerShare),
                _ => query.OrderBy(x => x.CompanyCode)
            };

            var data = await query.ToListAsync();

            ViewBag.Keyword = keyword;
            ViewBag.Sort = sort;

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var report = await _context.FinancialReports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Analysis()
        {
            var reports = await _context.FinancialReports.ToListAsync();

            var assetRanking = reports
                .OrderByDescending(x => x.TotalAssets ?? 0)
                .Take(5)
                .ToList();

            var debtRatioRanking = reports
                .Where(x => x.DebtRatio != null)
                .OrderByDescending(x => x.DebtRatio)
                .Take(5)
                .ToList();

            var netWorthRanking = reports
                .OrderByDescending(x => x.NetWorthPerShare ?? 0)
                .Take(5)
                .ToList();

            var model = new FinancialAnalysisViewModel
            {
                TotalCompanyCount = reports.Count,

                TopAssetCompany = reports
                    .OrderByDescending(x => x.TotalAssets ?? 0)
                    .FirstOrDefault(),

                TopDebtRatioCompany = reports
                    .Where(x => x.DebtRatio != null)
                    .OrderByDescending(x => x.DebtRatio)
                    .FirstOrDefault(),

                TopNetWorthCompany = reports
                    .OrderByDescending(x => x.NetWorthPerShare ?? 0)
                    .FirstOrDefault(),

                AssetRanking = assetRanking,
                DebtRatioRanking = debtRatioRanking,
                NetWorthRanking = netWorthRanking,

                AssetChartLabels = assetRanking.Select(x => x.CompanyName).ToList(),
                AssetChartValues = assetRanking.Select(x => x.TotalAssets ?? 0).ToList(),

                DebtRatioChartLabels = debtRatioRanking.Select(x => x.CompanyName).ToList(),
                DebtRatioChartValues = debtRatioRanking.Select(x => x.DebtRatio ?? 0).ToList(),

                NetWorthChartLabels = netWorthRanking.Select(x => x.CompanyName).ToList(),
                NetWorthChartValues = netWorthRanking.Select(x => x.NetWorthPerShare ?? 0).ToList()
            };

            return View(model);
        }
        public async Task<IActionResult> Import(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["Error"] = "請選擇 CSV 檔案。";
                return RedirectToAction(nameof(Import));
            }

            int importedCount = 0;
            int skippedCount = 0;

            using var stream = csvFile.OpenReadStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<dynamic>().ToList();

            foreach (var record in records)
            {
                var dict = (IDictionary<string, object>)record;

                int year = ToInt(dict, "年度");
                int quarter = ToInt(dict, "季別");
                string companyCode = ToStringValue(dict, "公司代號");

                bool exists = await _context.FinancialReports.AnyAsync(x =>
                    x.Year == year &&
                    x.Quarter == quarter &&
                    x.CompanyCode == companyCode);

                if (exists)
                {
                    skippedCount++;
                    continue;
                }

                var report = new FinancialReport
                {
                    ReportDate = ToStringValue(dict, "出表日期"),
                    Year = year,
                    Quarter = quarter,
                    CompanyCode = companyCode,
                    CompanyName = ToStringValue(dict, "公司名稱"),
                    TotalAssets = ToDecimal(dict, "資產總計"),
                    TotalLiabilities = ToDecimal(dict, "負債總計"),
                    TotalEquity = ToDecimal(dict, "權益總計"),
                    NetWorthPerShare = ToDecimal(dict, "每股參考淨值"),
                    RawJson = JsonSerializer.Serialize(dict),
                    ImportedAt = DateTime.Now
                };

                _context.FinancialReports.Add(report);
                importedCount++;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"匯入完成：新增 {importedCount} 筆，略過重複 {skippedCount} 筆。";

            return RedirectToAction(nameof(Index));
        }

        private static string ToStringValue(IDictionary<string, object> dict, string key)
        {
            if (!dict.ContainsKey(key) || dict[key] == null)
            {
                return string.Empty;
            }

            return dict[key]?.ToString()?.Trim() ?? string.Empty;
        }

        private static int ToInt(IDictionary<string, object> dict, string key)
        {
            string value = ToStringValue(dict, key);

            if (int.TryParse(value, out int result))
            {
                return result;
            }

            return 0;
        }

        private static decimal? ToDecimal(IDictionary<string, object> dict, string key)
        {
            string value = ToStringValue(dict, key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            return null;
        }
    }
}