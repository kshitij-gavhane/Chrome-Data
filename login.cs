using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chrome_history
{
    public partial class login : Form
    {
       
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mis_authoService.MISAuthoSoapClient mislogin = new mis_authoService.MISAuthoSoapClient();

            var m = mislogin.MISLogin(textBox1.Text, textBox2.Text, "MIS");
            if (m.status == "success")
            {
                var f = new Form1();
                f.Show();
                Process.Start("chrome.exe");
                this.Visible = false;
                String constring = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(constring);
                SqlCommand sqlCommand = new SqlCommand("Proc_Insert", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@UID", textBox1.Text);

                sqlCommand.ExecuteNonQuery();


                sqlConnection.Close();

            }
            else
            {
                MessageBox.Show("Invalid UserID Password", "Alert", MessageBoxButtons.OK);
            }
            
        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        

        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {
            if (checkBox_Show_Hide.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
