using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heysis
{
    internal class AddProductData
    {
        public static readonly string con_string = "server=localhost; database=Heysis; user=root; password=root";

        public int ProdID { set; get; } //0
        public string ProdName { set; get; } //1
        public string ProdPrice { set; get; } //2
        public string ProdCategory { set; get; } //3
        public string ProdImg { set; get; } // 4
        public string ProdStock { set; get; } //5
        public string Date { set; get; } //6

        public static List<AddProductData> AllProductsData()
        {
            List<AddProductData> listData = new List<AddProductData>();
            MySqlConnection conn = new MySqlConnection(con_string); 

            try
            {
                conn.Open();
                string selectData = "SELECT * FROM products";
                using (MySqlCommand cmd = new MySqlCommand(selectData, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AddProductData data = new AddProductData();
                            data.ProdID = (int)reader["prodID"];
                            data.ProdName = reader["prodName"].ToString();
                            data.ProdPrice = reader["prodPrice"].ToString();
                            data.ProdCategory = reader["category"].ToString();
                            data.ProdImg = reader["prodImage"].ToString();
                            data.ProdStock = reader["stock"].ToString();
                            data.Date = reader["date_insert"].ToString();

                            listData.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return listData;
        }
    }
}
