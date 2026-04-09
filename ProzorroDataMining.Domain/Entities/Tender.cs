using ProzorroDataMining.Domain.Entities;

namespace ProzorroDataMining.Domain.Models
{
    // Tender
    public class Tender
    {
        public string TenderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public string ProcuringEntity { get; set; } = string.Empty;
        public DateTime TenderDate { get; set; }
        public DateTime ImportedAt { get; set; }

        public string? CpvCode { get; set; }

        public List<Contract> Contracts { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
    }
}
