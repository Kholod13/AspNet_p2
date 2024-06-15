namespace Data.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // ------- navigation property
        public ICollection<Product>? Products { get; set; }
    }
}