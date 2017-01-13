using System.IO;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    // Interface is used to get ToDo items from third part JAVA servie in Stream view.
    // Returned stream should be closed after work with him.
    public interface IToDoItemFetcher
    {
        Task<Stream> Fetch(); // if Null value is returned Error happened
    }
}
