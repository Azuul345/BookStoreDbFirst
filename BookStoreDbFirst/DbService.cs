using BookStoreDbFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreDbFirst
{
    internal class DbService
    {

        private readonly BookStoreContext _context;

        public DbService(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<StockBalance>> GetStockBalandeInfo()
        {
            return await _context.StockBalances.ToListAsync();
        }

        public async Task<List<Store>> GetAllStores()
        {
            return await _context.Stores.ToListAsync();
        }


    }
}
