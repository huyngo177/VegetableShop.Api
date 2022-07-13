namespace VegetableShop.Api.Dto.Page
{
    public class PageResult<T> : PageBase
    {
        public List<T> Items { set; get; }
    }
}
