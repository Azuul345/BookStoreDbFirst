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

        public static async Task ViewAllBookTitles(DbService dbs)
        {
            var titles = await dbs.GetAllBookTitlesInfo();
            Console.WriteLine("All book titles available and information\n");

            foreach (var t in titles)
            {
                Console.WriteLine($"ISBN13: {t.Isbn13}, Author: {t.Author.FirstName} {t.Author.LastName} Publisher: {t.Publisher.PublisherName}, " +
                    $"Genre: {t.Genre.GenreName} Release date: {t.ReleaseDate} Price: {t.Price} Language: {t.Language} Title: {t.Title} ");

            }
            Console.WriteLine($"Total amount of titles: {titles.Count}");
        }

        public static async Task<BookTitle> SelectBookTitle(DbService dbs)
        {
            await ShowAllBookTitles(dbs);
            Console.Write("Select Book Index: ");

            var titlesinfo = await dbs.GetAllBookTitlesInfo();


            BookTitle? selectedTitle = null;
            while (selectedTitle == null)
            {

                if (!int.TryParse(Console.ReadLine(), out int bookID) || bookID < 1 || bookID > titlesinfo.Count)
                {
                    Console.Write("select existing index for Book title: ");
                    continue;
                }

                string selectedIsbn = titlesinfo[bookID - 1].Isbn13;
                selectedTitle = titlesinfo.FirstOrDefault(x => x.Isbn13 == selectedIsbn);
            }
            return selectedTitle;
        }





        public static async Task<Author> SelectAuthor(DbService dbs)
        {
            while (true)
            {

                Console.WriteLine("Would you like to select from existing authors?");
                var allAuthors = await dbs.GetAllAuthors();

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
                }
                else if (auChoice == "2")
                {
                    author = await CreateANewAuthor(dbs);
                    return author;
                }
                else
                {
                    Console.WriteLine("Invalid Choice");
                    continue;
                }
            }
        }







        public static async Task UpdateStockBalanceFromExisting(DbService dbs)
        {
            //store
            await ViewStockBalanceInfo(dbs);
            Console.WriteLine("============ \n");
            Console.Write("Select Store ID to update: ");

            var storeinfo = await dbs.GetAllStoreInfo();

            int? storeID = null;
            while (storeID == null)
            {
                if (!int.TryParse(Console.ReadLine(), out int id) || id < 1 || id > storeinfo.Count)
                {
                    Console.Write("Select correct store ID: ");
                    continue;
                }
                storeID = id;
            }


            var title = await SelectBookTitle(dbs);


            var stockinfo = await dbs.GetStockBalandeInfo();



            Console.Write("Update the new amount of books: ");
            int? bookAmount = null;
            while (bookAmount == null)
            {
                if (!int.TryParse(Console.ReadLine(), out int amount))
                {
                    Console.Write("enter a valid input for amount: ");
                    continue;
                }
                bookAmount = amount;
            }



            var update = stockinfo.FirstOrDefault(sb => sb.StoreId == storeID && sb.Isbn13 == title.Isbn13);


            if (update == null)
            {
                Console.WriteLine("No stock balance entry found for store and book combination ");
                update = await dbs.AddTitleToStock((int)storeID, title, (int)bookAmount);
                Console.WriteLine($"Added a {update.Isbn13Navigation.Title} to {update.Store.StoreName} amount: {update.BookAmounts}");
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

        //UPDATE Title 
        public static async Task UpdateBookTitle(DbService dbs)
        {
            var book = await SelectBookTitle(dbs);
            var author = await SelectAuthor(dbs);
            var publisher = await SelectPublisher(dbs);
            var genre = await SelectGenre(dbs);
            Console.WriteLine("When was the title released? ");
            var date = SelectDate();
            //var isbn13 = await SelectISBN13(dbs); //was not able to change without making adjustments and I'm not sure if this is something that is suppose to be allowed to be changed either. 
            var language = await SelectLanguage(dbs);
            var price = SelectPrice();

            Console.Write("Enter the name of title: ");
            string title = Console.ReadLine();

            book.Author = author;
            book.Publisher = publisher;
            book.Genre = genre;
            book.ReleaseDate = date;
            //book.Isbn13 = isbn13;
            book.Language = language;
            book.Price = price;
            book.Title = title;
            await dbs.UpdateBooktitle(book);
            Console.WriteLine("Title has been updated");
        }

        public static async Task UpdateAuthor(DbService dbs)
        {
            var author = await ChooseFromExistingAuthor(dbs);
            Console.Write("Enter first name: ");
            string firstname = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastname = Console.ReadLine();
            Console.WriteLine("When was the author born");
            var date = SelectDate();

            author.FirstName = firstname;
            author.LastName = lastname;
            author.Birthday = date;

            await dbs.UpdateAuthor(author);
            Console.WriteLine("Author information has been updated");

        }


        //Delete 

        public static async Task DeleteBookTitle(DbService dbs)
        {
            var title = await SelectBookTitle(dbs);
            var deleted = await dbs.DeletBookTitle(title.Isbn13);
            Console.WriteLine(deleted ? "Title deleted" : "Title not found");
        }

        public static async Task DeleteAuhtor(DbService dbs)
        {
            var author = await ChooseFromExistingAuthor(dbs);
            var deleted = await dbs.DeleteAuthor(author.AuthorId);
            Console.WriteLine(deleted ? "Author deleted" : "Author not found");
        }



        // CREATE TITLE and it's properties 

        public static async Task<Author> ChooseFromExistingAuthor(DbService dbs)
        {
            var allAuthors = await dbs.GetAllAuthors();
            while (true)
            {
                for (int i = 0; i < allAuthors.Count; i++)
                {
                    Console.WriteLine($"Author ID: {allAuthors[i].AuthorId}. First Name: {allAuthors[i].FirstName} Last Name: {allAuthors[i].LastName} Birthday: {allAuthors[i].Birthday} ");

                }

                Console.WriteLine("Enter Author ID of Author you wish to select");



                if (!int.TryParse(Console.ReadLine(), out int authorID) || !allAuthors.Any(aid => aid.AuthorId == authorID) || authorID <= 0)
                {
                    Console.WriteLine("Invalid input for Author ID.");
                    continue;
                    //return null;
                }

                var author = allAuthors.FirstOrDefault(p => p.AuthorId == authorID);

                return author;
            }
        }

        public static async Task<Author> CreateANewAuthor(DbService dbs)
        {
            DateOnly? birthday = null;

            Console.Write("Enter Authors first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter Authors last name: ");
            string lastName = Console.ReadLine();

            Console.Write("What is the authors birthday? Enter yyyy/mm/dd: ");
            while (birthday == null)
            {

                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
                {
                    Console.Write("Invalid date. set a , . or / between year, month and year: ");
                    continue;
                }
                birthday = date;
            }

            var author = new Author
            {
                FirstName = firstName,
                LastName = lastName,
                Birthday = birthday,

            };
            await dbs.CreateNewAuthor(author);
            return author;

        }


        public static async Task<Publisher> ChooseFromExistingPublishers(DbService dbs)
        {
            while (true)
            {
                Console.WriteLine("Enter index of publishers for the title");
                var allpublishers = await dbs.GetAllPublishers();

                if (!int.TryParse(Console.ReadLine(), out int publisherId) || !allpublishers.Any(p => p.PublisherId == publisherId) || publisherId <= 0)
                {
                    Console.WriteLine("Invalid input for publisher ID");
                    continue;
                    //return null;
                }
                var publisher = allpublishers.FirstOrDefault(p => p.PublisherId == publisherId);
                return publisher;

            }
        }

        public static async Task<Publisher> CreateANewPublisher(DbService dbs)
        {
            Console.Write("Enter the name of publisher: ");
            string name = Console.ReadLine();

            Console.Write("From what country is the publisher from?: ");
            string country = Console.ReadLine();

            var publisher = new Publisher
            {
                PublisherName = name,
                Country = country
            };
            await dbs.CreateNewPublisher(publisher);
            return publisher;
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

                }
                else if (puChoice == "2")
                {
                    publisher = await CreateANewPublisher(dbs);
                    return publisher;
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

                }
                else if (geChoice == "2")
                {
                    Console.Write("What genre is the title? ");
                    string genretype = Console.ReadLine();
                    genre = new Genre
                    {
                        GenreName = genretype
                    };
                    await dbs.CreateNewGenre(genre);
                    return genre;
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

                Console.WriteLine("Enter date in format: yyyy/mm/dd");
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
            string? isbn13valid = null;
            while (isbn13valid == null)
            {
                Console.WriteLine("Add a new ISBN13 Number. \n(Has to be 13 numbers) not matching any other book titles\nCan not be changed later.");
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

                isbn13valid = strIsbn13;
            }
            return isbn13valid;
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
                    Console.Write("Enter what language the title is in: ");
                    string newLanguage = Console.ReadLine();
                    return newLanguage;
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

                Console.WriteLine("How much does the new title cost?\nif more than two decimals are entered it will either round up or down");

                decimal.TryParse(Console.ReadLine(), CultureInfo.CurrentUICulture, out decimal price);
                if (price == 0)
                {
                    Console.WriteLine("Invalid choice");
                    continue;
                }
                price = Math.Round(price, 2);
                Console.WriteLine($"Price: {price}");
                return price;

            }
        }

        public static async Task AddNewBookTitle(DbService dbs)
        {
            var author = await SelectAuthor(dbs);
            var publisher = await SelectPublisher(dbs);
            var genre = await SelectGenre(dbs);
            Console.WriteLine("What date was the title release in?");
            var date = SelectDate();
            var isbn13 = await SelectISBN13(dbs);
            var language = await SelectLanguage(dbs);
            var price = SelectPrice();

            Console.WriteLine("Enter the name of the new title");
            string titlename = Console.ReadLine();

            var title = new BookTitle
            {
                Author = author,
                Publisher = publisher,
                Genre = genre,
                ReleaseDate = date,
                Isbn13 = isbn13,
                Language = language,
                Price = price,
                Title = titlename

            };

            await dbs.CreateBookTitle(title);
        }




    }
}
