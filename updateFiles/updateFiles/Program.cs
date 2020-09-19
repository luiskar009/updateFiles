using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace updateFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                iteratePath(ConfigurationManager.AppSettings["oriPath"], ConfigurationManager.AppSettings["destPath"], 0);
                iteratePath(ConfigurationManager.AppSettings["destPath"], ConfigurationManager.AppSettings["oriPath"], -1);

            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter($@"{System.Reflection.Assembly.GetExecutingAssembly().Location}\\{DateTime.Now.ToString("yyyy-MM-dd")}.log", true))
                {
                    writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }

        public static void iteratePath(string oriPath, string destPath, int delete)
        {
            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(oriPath);
            foreach (string fileName in files)
            {
                if (delete == 0)
                    checkFile(fileName, destPath);
                else if (delete == -1)
                    checkDeleteFile(fileName, destPath);
            }


            // Recurse into subdirectories of this directory.
            string[] subPaths = Directory.GetDirectories(oriPath);
            foreach (string sub in subPaths)
                iteratePath(sub, destPath, delete);
        }

        public static void checkFile(string oriFile, string destPath)
        {
            // Create the path for the destination file
            string destFile = destPath + oriFile.Replace(ConfigurationManager.AppSettings["oriPath"], "");

            try
            {
                if (!File.Exists(destFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                    File.Copy(oriFile, destFile, true);
                }

                else if (File.GetLastWriteTime(oriFile) > File.GetLastWriteTime(destFile))
                    File.Copy(oriFile, destFile, true);
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter($@"{System.Reflection.Assembly.GetExecutingAssembly().Location}\\{DateTime.Now.ToString("yyyy-MM-dd")}.log", true))
                {
                    writer.WriteLine("Error: Error trying to overwrite file." + Environment.NewLine + "Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }

        public static void checkDeleteFile(string oriFile, string destPath)
        {
            // Create the path for the destination file
            string destFile = destPath + oriFile.Replace(ConfigurationManager.AppSettings["destPath"], "");

            try
            {
                if (!File.Exists(destFile))
                    File.Delete(oriFile);
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter($@"{System.Reflection.Assembly.GetExecutingAssembly().Location}\\{DateTime.Now.ToString("yyyy-MM-dd")}.log", true))
                {
                    writer.WriteLine("Error: Error trying to delete file." + Environment.NewLine + "Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }
    }
}
