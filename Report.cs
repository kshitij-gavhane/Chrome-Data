using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chrome_history
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            
                
            btn_report.PerformClick();
          
            this.reportViewer1.RefreshReport();
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            String constring = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(constring);
            SqlCommand sqlCommand = new SqlCommand("Proc_Report", sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter d = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            d.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.ReportPath = @"F:\CODE\college internship\chrome History with db\chrome history\Report1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}
