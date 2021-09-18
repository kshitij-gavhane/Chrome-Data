using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Windows.Automation;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace chrome_history
{
    public partial class Form1 : Form
    {
        static DataTable dt = new DataTable();
        public Form1()
        {
            
            InitializeComponent();
            timer1.Enabled = true;
            timer1.Interval = 100;
            dt.Columns.Add("URL");
            dt.Columns.Add("Title");
            dt.Columns.Add("Date_Time");
        }
        


        private void button1_Click(object sender, EventArgs e)
        {

               RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion", true);
            reg.SetValue("chrome history", Application.ExecutablePath.ToString());
            //MessageBox.Show("Sucessfully saves.","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
            var results = dataGridView1
               .Rows
               .OfType<DataGridViewRow>()
               .GroupBy(x => new { x.Cells[0].Value })
               .Select(group => new { Item = group.Key, Row = group, Count = group.Count() })
               .ToList();


            for (var index = 0; index < results.Count; index++)
            {
                Console.WriteLine(results[index].Row.FirstOrDefault()?.Cells[0].Value);
                results[index].Row.Skip(1)
                    .ToList()
                    .ForEach(row => dataGridView1.Rows.Remove(row));
            }
            Process[] procschrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procschrome)
            {
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }
                AutomationElement sourceElement = AutomationElement.FromHandle(chrome.MainWindowHandle);
                Condition condition = new OrCondition(new Condition[2]
                {
                    new PropertyCondition(AutomationElement.ControlTypeProperty, (object) ControlType.Edit),
                    new PropertyCondition(AutomationElement.AccessKeyProperty, (object) "Ctrl+L", PropertyConditionFlags.IgnoreCase),
                });
                AutomationElement elmUrlBar = sourceElement.FindFirst(TreeScope.Descendants, condition);
                string url = "", title = "";
                if (elmUrlBar != null)
                {

                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        Console.WriteLine("\n URL found: " + val.Current.Value);
                        url = val.Current.Value;

                    }
                }

                AutomationElement elmTitleBar = sourceElement.FindFirst(TreeScope.Descendants,
             new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TitleBar));
                if (elmTitleBar != null)
                {

                    Console.WriteLine("\n titlebar found" + elmTitleBar.Current.Name);
                    title = elmTitleBar.Current.Name;
                }
                //if (!Items.Rows.Cast<DataRow>().Any(row => row["url"].Equals(rw["url"]) && row["title"].Equals(rw["title"])))
                dt.Rows.Add(url, title);
                LoadGrid();
            }
        }


        
        protected void LoadGrid()
        {
           
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }
        /// <summary>
      
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            button1.PerformClick();
           // this.Visible = false;
            save_database.PerformClick();
        }   

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

            RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion", true);
            reg.SetValue("chrome history", Application.ExecutablePath.ToString());
            //MessageBox.Show("Sucessfully saves.","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
            var results = dataGridView1
               .Rows
               .OfType<DataGridViewRow>()
               .GroupBy(x => new { x.Cells[0].Value })
               .Select(group => new { Item = group.Key, Row = group, Count = group.Count() })
               .ToList();


            for (var index = 0; index < results.Count; index++)
            {
                Console.WriteLine(results[index].Row.FirstOrDefault()?.Cells[0].Value);
                results[index].Row.Skip(1)
                    .ToList()
                    .ForEach(row => dataGridView1.Rows.Remove(row));
            }
            Process[] procschrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procschrome)
            {
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }
                AutomationElement sourceElement = AutomationElement.FromHandle(chrome.MainWindowHandle);
                Condition condition = new OrCondition(new Condition[2]
                {
                    new PropertyCondition(AutomationElement.ControlTypeProperty, (object) ControlType.Edit),
                    new PropertyCondition(AutomationElement.AccessKeyProperty, (object) "Ctrl+L", PropertyConditionFlags.IgnoreCase),
                });
                AutomationElement elmUrlBar = sourceElement.FindFirst(TreeScope.Descendants, condition);
                string url = "", title = "";
                if (elmUrlBar != null)
                {

                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        Console.WriteLine("\n URL found: " + val.Current.Value);
                        url = val.Current.Value;

                    }
                }

                AutomationElement elmTitleBar = sourceElement.FindFirst(TreeScope.Descendants,
             new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TitleBar));
                if (elmTitleBar != null)
                {

                    Console.WriteLine("\n titlebar found" + elmTitleBar.Current.Name);
                    title = elmTitleBar.Current.Name;
                }
                //if (!Items.Rows.Cast<DataRow>().Any(row => row["url"].Equals(rw["url"]) && row["title"].Equals(rw["title"])))
                dt.Rows.Add(url, title);
                LoadGrid();
            }
        }

        private void save_database_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                String constring = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(constring);
                SqlCommand sqlCommand = new SqlCommand("Proc_Insert", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@elmUrlBar", row.Cells[0].Value ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@elmTitleBar", row.Cells[1].Value ?? DBNull.Value);
               
                sqlCommand.Parameters.AddWithValue("@Date_Time", row.Cells[2].Value ?? DateTime.Now);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }

        }

        private void Get_Report_Click(object sender, EventArgs e)
        {
            var f = new Report();
            f.Show();
            this.Visible = false;
        }

        private void UID_Click(object sender, EventArgs e)
        {

        }

        public static void OpenLoginForm()
        {
            Application.Run(new login());
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Process[] proc = Process.GetProcessesByName("chrome");
            if (proc.Length == 0)
            {
                // run whatever here if process is NOT open
                Console.WriteLine("chrome is not running");
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(OpenLoginForm));
                t.Start();
                this.Close();

            }
            else
            {
                // run whatever here if process is open
                Console.WriteLine("chrome is running");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
    }
}
