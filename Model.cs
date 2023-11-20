using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_EXAM
{
    public class TestResult
    {
        public string TestName { get; set; }
        public string Result { get; set; }
        public string ReferenceUnits { get; set; }
        public string ResultFlag { get; set; }
    }
     
    public class ResultRecord
    {
        public string LabID { get; set; }
        public string HN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TestUnit { get; set; }
        public string TestTime { get; set; }
        public string Approved { get; set; }
        public string ApproveTime { get; set; }
        public List<TestResult> ResultList { get; set; }
    }

    public class ToLabResult
    {

        public string LabID { get; set; }
        public string HN { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string ResultFlag { get; set; }
        public string ReferenceUnits { get; set; }
        public string TestTime { get; set; }
        public string ApproveTime { get; set; }
        public string Approved { get; set; }
        public string TestUnit { get; set; }
    }


    public class ToPatient
    {
        public string HN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ToComboBox
    {
        public string TestName { get; set; }
        public int TestNameCount { get; set; }
    }

}
