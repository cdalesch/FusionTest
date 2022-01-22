namespace FusionTest.Models
{
    internal class CustomerParts
    {
        public string Location { get; set; }
        public ICollection<Parts> Parts { get; set; }

    }

    internal class Parts
    {
        public string CategoryType { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

    }
}
