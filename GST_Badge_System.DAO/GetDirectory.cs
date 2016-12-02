using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GST_Badge_System.DAO
{
    public class GetDirectory
    {
        public static string getFilePath()
        {
            string dirpath = Directory.GetCurrentDirectory();
            string path = Directory.GetParent(dirpath).FullName;
            path = Directory.GetParent(path).FullName;
            string directorypath = Directory.GetParent(path).FullName;

            return directorypath;
        }
    }
}
