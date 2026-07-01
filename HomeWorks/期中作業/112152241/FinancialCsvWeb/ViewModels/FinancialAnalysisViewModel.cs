using FinancialCsvWeb.Models;

namespace FinancialCsvWeb.ViewModels
{
    public class FinancialAnalysisViewModel
    {
        public int TotalCompanyCount { get; set; }

        public FinancialReport? TopAssetCompany { get; set; }

        public FinancialReport? TopDebtRatioCompany { get; set; }

        public FinancialReport? TopNetWorthCompany { get; set; }

        public List<FinancialReport> AssetRanking { get; set; } = new();

        public List<FinancialReport> DebtRatioRanking { get; set; } = new();

        public List<FinancialReport> NetWorthRanking { get; set; } = new();

        public List<string> AssetChartLabels { get; set; } = new();

        public List<decimal> AssetChartValues { get; set; } = new();

        public List<string> DebtRatioChartLabels { get; set; } = new();

        public List<decimal> DebtRatioChartValues { get; set; } = new();

        public List<string> NetWorthChartLabels { get; set; } = new();

        public List<decimal> NetWorthChartValues { get; set; } = new();
    }
}