using BookStoreDbFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreDbFirst
{
    public class DbService
    {

        private readonly BookStoreContext _context;

        public DbService(BookStoreContext context)
        {
            _context = context;
        }


        //Create 
        public async Task<BookTitle> CreateBookTitle(BookTitle title)
        {
            _context.BookTitles.Add(title);
            await _context.SaveChangesAsync();
            return title;
        }

        public async Task<Author> CreateNewAuthor(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }



        //READ
        public async Task<List<StockBalance>> GetStockBalandeInfo()
        {
            return await _context.StockBalances
                .Include(sb => sb.Store)
                .Include(sb => sb.Isbn13Navigation)
                .ToListAsync();
        }

        //Author
        public async Task<List<Author>> GetAllAuthors()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<List<Publisher>> GetAllPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        //BookTitle
        public async Task<List<BookTitle>> GetAllBookTitlesInfo()
        {
            return await _context.BookTitles.ToListAsync();

        }
        //Stores
        public async Task<List<Store>> GetAllStoreInfo()
        {
            return await _context.Stores.ToListAsync();
        }


        //UPDATE
        public async Task<StockBalance> UpdateStockBalance(StockBalance stockBalance)
        {
            //_context.StockBalances.Update(stockBalance);
            await _context.SaveChangesAsync();
            return stockBalance;
        }

        //public async Task<List<Store>> GetAllStores()
        //{
        //    return await _context.Stores.ToListAsync();
        //}



    }
}
