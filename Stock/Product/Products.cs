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
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = 0;
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=TOFU-LEE;Initial Catalog=Stock;Integrated Security=True");
            //Insert Logic
            con.Open();
            bool status = false;
            if(cmbStatus.SelectedIndex==0)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            var SQLQuery = "";
            if(IfProductsExits(con,txtPC.Text))
            {
                SQLQuery = @"UPDATE [Products] SET [ProductName] = '" + txtPN.Text + "',[ProductStatus] = '" + status + "'WHERE [ProductCode] = '" + txtPC.Text + "'";
            }
            else
            {
                SQLQuery = @"INSERT INTO [dbo].[Products]
                            ([ProductCode],[ProductName],[ProductStatus])
                            VALUES('" + txtPC.Text + "','" + txtPN.Text + "','" + status + "')";
            }
            
            SqlCommand cmd = new SqlCommand(SQLQuery, con);
            cmd.ExecuteNonQuery();
            con.Close();

            //Read Data
            LoadData();
            Cleartxt();
        }

        private bool IfProductsExits(SqlConnection con,string productcode)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select 1 from Products where [ProductCode]='"+productcode+"'", con);
            //select 1 from table和select*from table意思一樣，皆為查看是否有紀錄，但select 1 from table較快
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count > 0)//表示有找到相符的資料
                return true;
            else
                return false;
        }

        public void LoadData()
        {
            SqlConnection con = new SqlConnection("Data Source=TOFU-LEE;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter adapter = new SqlDataAdapter("select*from Products", con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView.Rows.Clear();

            foreach (DataRow item in table.Rows)
            {
                int n = dataGridView.Rows.Add();
                dataGridView.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView.Rows[n].Cells[2].Value = "Deactive";
                }
            }
        }

        private void Cleartxt()
        {
            txtPC.Text = "";
            txtPN.Text = "";
        }

        private void dataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtPC.Text = dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            txtPN.Text = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            //Status顯示方式不同
            if (dataGridView.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                cmbStatus.SelectedIndex = 0;//Active是0，Deactive是1，詳閱Product.cs[設計]
            }
            else
            {
                cmbStatus.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=TOFU-LEE;Initial Catalog=Stock;Integrated Security=True");
            con.Open();
            bool status = false;
            if (cmbStatus.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            var SQLQuery = "";
            if (IfProductsExits(con, txtPC.Text))
            {
                SQLQuery = @"delete from [Products] where [ProductCode] = '" + txtPC.Text + "'and [ProductName] = '" + txtPN.Text + "'and [ProductStatus] = '" + status + "'";
                SqlCommand cmd = new SqlCommand(SQLQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            

            //Read Data
            LoadData();
            Cleartxt();
        }
    }
}
