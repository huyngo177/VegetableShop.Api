namespace VegetableShop.Api.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
