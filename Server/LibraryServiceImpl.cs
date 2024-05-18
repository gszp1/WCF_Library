using System.Collections.Generic;
using LibraryService.DataContracts;
using LibraryService.ServiceContracts;

namespace Server
{
    public class LibraryServiceImpl : ILibraryService
    {

        private Dictionary<int, BookInfo> books;

        public LibraryServiceImpl(Dictionary<int, BookInfo> books)
        {
            this.books = books;
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
    }
}
