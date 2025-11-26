using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<StockBalance> StockBalances { get; set; } = new List<StockBalance>();
}
