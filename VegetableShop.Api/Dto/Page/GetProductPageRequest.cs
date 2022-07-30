namespace VegetableShop.Api.Dto.Page
{
    public class GetProductPageRequest : PageBaseRequest
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public string? Status { get; set; }
    }
}
