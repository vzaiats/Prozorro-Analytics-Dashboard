namespace ProzorroDataMining.Domain.Entities
{
    // Contract
    public class Contract
    {
        public Guid ContractId { get; set; }
        public string TenderId { get; set; }
        public decimal Amount { get; set; }
    }
}
