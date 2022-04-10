using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace XF_SqlServer
{
    public partial class MainPage : ContentPage
    {
        SqlConnection sqlConnection;
        public MainPage()
        {
            InitializeComponent();
            // Local Server
           //string serverdbname = "mydb";
            //string servername = "122.0.0.1";
            //string serverusername = "student";
            //string serverpassword = "student";
            // Online Server
            string serverdbname = "db_a6410c_mydb";
            string servername = "SQL5104.site4now.net";
            string serverusername = "db_a6410c_mydb_admin";
            string serverpassword = "Ss@123456";

            string sqlConn = $"Data Source={servername};Initial Catalog={serverdbname};User Id={serverusername};Password={serverpassword}";

           // string sqlConn = $"Data Source=SQL5104.site4now.net;Initial Catalog=db_a6410c_mydb;User Id=db_a6410c_mydb_admin;Password=Ss@123456";

            sqlConnection = new SqlConnection(sqlConn);
        }
        private async void TestConnection_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                await DisplayAlert("Connection", "Connection Established", "Ok"); 
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error Connection", ex.Message, "Ok");
                throw;
            }
        }
        private async void GetInfo_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<MyTable> myList = new List<MyTable>();
                sqlConnection.Open();
                string queryString = "SELECT * FROM dbo.mytable";
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();                  
                while (reader.Read())
                {
                    myList.Add(new MyTable
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString(),
                        Body = reader["Body"].ToString(),
                    });
                }
                reader.Close();
                sqlConnection.Close();
                MyListView.ItemsSource = myList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error Connection", ex.Message, "Ok");
                throw;
            }
        }
        private async void InsertInfo_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserId.Text) && !string.IsNullOrEmpty(UserTitle.Text)
                    && !string.IsNullOrEmpty(UserBody.Text))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO dbo.mytable VALUES(@Id, @Title, @Body)", sqlConnection))
                    {
                        command.Parameters.Add(new SqlParameter("Id", UserId.Text));
                        command.Parameters.Add(new SqlParameter("Title", UserTitle.Text));
                        command.Parameters.Add(new SqlParameter("Body", UserBody.Text));
                        command.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                    await DisplayAlert("Insert", "Insert Data", "Ok");
                }
                else
                    await DisplayAlert("Null", "Empty" , "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");                
            }
        }
        private async void UpdateInfo_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserId.Text) && !string.IsNullOrEmpty(UserTitle.Text)
                    && !string.IsNullOrEmpty(UserBody.Text))
                {
                    sqlConnection.Open();
                    int UId = Convert.ToInt32(UserId.Text);
                    string UTitle = UserTitle.Text;
                    string UBody = UserBody.Text;

                    string queryS = $"UPDATE dbo.mytable SET Id='{UId}',Title='{UTitle}',Body='{UBody}' WHERE Id='{UId}'";
                    using(SqlCommand command = new SqlCommand(queryS, sqlConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                    await DisplayAlert("Update", "Updated", "Ok");
                }
                else
                    await DisplayAlert("Null", "Empty", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
                throw;
            }
        }
        private async void DeleteInfo_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserId.Text))
                {
                    sqlConnection.Open();
                    int UId = Convert.ToInt32(UserId.Text);
                
                    using (SqlCommand command = new SqlCommand($"DELETE FROM dbo.mytable WHERE Id='{UId}'", sqlConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    sqlConnection.Close();

                    await DisplayAlert("Delete", "Deleted", "Ok");
                }
                else
                    await DisplayAlert("Null", "Empty", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
                throw;
            }
        }           
        private void Clear_Clicked(object sender, EventArgs e)
        {
            UserId.Text = ""; UserTitle.Text = "";UserBody.Text = "";
        }
        public class MyTable
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }       
    }
}
