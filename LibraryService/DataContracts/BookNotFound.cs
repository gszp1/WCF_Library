using System.Runtime.Serialization;

namespace LibraryService.DataContracts
{
    [DataContract(Name = "BookNotFound", Namespace = "")]
    public class BookNotFound
    {
        [DataMember]
        public int bookID;
        public BookNotFound(int bookID) { this.bookID = bookID; }
    }
}
