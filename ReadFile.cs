using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LAB_EXAM
{
    //class AllResultList
    //{
    //    public List<string> TxtResultList { set; get; }
    //    public List<AllResultList> FullList { set; get; }
    //}
    class ReadFile
    {
      
        public List<List<string>> ReaderTxt(string file_path)
        {
            // Reads file line by line
            StreamReader file = new StreamReader(file_path);

            List<string> ResultList = new List<string>();
            List<List<string>> FullList = new List<List<string>>();

            string ln;


            while ((ln = file.ReadLine()) != null)
            {

                if (ln.Trim().Split(' ')[0] != "Approved:")
                {
                    ResultList.Add(ln);
                }
                else
                {
                    ResultList.Add(ln);
                    FullList.Add(new List<string>(ResultList));
                    ResultList.Clear();
                }
            }

            file.Close();

            return FullList;

        }
    }
}
