using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class Inventoryinfo
{
    public string? StoreName { get; set; }

    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? BookAmounts { get; set; }
}
