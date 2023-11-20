using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Globalization;


namespace LAB_EXAM
{
    class TextConvert
    {
        public ResultRecord LoadData(List<string> text_list)
        {

            int j = 0;
            List<string> lineList = new List<string>();
            List<string> testName = new List<string>();
            List<TestResult> TRL = new List<TestResult>();
            ResultRecord RR = new ResultRecord
            {
                ResultList = TRL
            };

            lineList.AddRange(text_list);


            string[] header = lineList[0].Replace('\t', ' ').Trim().Split(' ');
            string[] footer = lineList[lineList.Count - 1].Trim().Split(' ');


            lineList.RemoveRange(0, 2);
            lineList.RemoveAt(lineList.Count - 1);

       //===== Appd. time Convert =====//
            string ApproveTime = $"{(footer[2].Remove(footer[2].Length - 2, 2) + (int.Parse(footer[2].Split('/')[2]) + 1957).ToString()).Replace('/', '-')} {footer[3]}";

            if (DateTime.TryParseExact(ApproveTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                Console.WriteLine(parsedDateTime.ToString("yyyy-MM-dd HH:mm"));
            }
       //==============================//

            RR.LabID = header[0].Replace("-",string.Empty);
            RR.HN = header[2];
            RR.FirstName = header[3];
            RR.LastName = header[4];
            RR.TestUnit = $"{ header[5]} {header[6]}";
            RR.TestTime = $"{header[0]} {header[1]}";
            RR.Approved = footer[1];
            RR.ApproveTime = parsedDateTime.ToString("yyyy-MM-dd HH:mm");




            foreach (string line in lineList)
            {

                Match match = Regex.Match(line, @"( +|)(\d.*)");             //--- match all value & unit ---
                Match match2 = Regex.Match(line, @"(\(\w\))( +|)(\d.*)");    //--- match \w flag 

                for (int i = 1; i <= (line.Length); i++)
                {
                    if (line.Substring(i, 3) == "   " ||
                        line.Substring(i, 3) == "(H)" ||
                        line.Substring(i, 3) == "(L)")
                    {
                        testName.Add(line.Substring(2, i - 2));
                        // Console.WriteLine(line.Substring(2, i - 2));
                        break;
                    }
                }

                if (match.Success)
                {

                    //  string g1 = match2.Groups[0].Value;
                    string g2 = match2.Groups[1].Value;  // Flag
                    string[] g3 = match.Groups[2].Value.Replace('\r', ' ').Split(' ');  //  Split from group match ==> [ value , unit ]

                    TRL.Add(new TestResult
                    {
                        TestName = testName[j],
                        Result = g3[0],
                        ReferenceUnits = (g3.Length > 2) ? $"{g3[1]} {g3[2]}" : g3[1],
                        ResultFlag = g2
                    });

                    j++;
                }
            }


            string json = JsonConvert.SerializeObject(RR, Formatting.Indented);
              Console.WriteLine(json);

            return RR;
        }
    }
}
