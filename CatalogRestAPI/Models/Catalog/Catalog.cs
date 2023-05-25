namespace CatalogRestAPI.Models.Catalog
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
    }
}
