using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Heysis
{
    public partial class fclothes : Form
    {
        public fclothes()
        {
            InitializeComponent();

            displayCategories();
            displayAllProducts();

        }
        public void displayAllProducts()
        {
            List<AddProductData> listData = AddProductData.AllProductsData();
            dataGridView1.DataSource = listData;
        }

        public void displayCategories()
        {
            if (MainClass.checkConnection())
            {
                MainClass.conn.Open();
                string selectData = "select * from categories";
                using (MySqlCommand cmd = new MySqlCommand(selectData, MainClass.conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cbox_addcat.Items.Add(reader["categoryName"].ToString());
                        }
                    }
                }

            }

        }


        private void btnaddprod_Click(object sender, EventArgs e)
        {
            string con_string = "server=localhost; database=Heysis; user=root; password=root";
            using (MySqlConnection conn = new MySqlConnection(con_string))
            {
                if (EmptyFiled())
                {
                    MessageBox.Show("Empty Field", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        conn.Open();
                        string selectData = "select * from products where prodID = @prodID";
                        using (MySqlCommand cmd = new MySqlCommand(selectData, conn))
                        {
                            cmd.Parameters.AddWithValue("@prodID", txtaddpid.Text.Trim());
                            DataTable dt = new DataTable();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Product ID: " + txtaddpid.Text.Trim() + " is exiting already");
                            }
                            else
                            {
                                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                string relativePath = Path.Combine("Product_Directory", txtaddpid.Text.Trim() + ".jpg");
                                string path = Path.Combine(baseDirectory, relativePath);
                                string directoryPath = Path.GetDirectoryName(path);

                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                                File.Copy(picturebox_addpImg.ImageLocation, path, true);


                                string insertData = "insert into products " +
                                    "(prodID, prodName, prodPrice, prodImage, date_insert, category, stock) " +
                                    "VALUES(@prodID, @prodName, @price, @prodimage, @date, @cat, @stock)";

                                using (MySqlCommand insertD = new MySqlCommand(insertData, conn))
                                {
                                    insertD.Parameters.AddWithValue("@prodID", txtaddpid.Text.Trim());
                                    insertD.Parameters.AddWithValue("@prodName", txtaddpname.Text.Trim());
                                    insertD.Parameters.AddWithValue("@price", txtaddprice.Text.Trim());
                                    insertD.Parameters.AddWithValue("@prodimage", path);
                                    insertD.Parameters.AddWithValue("@cat", cbox_addcat.SelectedItem);
                                    insertD.Parameters.AddWithValue("@stock", txtaddquantity.Text.Trim());

                                    DateTime today = DateTime.Today;
                                    insertD.Parameters.AddWithValue("@date", today);

                                    insertD.ExecuteNonQuery();
                                    clearFilds();

                                    displayAllProducts();
                                    MessageBox.Show("เพิ่มข้อมูลเรียบร้อย", "Infomation Message", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fail connection : " + ex, "Error Message", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }

        }
        public bool EmptyFiled()
        {
            if (txtaddpid.Text == "" || txtaddpname.Text == "" || cbox_addcat.SelectedIndex == -1
                || txtaddprice.Text == "" || txtaddquantity.Text == "" || picturebox_addpImg.Image == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void btnaddpimage_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image File (*.jpg; *.png)|*.jpg;*.png";
                string imagePath = "";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    picturebox_addpImg.ImageLocation = imagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail connection : " + ex, "Error Message", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                MainClass.conn.Close();
            }

        }
        private void btnupdateprod_Click(object sender, EventArgs e)
        {
            string con_string = "server=localhost; database=Heysis; user=root; password=root";
            MySqlConnection conn = new MySqlConnection(con_string);
            if (EmptyFiled())
            {
                MessageBox.Show("Empty Fields", "Error Message", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Update Product ID: "
                    + txtaddpid.Text.Trim() + "?", "Confirmation Message"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        string updateData = "UPDATE products SET prodID = @prodID, prodName = @prodName" +
                            ",category= @cat,prodPrice = @price,stock = @stock WHERE  prodID = @prodID";

                        using (MySqlCommand updateD = new MySqlCommand(updateData, conn))
                        {
                            updateD.Parameters.AddWithValue("@prodID", txtaddpid.Text.Trim());
                            updateD.Parameters.AddWithValue("@prodName", txtaddpname.Text.Trim());
                            updateD.Parameters.AddWithValue("@cat", cbox_addcat.SelectedItem);
                            updateD.Parameters.AddWithValue("@price", txtaddprice.Text.Trim());
                            updateD.Parameters.AddWithValue("@stock", txtaddquantity.Text.Trim());

                            updateD.ExecuteNonQuery();
                            clearFilds();
                            displayAllProducts();
                            MessageBox.Show("อัพเดตข้อมูลเรียบร้อย", "Infomation Message", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Message: " + ex, "Error Message", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void btndeleteprod_Click(object sender, EventArgs e)
        {
            string con_string = "server=localhost; database=Heysis; user=root; password=root";
            MySqlConnection conn = new MySqlConnection(con_string);
            if (EmptyFiled())
            {
                MessageBox.Show("Empty Fields", "Error Message", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Delete Product ID: "
                    + txtaddpid.Text.Trim() + "?", "Confirmation Message"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        string deleteData = "Delete from products where prodID = @prodID";

                        using (MySqlCommand deleteD = new MySqlCommand(deleteData, conn))
                        {

                            deleteD.Parameters.AddWithValue("@prodID", txtaddpid.Text.Trim());

                            deleteD.ExecuteNonQuery();
                            clearFilds();
                            displayAllProducts();
                            MessageBox.Show("ลบข้อมูลเรียบร้อย", "Infomation Message", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Message: " + ex.Message, "Error Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }

        public void clearFilds()
        {
            txtaddpid.Text = "";
            txtaddpname.Text = "";
            txtaddprice.Text = "";
            txtaddquantity.Text = "";
            cbox_addcat.SelectedIndex = -1;
            picturebox_addpImg.Image = null;

        }
        private void btnClearprod_Click(object sender, EventArgs e)
        {
            clearFilds();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtaddpid.Text = row.Cells[0].Value.ToString();
                txtaddpname.Text = row.Cells[1].Value.ToString();
                txtaddprice.Text = row.Cells[2].Value.ToString();
                cbox_addcat.Text = row.Cells[3].Value.ToString();
                string imagepath = row.Cells[4].Value.ToString();
                try
                {
                    if (imagepath != null)
                    {
                        picturebox_addpImg.Image = System.Drawing.Image.FromFile(imagepath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Message: " + ex, "Error Message", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                txtaddquantity.Text = row.Cells[5].Value.ToString();
            }
        }












        private void fclothes_Load(object sender, EventArgs e)
        {

        }
    }
}
