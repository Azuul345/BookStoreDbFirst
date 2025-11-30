using BookStoreDbFirst.Models;

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




        public static async Task ShowAllBookTitles(DbService dbs)
        {
            var titles = await dbs.GetAllBookTitlesInfo();
            for (int i = 0; i < titles.Count; i++)
            {
                Console.WriteLine($"ID: {i + 1} ISBN13: {titles[i].Isbn13} Title: {titles[i].Title}");
            }
        }



        ///  VG 




        public static async Task<Author> ChooseFromExistingAuthor(DbService dbs)
        {
            Console.WriteLine("Enter index of Author you wish to add title to");
            var allAuthors = await dbs.GetAllAuthors();

            for (int i = 0; i < allAuthors.Count; i++)
            {
                Console.WriteLine($"Index {i + 1}. First Name: {allAuthors[i].FirstName} Last Name: {allAuthors[i].LastName} Birthday: {allAuthors[i].Birthday}");
            }
            if (!int.TryParse(Console.ReadLine(), out int authorID) || authorID > allAuthors.Count || authorID <= 0)
            {
                Console.WriteLine("Invalid input for Author index.");
                return null;
            }
            var author = allAuthors[authorID - 1];

            //Console.WriteLine($"{author.FirstName} {author.LastName}");
            return author;
        }

        public static async Task ChooseFromExistingPublishers(DbService dbs)
        {
            Console.WriteLine("Enter index of publishers for the title");
            var allpublishers = await dbs.GetAllPublishers();

            for (int i = 0; i < allpublishers.Count; i++)
            {
                Console.WriteLine($"Index:{i + 1} Publisher: {allpublishers[i].PublisherName} Country: {allpublishers[i].Country}");
            }

            if (!int.TryParse(Console.ReadLine(), out int publisherId) || publisherId > allpublishers.Count || publisherId <= 0)
            {
                Console.WriteLine("Invalid input for publisher index");
                return;
            }

            var publisher = allpublishers[publisherId - 1];
            Console.WriteLine($"Selected publisher: {publisher.PublisherName}");
        }

        public static async Task AddNewBookTitle(DbService dbs)
        {
            Console.WriteLine("Would you like to add title from existing authors? \n[1] Yes \n[2] No");
            string auChoice = Console.ReadLine();

            Author? author = null;
            if (auChoice == "1")
            {
                author = await ChooseFromExistingAuthor(dbs);
                if (author == null)
                {
                    Console.WriteLine("No Author selected");
                    return;
                }
                Console.WriteLine($"Chosen Author: {author.FirstName} {author.LastName}");
                //Console.WriteLine("End of test //////////////////////////////");

            }
            else if (auChoice == "2")
            {
                Console.WriteLine("Adding method soon");
            }
            else
            {
                Console.WriteLine("Invalid Choice");
            }





        }


        //add new title 
        //add all the information necessary ()
        // ISBN13 - can't be any other existing ones
        // title, language, price, release date, AuhtorID, PublischedID, GenreID


        // - check if author and publisher exist 

    }
}
