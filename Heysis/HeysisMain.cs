using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Heysis
{
    public partial class HeysisMain : Form
    {
        public HeysisMain()
        {
            InitializeComponent();

        }
        //เปลี่ยนข้อความตรงlabelด้วยUsernamer ที่loginเข้ามา
        private void HeysisMain_Load(object sender, EventArgs e)
        {
            lblUser.Text = MainClass.USER;
        }

        //Method ta Controls in MainForm(HeysisMain)
        public void AddControls(Form f)
        {
            panelCenter.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            panelCenter.Controls.Add(f);
            f.Show();
        }

        private void btnCloth_Click(object sender, EventArgs e)
        {
            AddControls(new fclothes());
        }
    
        private void btnlogout_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("ออกจากระบบแน่นะ ?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AddControls(new Introduction());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void lblUser_Click_1(object sender, EventArgs e)
        {

        }

        private void panelCenter_Paint(object sender, PaintEventArgs e)
        {

        }

      
    }
}
