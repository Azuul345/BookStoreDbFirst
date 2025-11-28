namespace BookStoreDbFirst
{
    public static class HelpMethods
    {


        public static async Task ViewStockBalanceInfo(DbService dbs)
        {
            var stockinfo = await dbs.GetStockBalandeInfo();


            Console.WriteLine("=== Stock Balance ===");
            foreach (var s in stockinfo)
            {
                Console.WriteLine($"Store: {s.Store.StoreName} || Store ID: {s.StoreId} || Book Amount: {s.BookAmounts} || Title: {s.Isbn13Navigation.Title}");
            }
        }


        public static async Task UpdateStockBalanceFromExisting(DbService dbs)
        {
            await ViewStockBalanceInfo(dbs);
            Console.WriteLine("============ \n");
            Console.Write("Select Store ID to update: ");

            var storeinfo = await dbs.GetAllStoreInfo();
            int storeID = int.Parse(Console.ReadLine());
            if (storeID < 1 || storeID > storeinfo.Count)
            {
                Console.WriteLine("No Store Available");
                return;
            }


            await ShowAllBookTitles(dbs);
            Console.WriteLine("Select Book ID to update: ");

            var titlesinfo = await dbs.GetAllBookTitlesInfo();

            var bookID = int.Parse(Console.ReadLine());


            //if (!int.TryParse(Console.ReadLine(), out int bookID))
            //{
            //    Console.WriteLine("Invalid input for Book ID.");
            //    return;
            //}



            if (bookID < 1 || bookID > titlesinfo.Count)
            {
                Console.WriteLine("Book ID does not exist");
                return;
            }


            string selectedIsbn = titlesinfo[bookID - 1].Isbn13;

            var stockinfo = await dbs.GetStockBalandeInfo();


            Console.Write("Update the new amount of books: ");
            var bookAmount = int.Parse(Console.ReadLine());

            //if (!int.TryParse(Console.ReadLine(), out int bookAmount))
            //{
            //    Console.WriteLine("Invalid input for amount.");
            //    return;
            //}

            var update = stockinfo.FirstOrDefault(sb => sb.StoreId == storeID && sb.Isbn13 == selectedIsbn);


            if (update == null)
            {
                Console.WriteLine("No stock balance entry found for store and book combination ");
                return;
            }


            update.BookAmounts = bookAmount;

            Console.WriteLine($"Updated\n Store: {update.Store.StoreName} | Title: {update.Isbn13Navigation.Title} | Amount: {update.BookAmounts} ");

            await dbs.UpdateStockBalance(update);

            Console.WriteLine("Update successful.");
        }


        //Console.WriteLine($"{update[0].Store.StoreName} || {update[0].BookAmounts} || {update[0].Isbn13Navigation.Title}");
        //Console.WriteLine("\n\n");

        ////Console.WriteLine("End of test");
        //foreach (var s in stockinfo)
        //{
        //    Console.WriteLine($"Store: {s.Store.StoreName} || Store ID: {s.StoreId} || Book Amount: {s.BookAmounts} || Title: {s.Isbn13Navigation.Title}");
        //}


        public static async Task ShowAllBookTitles(DbService dbs)
        {
            var titles = await dbs.GetAllBookTitlesInfo();
            for (int i = 0; i < titles.Count; i++)
            {
                Console.WriteLine($"ID: {i + 1} ISBN13: {titles[i].Isbn13} Title: {titles[i].Title}");
            }
        }







        //add new title 
        // - check if author and publisher exist 
        //ISBN has to be 13
    }
}
