using System;
using System.Collections.Generic;

namespace BookStoreDbFirst.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? Birthday { get; set; }

    public virtual ICollection<BookTitle> BookTitles { get; set; } = new List<BookTitle>();
}
