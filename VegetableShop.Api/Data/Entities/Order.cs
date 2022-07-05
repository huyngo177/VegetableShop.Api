﻿namespace VegetableShop.Api.Data.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { set; get; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public int UserId { set; get; }
        public virtual AppUser AppUser { get; set; }
    }
}
