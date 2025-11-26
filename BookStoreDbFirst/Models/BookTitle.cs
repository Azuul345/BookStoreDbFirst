using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class BookTitle
{
    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public string? Language { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public int? AuthorId { get; set; }

    public int? PublisherId { get; set; }

    public int? GenreId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<StockBalance> StockBalances { get; set; } = new List<StockBalance>();
}
