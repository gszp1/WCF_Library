using System.Runtime.Serialization;

namespace LibraryService.DataContracts
{
    [DataContract(Name = "BookInfo", Namespace = "")]
    public class BookInfo
    {
        [DataMember]
        public string title;

        [DataMember]
        public AuthorInfo[] authors;
    }
}
