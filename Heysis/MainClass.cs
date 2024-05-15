using MySql.Data.MySqlClient;
using System.Data;

namespace Heysis
{
    internal class MainClass
    {
        public static readonly string con_string = "server=localhost; database=Heysis; user=root; password=root";
        public static MySqlConnection conn = new MySqlConnection(con_string);

        //สร้างตาราง user

        //method to check user 
        public static bool IsValidUser(string user, string pass)
        {
            bool isValid = false;
            string qry = @"Select * from users where uname = '" + user + "' and upass = '" + pass + "' ";
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                isValid = true;
                USER = dt.Rows[0]["username"].ToString();
            }

            return isValid;
        }



        //สร้างmethod 
        public static string user;

        public static string USER
        {
            get { return user; }
            private set { user = value; }

        }
        //สร้างmethod เพื่อเช้คการเชื่อมต่อกับฐานข้อมูล
        public static bool checkConnection()
        {
            if (conn.State != ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }





    }
}
