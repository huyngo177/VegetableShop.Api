namespace VegetableShop.Api.Dto.Page
{
    public class PaginationResult<T> : List<T>
    {
        public int TotalRecords { get; set; }
        public PaginationResult() { }
        public PaginationResult(List<T> source, int pageIndex, int pageSize)
        {
            TotalRecords = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            this.AddRange(items);
        }
    }
}
