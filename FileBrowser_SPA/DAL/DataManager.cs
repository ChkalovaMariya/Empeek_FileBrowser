using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FileBrowser_SPA.Models;
using System.IO;

namespace FileBrowser_SPA.DAL
{
    public class DataManager:IDataManager
    {
        public DataManager()
        {

        }

        public async Task<IEnumerable<DirectoryItem>> Get(string directory)
        {
            List<DirectoryItem> dirItems = new List<DirectoryItem>();

            try
            {
                if (directory.Equals("drives"))
                {
                    string[] drives = System.Environment.GetLogicalDrives();
                    foreach (string d in drives)
                    {
                        dirItems.Add(new DirectoryItem
                        {
                            Name = d.Split(':')[0],
                            FullPath = d,
                            IsFile = false
                        });
                    }
                }
                else
                {
                    CheckTargetDirectory(directory);

                    List<DirectoryItem> files = null;
                    List<DirectoryItem> subDirs = null;

                    DirectoryInfo di = new DirectoryInfo(directory);
                    Task getFiles = Task.Factory.StartNew(() =>
                    {
                        files = di.GetFiles("*.*").Where(x => (x.Attributes & FileAttributes.Hidden) == 0)
                                               .Select(fi => new DirectoryItem
                                               {
                                                   Name = fi.Name,
                                                   FullPath = fi.FullName,
                                                   IsFile = true
                                               })
                                               .ToList();
                    });
                    Task getDirectories = Task.Factory.StartNew(() =>
                    {
                        subDirs = di.GetDirectories().Where(x => (x.Attributes & FileAttributes.Hidden) == 0)
                                                 .Select(dir => new DirectoryItem
                                                 {
                                                     Name = dir.Name,
                                                     FullPath = dir.FullName,
                                                     IsFile = false
                                                 })
                                                 .ToList();
                    });

                    Task.WaitAll(getFiles, getDirectories);
                    dirItems.AddRange(files);
                    dirItems.AddRange(subDirs);
                }
                return dirItems;
            }
            catch (ArgumentException e)
            {

                throw new ArgumentException(e.Message);
            }

            catch (UnauthorizedAccessException e)
            {

                throw new UnauthorizedAccessException(e.Message);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async Task<int[]> GetCount(string directory)
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;

             double size;
             double mb;

                IEnumerable<string> files = FileEnumerator.EnumerateFiles(directory, ".", SearchOption.AllDirectories);

                foreach (string fileName in files)
                {
                    FileInfo fi = new FileInfo(fileName);
                    if ((fi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        size = fi.Length;
                        mb = size / 1000000;
                        if (mb <= 10)
                        {
                            count1++;
                        }
                        else if (mb > 10 & mb <= 50)
                        {
                            count2++;
                        }
                        else if (mb >= 100)
                        {
                            count3++;
                        }
                    }
                }
            
            int[] results = new int[] { count1, count2, count3 };
            return results;
        }
        


        public void CheckTargetDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new ArgumentException("The destination path " + dir + " could not be found");
            }
        }


    }
}