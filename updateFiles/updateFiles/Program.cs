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
                iteratePath(ConfigurationManager.AppSettings["oriPath"]);
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

        public static void iteratePath(string path)
        {
            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(path);
            foreach (string fileName in files)
                checkFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subPaths = Directory.GetDirectories(path);
            foreach (string sub in subPaths)
                iteratePath(sub);
        }

        public static void checkFile(string oriFile)
        {
            // Create the path for the destination file
            string destFile = ConfigurationManager.AppSettings["destPath"] +
                oriFile.Replace(ConfigurationManager.AppSettings["oriPath"], "");

            try
            {
                if (File.GetLastWriteTime(oriFile) > File.GetLastWriteTime(destFile))
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
    }
}
