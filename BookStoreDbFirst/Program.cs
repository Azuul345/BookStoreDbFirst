using BookStoreDbFirst.Models;

namespace BookStoreDbFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new BookStoreContext();
            var dbservice = new DbService(context);

            ViewStockBalanceInfo(dbservice);

            Console.ReadLine();
        }


        static async Task ViewStockBalanceInfo(DbService dbs)
        {
            var stockinfo = await dbs.GetStockBalandeInfo();

            Console.WriteLine("=== Stock Balance ===");
            foreach (var s in stockinfo)
            {
                Console.WriteLine($"  {s.Orders}");
            }
        }
    }
}
