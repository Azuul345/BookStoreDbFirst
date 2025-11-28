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



        //GET
        public async Task<List<StockBalance>> GetStockBalandeInfo()
        {
            return await _context.StockBalances
                .Include(sb => sb.Store)
                .Include(sb => sb.Isbn13Navigation)
                .ToListAsync();
        }


        public async Task<List<BookTitle>> GetAllBookTitlesInfo()
        {
            return await _context.BookTitles.ToListAsync();

        }

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
