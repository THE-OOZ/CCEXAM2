using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using Dapper;
using System.Drawing.Text;
//using System.Windows.Forms.DataVisualization.Charting;
using LiveCharts;
using LiveCharts.Wpf;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace LAB_EXAM
{
 
    public partial class MAIN_FORM : Form
    {
        public List<ToLabResult> ChartData;
        public bool ChartState =  false;
        public MAIN_FORM()
        {
            InitializeComponent();

            PrivateFontCollection modernFont = new PrivateFontCollection();

            modernFont.AddFontFile("Kanit-Medium.ttf");
           //BtnExit.Font = new Font(modernFont.Families[0], 8);
           //BtnImport.Font = new Font(modernFont.Families[0], 8);
         

            label1.Font =  new Font(modernFont.Families[0], 14);
           label2.Font = new Font(modernFont.Families[0], 12);
           label3.Font = new Font(modernFont.Families[0], 18);
           LabelName.Font = new Font(modernFont.Families[0], 18);

           checkBox1.Font = new Font(modernFont.Families[0], 12);
           checkBox2.Font = new Font(modernFont.Families[0], 12);
           checkBox1.Enabled = false;
           checkBox2.Enabled = false;



        }

        

        public void BtnImport_Click(object sender, EventArgs e)
        {
        
            string currentPath =  Directory.GetCurrentDirectory();
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                
                InitialDirectory = $"{currentPath.Remove(currentPath.Length-9,9)}/src",
                Title = "Browse Text Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
               // FilterIndex = 2,
                RestoreDirectory = true,
                ShowReadOnly = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // MessageBox.Show(openFileDialog1.FileName);
                string filePath = openFileDialog1.FileName;

                List<ResultRecord> RRL = new List<ResultRecord>();

                ReadFile ReadFile = new ReadFile();

                TextConvert TextConvert = new TextConvert();
                
                List<List<string>> txtfullList = ReadFile.ReaderTxt(filePath);
                

                foreach (List<string> list in txtfullList)
                {
                   RRL.Add(TextConvert.LoadData(list));
                }
            


               
            
                string message = $"Do you want to Insert { RRL.Count.ToString()} Record";
                string title = "Insert Confirmation";


                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.No)
                {
                   Close();
                }
                else
                { // Execute : insert record 

                    MessageBox.Show("Complte !!","", MessageBoxButtons.OK,MessageBoxIcon.Information);

                    MySqlConnection Connection = new DB().Connect();

                    var SqlPatient = @"INSERT INTO `tbl_patient` " +
                                    @"(`HN`, `FirstName`, `LastName`) VALUES  (@HN, @FirstName, @LastName)";

                    var SqlLabResult = @"INSERT INTO `tbl_lab_result` " +
                                      @"(`LabID`,`HN`, `TestName`, `Result`, `ResultFlag`, `ReferenceUnits`, " +
                                      @"`TestTime`, `ApproveTime`, `Approved`, `TestUnit`) " +
                                      @"VALUES (@LabID, @HN, @TestName, @Result, @ResultFlag, @ReferenceUnits, @TestTime, @ApproveTime, @Approved, @TestUnit)";


                    foreach (var RR in RRL)
                    {
                        ToPatient toPatient = new ToPatient()
                        {
                            HN = RR.HN,
                            FirstName = RR.FirstName,
                            LastName = RR.LastName
                        };

                        if(Connection.Execute(SqlPatient, toPatient) == 1)
                        {
                            break;
                        }
                    }
                    int _id=0;
                    foreach (var RR in RRL)
                    {
                        _id++;
                        foreach (var RL in RR.ResultList)
                        {
                            
                            ToLabResult toLabResult = new ToLabResult()
                            {
                                LabID = $"{RR.LabID}-{_id.ToString()}",
                                HN = RR.HN,
                                TestName = RL.TestName,
                                Result = RL.Result,
                                ResultFlag = RL.ResultFlag,
                                ReferenceUnits = RL.ReferenceUnits,
                                TestTime = RR.TestTime,
                                ApproveTime = RR.ApproveTime,
                                Approved = RR.Approved,
                                TestUnit = RR.TestUnit
                            };

                             Connection.Execute(SqlLabResult, toLabResult);

                            //MessageBox.Show(RR.ApproveTime +"| "+ RR.TestTime);
                   
                        }


                    }
                             

                            

                }
            }


        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void BtnSeacrh_Click(object sender, EventArgs e)
        {
            
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;

            try
            {
                using (var Connection = new DB().Connect())
                {
                    var SqlByHN = @"SELECT * FROM `tbl_lab_result` WHERE HN = @HN ORDER BY `TestTime` DESC";
                    var reader = Connection.ExecuteReader(SqlByHN, new { HN = TextBoxHn.Text });
                    DataTable table = new DataTable();

                    table.Load(reader);

                    dataGridView1.DataSource = table;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }



            using (var Connection = new DB().Connect())
            {

                var SqlByTestName = @"SELECT `TestName` , COUNT(*) as 'TestNameCount' FROM `tbl_lab_result` WHERE HN = @HN GROUP BY TestName;";

                comboBox1.Items.Clear();   // Clear Item  : Prevent recursion 

                var ComboboxData = Connection.Query<ToComboBox>(SqlByTestName, new { HN = TextBoxHn.Text }).ToList();



                foreach (ToComboBox elem in ComboboxData)
                {
                    comboBox1.Items.Add($"{elem.TestName} [ {elem.TestNameCount} รายการ]");
                }

            }

            try
            {
                using (var Connection = new DB().Connect())
                {
                    var SqlName = @"SELECT `FirstName`,`LastName` FROM `tbl_patient`  WHERE HN = @HN;";
                    if (TextBoxHn.Text != "")
                    {
                        var Fullname = Connection.QuerySingle<ToPatient>(SqlName, new { HN = TextBoxHn.Text });
                        LabelName.Text = $"{Fullname.FirstName}  {Fullname.LastName}";
                    }
                    else
                    {
                        MessageBox.Show("กรุณานำเข้าข้อมูล", "Human Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
            }
            catch (Exception ex)
            {
                string ERR_MSG = ex.Message == "Sequence contains no elements" ? "ไม่พบข้อมูล": "ข้อมูลผิดพลาด";

                MessageBox.Show(ERR_MSG, "Human Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
  



            //   Connection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            var SqlSelectTestName = @"SELECT * FROM `tbl_lab_result` WHERE TestName = @TestName AND HN = @HN ORDER BY TestTime DESC";

            string SelectItem = comboBox1.SelectedItem.ToString().Split('[')[0];

            using (var Connection = new DB().Connect())
            {
            var reader = Connection.ExecuteReader(SqlSelectTestName, new { TestName = SelectItem ,HN = TextBoxHn.Text});

                DataTable table = new DataTable();

                table.Load(reader);

                dataGridView1.DataSource = table;

            }

            using (var Connection = new DB().Connect())
            {

                var _ChartData = Connection.Query<ToLabResult>(SqlSelectTestName, new { TestName = SelectItem, HN = TextBoxHn.Text }).ToList();

                ChartData = _ChartData;
            }

            if (ChartState)
            {
                BarChart(ChartData);
                checkBox1.Checked = true;
            }
            else
            {
                LineChart(ChartData);
                checkBox2.Checked = true;
            }
           

        }

        private void LabelName_Click(object sender, EventArgs e)
        {

        }



        public void LineChart(List<ToLabResult> CHDT)
        {
            if (CHDT == null) {
                MessageBox.Show("กรุณานำเข้าข้อมูล", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBox2.Checked = false;
            }
            else
            {
                cartesianChart1.Series.Clear();
                cartesianChart1.AxisX.Clear();

                IList<string> lbx = new List<string>();

                cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Result",
                    Values = new ChartValues<double> {}
                }
            };

                foreach (var data in CHDT)
                {
                    cartesianChart1.Series[0].Values.Add(Double.Parse(data.Result));
                    lbx.Add(data.TestTime);
                }

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "DateTime",
                    Labels = lbx,

                });

            }

        }

        public void BarChart(List<ToLabResult> CHDT)
        {
            if (CHDT == null) {
                MessageBox.Show("กรุณานำเข้าข้อมูล", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBox1.Checked = false;
            }
            else
            {
                cartesianChart1.Series.Clear();
                cartesianChart1.AxisX.Clear();

                IList<string> lbx = new List<string>();

                cartesianChart1.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Result",
                    Values = new ChartValues<double>{ }
                }
            };

                foreach (var data in CHDT)
                {
                    cartesianChart1.Series[0].Values.Add(Double.Parse(data.Result));
                    lbx.Add(data.TestTime);
                }

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "DateTime",
                    Labels = lbx,

                });
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
               BarChart(ChartData);
                checkBox2.Checked = false;
                ChartState = true;

            }
            else
            {
                checkBox1.Checked = false;
                LineChart(ChartData);
                ChartState = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                LineChart(ChartData);
                checkBox1.Checked = false;
                ChartState = false;
            }
            else
            {
                checkBox2.Checked = false;
                BarChart(ChartData);
                ChartState = true;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void TextBoxHn_TextChanged(object sender, EventArgs e)
        {

            if ((new Regex(@"([0-9]{7})")).IsMatch(TextBoxHn.Text))
            {
                TextBoxHn.ForeColor = Color.Black;
                TextBoxHn.BackColor = Color.Aquamarine;
            }
            else
            {
                TextBoxHn.BackColor = Color.Salmon;
            }
        }

       

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = Color.LightBlue;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            var datarow = ChartData.ElementAt(e.RowIndex);
            MessageBox.Show(JsonConvert.SerializeObject( datarow, Formatting.Indented));

            //List<string> DataSelected = new List<string>();


            //foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            //{
            //    for (int i = 0; i < (row.Cells.Count) - 1; i++)
            //    {
            //        DataSelected.Add(row.Cells[i].Value.ToString());
            //    }

            //      MessageBox.Show(string.Join(", ", DataSelected));



            //if (dataGridView1.SelectedRows.Count > 0)
            //{

            //    for (int i = 0; i < (dataGridView1.SelectedRows[(e.RowIndex)].Cells.Count)-1; i++)
            //    {
            //        DataSelected.Add(dataGridView1.SelectedRows[e.RowIndex].Cells[i].Value.ToString());
            //    }
            //}

            //    }

            //MessageBox.Show(string.Join(", ", DataSelected));
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
         
         
        }

        private void BtnForm2_Click(object sender, EventArgs e)
        {
            Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            f2 = null;         
            Show();
                      
        }
    }
}
