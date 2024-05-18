using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryService.DataContracts;
using LibraryService.ServiceContracts;

namespace Server
{
    public class LibraryServiceImpl : ILibraryService
    {
        public int[] FindBooks(string keyword)
        {
            List<int> booksIdentifiers = new List<int>();

            return booksIdentifiers.ToArray();
        }

        public BookInfo GetBookInfo(int bookID)
        {
            BookInfo bookInfo = new BookInfo();
            return bookInfo;
        }
    }
}
