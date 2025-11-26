using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual ICollection<BookTitle> BookTitles { get; set; } = new List<BookTitle>();
}
