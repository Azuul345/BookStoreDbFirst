using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class StockBalance
{
    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int? BookAmounts { get; set; }

    public virtual BookTitle Isbn13Navigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Store Store { get; set; } = null!;
}
