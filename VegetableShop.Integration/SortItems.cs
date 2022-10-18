namespace VegetableShop
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public class SortItems
    {
        private string UpIcon = "fa fa-arrow-up";
        private string DownIcon = "fa fa-arrow-down";
        public string SortedProperty { get; set; }
        public SortOrder SortOrder { get; set; }

        private List<SortableColumn> sortableColumns = new List<SortableColumn>();
        public void AddColumn(string columnName, bool isDefaultColumn = false)
        {
            SortableColumn temp = sortableColumns.Where(x => x.ColumnName.ToLower() == columnName.ToLower()).SingleOrDefault();
            if (temp is null)
            {
                sortableColumns.Add(new SortableColumn()
                {
                    ColumnName = columnName,
                });
            }
            if (isDefaultColumn || sortableColumns.Count == 1)
            {
                SortOrder = SortOrder.Ascending;
                SortedProperty = columnName;
            }
        }

        public SortableColumn GetColumn(string columnName)
        {
            SortableColumn temp = sortableColumns.Where(x => x.ColumnName.ToLower() == columnName.ToLower()).SingleOrDefault();
            if (temp is null)
            {
                sortableColumns.Add(new SortableColumn()
                {
                    ColumnName = columnName,
                });
            }
            return temp;
        }
        public void ApplySort(string sortExpression)
        {
            if (sortExpression is null)
            {
                sortExpression = this.SortedProperty.ToLower();
            }
            foreach (SortableColumn item in sortableColumns)
            {
                item.SortIcon = "";
                item.SortExpression = item.ColumnName;
                if (sortExpression == item.ColumnName.ToLower() + "_asc")
                {
                    SortOrder = SortOrder.Ascending;
                    SortedProperty = item.ColumnName;
                    item.SortExpression = item.ColumnName + "_desc";
                    item.SortIcon = DownIcon;
                }
                if (sortExpression == item.ColumnName.ToLower() + "_desc")
                {
                    SortOrder = SortOrder.Ascending;
                    SortedProperty = item.ColumnName;
                    item.SortExpression = item.ColumnName + "_asc";
                    item.SortIcon = UpIcon;
                }
            }
        }
    }
    public class SortableColumn
    {
        public string ColumnName { get; set; }
        public string SortExpression { get; set; }
        public string SortIcon { get; set; }
    }
}
