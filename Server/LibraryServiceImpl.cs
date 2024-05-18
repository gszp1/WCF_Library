using System.Collections.Generic;
using LibraryService.DataContracts;
using LibraryService.ServiceContracts;

namespace Server
{
    public class LibraryServiceImpl : ILibraryService
    {

        private readonly Dictionary<int, BookInfo> books;

        public LibraryServiceImpl()
        {
            books = new Dictionary<int, BookInfo>();
            PopulateDictionary(books);
        }

        public int[] FindBooks(string keyword)
        {
            if (keyword == null)
            {
                return new int[0];
            }

            List<int> booksIdentifiers = new List<int>();
            
            foreach (var book in books)
            {
                if (book.Value.title.ToLower().Contains(keyword.ToLower()) == true)
                {
                    booksIdentifiers.Add(book.Key);
                }
            }

            return booksIdentifiers.ToArray();
        }

        public BookInfo GetBookInfo(int bookID)
        {
            if (books.ContainsKey(bookID))
            {
                return books[bookID];
            }
            else
            {
                return null;
            }
        }

        public void PopulateDictionary(Dictionary<int, BookInfo> booksDictionary)
        {

            AuthorInfo[] authors =
            {
                new AuthorInfo{firstName = "name1", lastName = "surname1"},
                new AuthorInfo{firstName = "name2", lastName = "surname2"},
                new AuthorInfo{firstName = "name3", lastName = "surname3"},
                new AuthorInfo{firstName = "name4", lastName = "surname4"},
                new AuthorInfo{firstName = "name5", lastName = "surname5"},
                new AuthorInfo{firstName = "name6", lastName = "surname6"}
            };

            int identifier = 0;
            booksDictionary.Add(identifier++, new BookInfo { title = "Book1", authors = new AuthorInfo[] { authors[0], authors[1] } });
            booksDictionary.Add(identifier++, new BookInfo { title = "Book2", authors = new AuthorInfo[] { authors[2], authors[3] } });
            booksDictionary.Add(identifier++, new BookInfo { title = "Book3", authors = new AuthorInfo[] { authors[4], authors[5] } });
            booksDictionary.Add(identifier++, new BookInfo { title = "Book4", authors = new AuthorInfo[] { authors[3] } });
            booksDictionary.Add(identifier++, new BookInfo { title = "Book5", authors = new AuthorInfo[] { authors[1] } });
        }
    }
}
