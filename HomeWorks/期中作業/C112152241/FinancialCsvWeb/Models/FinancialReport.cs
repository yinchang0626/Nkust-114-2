using System.ComponentModel.DataAnnotations;

namespace FinancialCsvWeb.Models
{
    public class FinancialReport
    {
        public int Id { get; set; }

        [Display(Name = "出表日期")]
        public string ReportDate { get; set; } = string.Empty;

        [Display(Name = "年度")]
        public int Year { get; set; }

        [Display(Name = "季別")]
        public int Quarter { get; set; }

        [Display(Name = "公司代號")]
        [StringLength(20)]
        public string CompanyCode { get; set; } = string.Empty;

        [Display(Name = "公司名稱")]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "現金及約當現金")]
        public decimal? CashAndCashEquivalents { get; set; }

        [Display(Name = "資產總計")]
        public decimal? TotalAssets { get; set; }

        [Display(Name = "負債總計")]
        public decimal? TotalLiabilities { get; set; }

        [Display(Name = "股本")]
        public decimal? CapitalStock { get; set; }

        [Display(Name = "資本公積")]
        public decimal? CapitalSurplus { get; set; }

        [Display(Name = "保留盈餘")]
        public decimal? RetainedEarnings { get; set; }

        [Display(Name = "權益總計")]
        public decimal? TotalEquity { get; set; }

        [Display(Name = "每股參考淨值")]
        public decimal? NetWorthPerShare { get; set; }

        [Display(Name = "原始資料")]
        public string? RawJson { get; set; }

        [Display(Name = "匯入時間")]
        public DateTime ImportedAt { get; set; } = DateTime.Now;

        [Display(Name = "負債比")]
        public decimal? DebtRatio
        {
            get
            {
                if (TotalAssets == null || TotalAssets == 0 || TotalLiabilities == null)
                {
                    return null;
                }

                return TotalLiabilities / TotalAssets * 100;
            }
        }

        [Display(Name = "權益比")]
        public decimal? EquityRatio
        {
            get
            {
                if (TotalAssets == null || TotalAssets == 0 || TotalEquity == null)
                {
                    return null;
                }

                return TotalEquity / TotalAssets * 100;
            }
        }
    }
}