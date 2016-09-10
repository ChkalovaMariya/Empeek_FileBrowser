using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileBrowser_SPA.Models;
using System.Threading.Tasks;

namespace FileBrowser_SPA.DAL
{
    public interface IDataManager
    {
        Task<IEnumerable<DirectoryItem>> Get(string directory);
        Task<int> GetCount(string directory, int min, int max);
        bool DirExists(string directory);
    }
}