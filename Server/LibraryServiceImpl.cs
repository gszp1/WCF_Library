using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using LibraryService.DataContracts;
using LibraryService.ServiceContracts;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
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
            if (keyword == null || keyword.Equals("") )
            {
                return new int[0];
            }

            return books
                .Where(book => book.Value.title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(book => book.Key)
                .ToArray();
        }

        public BookInfo GetBookInfo(int bookID)
        {
            if (books.ContainsKey(bookID))
            {
                return books[bookID];
            }
            else
            {
                throw new FaultException<BookNotFound>(
                    new BookNotFound(bookID),
                    new FaultReason("No book with given identifier")
                );
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
            booksDictionary.Add(
                identifier++,
                new BookInfo {
                    title = "Book1",
                    authors = new AuthorInfo[] { authors[0], authors[1] } 
                }
            );
            booksDictionary.Add(
                identifier++,
                new BookInfo {
                    title = "Book2",
                    authors = new AuthorInfo[] { authors[2], authors[3] }
                }
            );
            booksDictionary.Add(
                identifier++,
                new BookInfo {
                    title = "Book3",
                    authors = new AuthorInfo[] { authors[4], authors[5] }
                }
            );
            booksDictionary.Add(
                identifier++,
                new BookInfo {
                    title = "Book4",
                    authors = new AuthorInfo[] { authors[3] }
                }
            );
            booksDictionary.Add(
                identifier++,
                new BookInfo {
                    title = "Book5",
                    authors = new AuthorInfo[] { authors[1] } 
                }
            );
        }
    }
}
