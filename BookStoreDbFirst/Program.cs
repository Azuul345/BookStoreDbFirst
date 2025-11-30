using BookStoreDbFirst.Models;

namespace BookStoreDbFirst
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var context = new BookStoreContext();
            var dbservice = new DbService(context);


            while (true)
            {
                Console.WriteLine("Admin Menu");
                Console.WriteLine("[1]: Check StockBalande");
                Console.WriteLine("[2]: Update StockBalande From existing Books");
                Console.WriteLine("[3]: Add new book title");
                Console.WriteLine("[4]: Exit Program");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await HelpMethods.ViewStockBalanceInfo(dbservice);
                        break;
                    case "2":
                        await HelpMethods.UpdateStockBalanceFromExisting(dbservice);
                        break;
                    case "3":
                        await HelpMethods.AddNewBookTitle(dbservice);
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                }
            }

            Console.ReadLine();
        }



    }
}
