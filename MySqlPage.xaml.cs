using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XF_SqlServer
{
    public partial class MySqlPage : ContentPage
    {
        MySqlConnection connection;
        public MySqlPage()
        {
            InitializeComponent();
            // Local Server
            string serverdbname = "mydb";
            string servername = "127.0.0.1";
            string serverusername = "student";
            string serverpassword = "student";
            string serverport = "3306";          

             string sqlConn = $"Server={servername};Port={serverport};database={serverdbname};User Id={serverusername};Password={serverpassword};charset=utf8";
           // string sqlConn = "Server=****;Database=****;Uid=****;Pwd=****";

            connection = new MySqlConnection(sqlConn);
        }
        private async void TestConnection_Clicked(object sender, System.EventArgs e)
        {
            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                    await DisplayAlert("Yeah", "Connection established", "OK");

                }
                catch (MySqlException ex)
                {
                    await DisplayAlert("Naaah", "Connection failed: " + ex.Message, "Quit");
                }
                finally
                {

                    connection.Close();
                    await DisplayAlert("Finally", "Connection closed", "OK");
                }
            }
        }
        private async void GetInfo_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<MyTable> myList = new List<MyTable>();

                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM mytable";

                var reader = command.ExecuteReader();
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
                connection.Close();
                MyListView.ItemsSource = myList;

            }catch (Exception ex)
            {
                await DisplayAlert("Error Connection", ex.Message, "Ok");              
            }
        }
        private async void InsertInfo_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserId.Text) && !string.IsNullOrEmpty(UserTitle.Text)
                    && !string.IsNullOrEmpty(UserBody.Text))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("INSERT INTO mytable (Id, Title, Body) VALUES('" + UserId.Text + "','" + UserTitle.Text + "','" + UserBody.Text + "')", connection);
                    var reader = command.ExecuteReaderAsync();
                    connection.Close();
                    await DisplayAlert("Insert", "Insert Data", "Ok");
                }
                else
                    await DisplayAlert("Info", "Empty", "Ok");
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
                    connection.Open();
                    int UId = Convert.ToInt32(UserId.Text);
                    string UTitle = UserTitle.Text;
                    string UBody = UserBody.Text;

                    string queryS = $"UPDATE mytable SET Id='{UId}',Title='{UTitle}',Body='{UBody}' WHERE Id='{UId}'";
                    MySqlCommand command = new MySqlCommand(queryS, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
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
        private async  void DeleteInfo_Clicked(object sender, EventArgs e)
    {
            try
            {
                if (!string.IsNullOrEmpty(UserId.Text))
                {
                    connection.Open();
                    int UId = Convert.ToInt32(UserId.Text);

                    MySqlCommand command = new MySqlCommand($"DELETE FROM mytable WHERE Id='{UId}'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();

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
            UserId.Text = ""; UserTitle.Text = ""; UserBody.Text = "";
        }
        public class MyTable
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }
}
