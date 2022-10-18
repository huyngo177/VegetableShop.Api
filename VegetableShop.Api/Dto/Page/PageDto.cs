namespace VegetableShop.Api.Dto.Page
{
    public class PageDto
    {
        public string SortProperty { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public SortOrder SortOrder { get; set; }
        public string? Keyword { get; set; }
        public bool IsLocked { get; set; } = false;
    }
}
