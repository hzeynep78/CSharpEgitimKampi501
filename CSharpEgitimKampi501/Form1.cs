using CSharpEgitimKampi501.Dtos;
using Dapper;
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

namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection("Data Source=HILALZ\\SQLEXPRESS;Initial Catalog=EgitimKampi501DB; Integrated Security=True;");

        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "Select * From TBL_PRODUCT";
            var values = await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;
;        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "Insert Into TBL_PRODUCT (ProductName,ProductStock,ProductPrice,ProductCategory) values (@ProductName,@ProductStock,@ProductPrice,@ProductCategory)";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", txtName.Text);
            parameters.Add("@ProductStock", txtStock.Text);
            parameters.Add("@ProductPrice", txtPrice.Text);
            parameters.Add("@ProductCategory", txtCategory.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Ekleme Başarılı");
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "Delete From TBL_PRODUCT Where ProductId = @ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", txtID.Text);
            await connection.ExecuteAsync(query,parameters);
            MessageBox.Show("Silme Başarılı");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "Update TBL_PRODUCT Set ProductName=@ProductName,ProductStock=@ProductStock,ProductPrice=@ProductPrice,ProductCategory=@ProductCategory Where ProductId = @ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", txtName.Text);
            parameters.Add("@ProductStock", txtStock.Text);
            parameters.Add("@ProductPrice", txtPrice.Text);
            parameters.Add("@ProductCategory", txtCategory.Text);
            parameters.Add("@ProductId", txtID.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Güncelleme Başarılı");
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string query1 = "Select Count(*) From TBL_PRODUCT";
            var productcount = await connection.QueryFirstOrDefaultAsync<int>(query1);
            lblProductCount.Text = productcount.ToString();

            string query2 = "Select ProductName From TBL_PRODUCT Where ProductPrice=(Select Max(ProductPrice) From TBL_PRODUCT)";
            var MaxProduct = await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblMaxProduct.Text = MaxProduct.ToString();

            string query3 = "Select Count(Distinct(ProductCategory))From TBL_PRODUCT";
            var CategoryCount = await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblCategoryCount.Text= CategoryCount.ToString();
        }
    }
}
