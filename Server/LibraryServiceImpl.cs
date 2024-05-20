﻿using System;
using System.Collections.Generic;
using System.IO;
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
            books = GetBooks(GetAuthors());
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

        private List<AuthorInfo> GetAuthors()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..");
            filePath = Path.Combine(filePath, "Data", "Authors.txt");
            List <AuthorInfo> authors = new List<AuthorInfo>();
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');

                    if (values.Length == 2)
                    {
                        var author = new AuthorInfo
                        {
                            firstName = values[0].Trim(),
                            lastName = values[1].Trim()
                        };

                        authors.Add(author);
                    }
                }
            }

            return authors;
        }

        private Dictionary<int, BookInfo> GetBooks(List<AuthorInfo> authors)
        {
            var books = new Dictionary<int, BookInfo>();
            int identifier = 0;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..");
            filePath = Path.Combine(filePath, "Data", "Books.txt");
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');
                    if (values.Length >= 1)
                    {
                        List<AuthorInfo> authorInfos = new List<AuthorInfo>();
                        for (int i = 1; i < values.Length; ++i)
                        {
                            try
                            {
                                int id = Convert.ToInt32(values[i].Trim());
                                authorInfos.Add(authors[id]);
                            } catch { }
                        }

                        books.Add(
                            identifier++,
                            new BookInfo
                            { 
                                title = values[0].Trim(),
                                authors = authorInfos.ToArray()
                            }
                        );
                    }
                }
            }
            return books;
        }
    }
}
