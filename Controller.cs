using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using Dapper;

namespace LAB_EXAM
{

    class DB
    {
        public string cs = @"server=localhost;userid=root;password=;database=lab_exam ; convert zero datetime=True";
        public MySqlConnection conn;

        public MySqlConnection Connect()
        {
            conn = new MySqlConnection(cs);
            return conn;
        }


    }


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
