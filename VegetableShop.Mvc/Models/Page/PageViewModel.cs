namespace VegetableShop.Mvc.Models.Page
{
    public class PageViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortProperty { get; set; }
        public SortOrder SortOrder { get; set; }
        public string? Keyword { get; set; }
        public bool? IsLocked { get; set; }
    }
}
