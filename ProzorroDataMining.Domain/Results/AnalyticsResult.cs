namespace ProzorroDataMining.Domain.Results
{
    public class AnalyticsResult
    {
        public string ProcuringEntity { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal Savings { get; set; }
    }
}
