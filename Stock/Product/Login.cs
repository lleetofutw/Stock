using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Product
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUN.Clear();
            txtPW.Clear();
            txtUN.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=TOFU-LEE;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT*FROM[dbo].[Login]where UserName = '"+txtUN.Text+"' and PassWord = '"+txtPW.Text+"'",con);
            DataTable tab = new DataTable();
            adapter.Fill(tab);
            if(tab.Rows.Count==1)//這裡的'1'代表True，表示帳密輸入正確
            {
                this.Hide();//隱藏此Form，不用Close()是因為，Close()會直接關閉，而無法執行下面的程式，若要用Close()，則此行要放最後一行
                StockMain main = new StockMain();
                main.Show();
            }
            else
            {
                MessageBox.Show("Error UserName or Password","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnClear_Click(sender,e);//使用btnClear的方法，清除txtUN和txtPW
            }
        }
    }
}
