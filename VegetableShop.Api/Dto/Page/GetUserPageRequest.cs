namespace VegetableShop.Api.Dto.Page
{
    public class GetUserPageRequest : PageBase
    {
        public string? Keyword { get; set; }
        public bool? IsLocked { get; set; }
    }
}
