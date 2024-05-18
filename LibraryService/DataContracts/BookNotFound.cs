using System.Runtime.Serialization;

namespace LibraryService.DataContracts
{
    [DataContract]
    public class BookNotFound
    {
        [DataMember]
        public int bookID;
        public BookNotFound(int bookID) { this.bookID = bookID; }
    }
}
