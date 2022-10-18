using Microsoft.AspNetCore.Mvc.Rendering;

namespace VegetableShop.Mvc.Models.Page
{
    public class Pager
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int StartRecord { get; set; }
        public int EndRecord { get; set; }

        public string Action { get; set; } = "Index";
        public string Keyword { get; set; }
        public string SortExpression { get; set; }

        public Pager(int totalItems, int currentPage, int pageSize = 5)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;

            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            TotalPages = totalPages;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            StartRecord = (CurrentPage - 1) * PageSize + 1;

            EndRecord = StartRecord - 1 + PageSize;

            if (EndRecord > TotalItems)
            {
                EndRecord = TotalItems;
            }

            if (TotalItems == 0)
            {
                StartPage = 0;
                StartRecord = 0;
                CurrentPage = 0;
                EndRecord = 0;
            }
            else
            {
                StartPage = startPage;
                EndPage = endPage;
            }
        }

        public List<SelectListItem> GetPageSizes()
        {
            var pageSizes = new List<SelectListItem>();

            for (int lp = 5; lp <= 50; lp += 5)
            {
                if (lp == this.PageSize)
                {
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString(), true));
                }
                else
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }
            return pageSizes;
        }
    }
}
