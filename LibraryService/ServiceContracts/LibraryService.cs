using System.ServiceModel;
using LibraryService.DataContracts;

namespace LibraryService.ServiceContracts
{
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        int[] FindBooks(string keyword);

        [OperationContract]
        [FaultContract(typeof(BookNotFound))]
        BookInfo GetBookInfo(int bookID);
    }
}
