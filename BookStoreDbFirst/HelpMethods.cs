namespace BookStoreDbFirst
{
    public class HelpMethods
    {




        static async Task ViewStockBalanceInfo(DbService dbs)
        {
            var stockinfo = await dbs.GetStockBalandeInfo();

            Console.WriteLine("=== Stock Balance ===");
            foreach (var s in stockinfo)
            {
                Console.WriteLine($"{s.Store} || {s.StoreId} || {s.Isbn13Navigation} || {s.BookAmounts} || {s.Orders}");
            }
        }
    }
}
