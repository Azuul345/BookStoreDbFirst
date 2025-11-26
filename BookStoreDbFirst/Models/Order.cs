using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int StoreId { get; set; }

    public DateOnly OrderDate { get; set; }

    public int Quantity { get; set; }

    public decimal PurchasePrice { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual BookTitle Isbn13Navigation { get; set; } = null!;

    public virtual StockBalance StockBalance { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
