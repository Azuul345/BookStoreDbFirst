using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string PublisherName { get; set; } = null!;

    public string? Country { get; set; }

    public virtual ICollection<BookTitle> BookTitles { get; set; } = new List<BookTitle>();
}
