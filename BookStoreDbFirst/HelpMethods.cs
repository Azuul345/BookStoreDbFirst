using BookStoreDbFirst.Models;
using System.Globalization;

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
                Console.WriteLine($"Store: {s.Store.StoreName} || Store ID: {s.StoreId} || Book Amount: {s.BookAmounts} ISBN13: {s.Isbn13} || Title: {s.Isbn13Navigation.Title}");
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
                Console.WriteLine($"Index: {i + 1} ISBN13: {titles[i].Isbn13} Title: {titles[i].Title}");
            }
        }



        ///  VG 




        public static async Task<Author> ChooseFromExistingAuthor(DbService dbs)
        {
            while (true)
            {

                Console.WriteLine("Enter index of Author you wish to add title to");
                var allAuthors = await dbs.GetAllAuthors();

                //for (int i = 0; i < allAuthors.Count; i++)
                //{
                //    Console.WriteLine($"Author ID: {allAuthors[i].AuthorId}. First Name: {allAuthors[i].FirstName} Last Name: {allAuthors[i].LastName} Birthday: {allAuthors[i].Birthday} ");

                //}
                if (!int.TryParse(Console.ReadLine(), out int authorID) || !allAuthors.Any(aid => aid.AuthorId == authorID) || authorID <= 0)
                {
                    Console.WriteLine("Invalid input for Author ID.");
                    continue;
                    //return null;
                }
                //var author = allAuthors[authorID - 1] not needed! 
                var author = allAuthors.FirstOrDefault(p => p.AuthorId == authorID);

                //Console.WriteLine($"{author.FirstName} {author.LastName}");
                return author;
                break;
            }
        }

        //maybe change from condition checking count to see that id matches any of existing ones instead
        public static async Task<Publisher> ChooseFromExistingPublishers(DbService dbs)
        {
            while (true)
            {

                Console.WriteLine("Enter index of publishers for the title");
                var allpublishers = await dbs.GetAllPublishers();

                //for (int i = 0; i < allpublishers.Count; i++)
                //{
                //    Console.WriteLine($"Publisher ID:{allpublishers[i].PublisherId} Publisher: {allpublishers[i].PublisherName} Country: {allpublishers[i].Country}");
                //}

                if (!int.TryParse(Console.ReadLine(), out int publisherId) || !allpublishers.Any(p => p.PublisherId == publisherId) || publisherId <= 0)
                {
                    Console.WriteLine("Invalid input for publisher ID");
                    continue;
                    //return null;
                }

                //var publisher = allpublishers[publisherId - 1]; not needed!
                var publisher = allpublishers.FirstOrDefault(p => p.PublisherId == publisherId);
                //Console.WriteLine($"Selected publisher: {publisher.PublisherName}");
                return publisher;
                break;
            }
        }



        public static async Task<Author> SelectAuthor(DbService dbs)
        {
            while (true)
            {

                Console.WriteLine("Would you like to add title from existing authors?");
                var allAuthors = await dbs.GetAllAuthors();

                for (int i = 0; i < allAuthors.Count; i++)
                {
                    Console.WriteLine($"Author ID: {allAuthors[i].AuthorId}. First Name: {allAuthors[i].FirstName} Last Name: {allAuthors[i].LastName} Birthday: {allAuthors[i].Birthday} ");

                }

                Console.WriteLine(" \n[1] Yes \n[2] No");
                string auChoice = Console.ReadLine();
                Author? author = null;

                if (auChoice == "1")
                {
                    author = await ChooseFromExistingAuthor(dbs);
                    if (author == null) //maybe not needed
                    {
                        Console.WriteLine("No Author selected");

                    }
                    Console.WriteLine($"Chosen Author: {author.FirstName} {author.LastName}");
                    return author;
                    break;
                    //Console.WriteLine("End of test //////////////////////////////");
                }
                else if (auChoice == "2")
                {
                    Console.WriteLine("Adding method soon");
                    return null;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Choice");
                    continue;

                }
            }
        }


        public static async Task<Publisher> SelectPublisher(DbService dbs)
        {
            while (true)
            {
                Console.WriteLine("Would like to add the publisher from database?");
                var allpublishers = await dbs.GetAllPublishers();

                for (int i = 0; i < allpublishers.Count; i++)
                {
                    Console.WriteLine($"Publisher ID:{allpublishers[i].PublisherId} Publisher: {allpublishers[i].PublisherName} Country: {allpublishers[i].Country}");
                }

                Console.WriteLine("\n[1] Yes\n[2] No");
                string puChoice = Console.ReadLine();
                Publisher? publisher = null;
                if (puChoice == "1")
                {
                    publisher = await ChooseFromExistingPublishers(dbs);
                    if (publisher == null) //maybe not needed
                    {
                        Console.WriteLine("No publisher selected");
                    }
                    Console.WriteLine($"Chosen Publisher: {publisher.PublisherName}");
                    return publisher;
                    break;
                }
                else if (puChoice == "2")
                {
                    Console.WriteLine("Adding method soon");
                }
                else
                {
                    Console.WriteLine("invalid choice");
                    continue;
                }
            }
        }

        public static async Task<Genre> SelectGenre(DbService dbs)
        {
            while (true)
            {
                Console.WriteLine("Would you like to add form existing genre?");
                var allGenres = await dbs.GellAllGenre();
                foreach (var g in allGenres)
                {
                    Console.WriteLine($"ID: {g.GenreId} Genre: {g.GenreName}");
                }
                Console.WriteLine("\n[1] Yes\n[2] Add another Genre");
                string geChoice = Console.ReadLine();
                Genre? genre = null;

                if (geChoice == "1")
                {
                    Console.WriteLine("Select the Genre ID of title: ");
                    if (!int.TryParse(Console.ReadLine(), out var genreID) || !allGenres.Any(g => g.GenreId == genreID) || genreID <= 0)
                    {
                        Console.WriteLine("Invalid input for genre");
                        continue;
                    }
                    genre = allGenres.FirstOrDefault(g => g.GenreId == genreID);
                    Console.WriteLine($"Genre: {genre.GenreName}");
                    return genre;
                    break;
                }
                else if (geChoice == "2")
                {
                    Console.WriteLine("adding method");
                }
                else
                {
                    Console.WriteLine("Invalid choice");
                    continue;
                }


            }
        }

        public static DateOnly SelectDate()
        {
            while (true)
            {

                Console.WriteLine("What date was the title released? yyyy/mm/dd");
                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
                {
                    Console.WriteLine("Invalid Date. \nhas to be yyyymmdd with either of / , .  Between year, month and day.");
                    continue;
                }
                Console.WriteLine(date);
                return date;
            }
        }

        public static async Task<string> SelectISBN13(DbService dbs)
        {
            while (true)
            {
                Console.WriteLine("Add a new ISBN13 Number. \n(Has to be 13 numbers) not matching any other book titles");
                string strIsbn13 = Console.ReadLine();
                var titles = await dbs.GetAllBookTitlesInfo();


                if (strIsbn13.Length != 13 || !long.TryParse(strIsbn13, out long isbn13))
                {
                    Console.WriteLine("ISBN13 Has to be 13 numbers");
                    continue;
                }
                bool isbn13exist = titles.Any(i => i.Isbn13 == strIsbn13);
                if (isbn13exist)
                {
                    Console.WriteLine("ISBN13 number already exist for another title");
                    continue;
                }
                return strIsbn13;
                break;
            }
        }

        public static async Task<string> SelectLanguage(DbService dbs)
        {
            while (true)
            {
                Console.WriteLine("What Language is the title published in?");
                var titles = await dbs.GetAllBookTitlesInfo();

                var langagues = titles.Select(t => t.Language).Distinct().ToList();

                for (int i = 0; i < langagues.Count; i++)
                {
                    Console.WriteLine($"Index: {i + 1} language: {langagues[i]}");
                }
                Console.WriteLine("[1]: Chose existing language?\n[2]: Enter a new language");

                string? selectedLanguae = null;
                string langChoice = Console.ReadLine();
                if (langChoice == "1")
                {
                    Console.Write("Enter index of language: ");
                    if (!int.TryParse(Console.ReadLine(), out int langageindex) || langageindex <= 0 || langageindex > langagues.Count)
                    {
                        Console.WriteLine("Invalid choice for language");
                        continue;
                    }
                    selectedLanguae = langagues[langageindex - 1];
                    Console.WriteLine($"Chosen language: {selectedLanguae} ");
                    return selectedLanguae;
                }
                else if (langChoice == "2")
                {
                    Console.WriteLine("Add method to add language");
                }
                else
                {
                    Console.WriteLine("Invalid choice");
                    continue;
                }
            }
        }

        public static Decimal SelectPrice()
        {
            ////Price 
            while (true)
            {

                Console.WriteLine("How much does the new title cost?");

                decimal.TryParse(Console.ReadLine(), CultureInfo.CurrentUICulture, out decimal price);
                if (price == 0)
                {
                    Console.WriteLine("Invalid choice");
                    continue;
                }
                Console.WriteLine($"Price: {price}");
                return price;

            }
        }

        public static async Task AddNewBookTitle(DbService dbs)
        {

            //var author = await SelectAuthor(dbs);
            ////Console.WriteLine($"Test// {author.FirstName} {author.LastName}");

            //var publisher = await SelectPublisher(dbs);
            //Console.WriteLine($"Publisher Name {publisher.PublisherName}");

            //var genre = await SelectGenre(dbs);
            //Console.WriteLine($"//Test {genre.GenreName}");

            //var date = SelectDate();
            //Console.WriteLine($"//Test Selected date: {date}");

            //var isbn13 = await SelectISBN13(dbs);
            //Console.WriteLine(isbn13);

            //var language = await SelectLanguage(dbs);
            //Console.WriteLine($"//Test Selected lan: {language}");

            var price = SelectPrice();


            //Console.WriteLine("Enter the name of the new title");
            //string titlename = Console.ReadLine();

        }

        //  price left and title for new book.

        //add new title 
        //add all the information necessary ()
        // ISBN13 - can't be any other existing ones
        // title, language, price, release date, AuhtorID, PublischedID, GenreID


        // - check if author and publisher exist 

    }
}
