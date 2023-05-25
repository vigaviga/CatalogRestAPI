namespace CatalogRestAPI.Models.Catalog
{
    public class CatalogCreate
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }

    }
}
