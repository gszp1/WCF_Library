using System.Runtime.Serialization;

namespace LibraryService.DataContracts
{
    [DataContract]
    public class BookInfo
    {
        [DataMember]
        public string title;

        [DataMember]
        public AuthorInfo[] authors;
    }
}
