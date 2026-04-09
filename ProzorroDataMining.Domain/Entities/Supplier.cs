namespace ProzorroDataMining.Domain.Entities
{
    // Supplier
    public class Supplier
    {
        public Guid SupplierId { get; set; }
        public string TenderId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
