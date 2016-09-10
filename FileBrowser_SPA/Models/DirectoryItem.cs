using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowser_SPA.Models
{
    public class DirectoryItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsFile { get; set; }
    }
}