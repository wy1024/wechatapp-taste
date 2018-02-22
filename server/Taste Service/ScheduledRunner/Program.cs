using Newtonsoft.Json;
using ScheduledRunner.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Taste_Service.Models;

namespace ScheduledRunner
{
    class Program
    {
        const string Base_URL = "https://apisandbox.dev.clover.com/v3/merchants/{MERCHANT_ID}/items?access_token={Access_Token}";
        const string MERCHANT_ID = "HS4VTV8MXDMDM";
        const string Access_Token = "02c5cac5-06ba-c31b-effd-9286db53d8a0";

        static void Main(string[] args)
        {
            // TODO:


            //string inventoryJson = GetInventoryFromClover(MERCHANT_ID, Access_Token).Result;
            //Console.WriteLine(inventoryJson);
            //UpdateDbInventory(MERCHANT_ID, inventoryJson).Wait();


            // Set preference for every user
            var preferenceSetter = new PreferenceSetter();
            var v = preferenceSetter.GetOrders();
            var result = preferenceSetter.SummarizePreference(v);
            SavePreferenceToDb(result).Wait();
            // set result to sql
        }

        private static async Task SavePreferenceToDb(Dictionary<int, Tuple<List<string>, List<string>, List<int>>> result)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = Constants.DbConnectionString,
                UserID = Constants.DbUsername,
                Password = Constants.DbPassword,
                InitialCatalog = Constants.DbCatalog
            };
            var DbConnectionString = builder.ConnectionString;
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();

                foreach (var res in result)
                {
                    var userid = res.Key;
                    var favFlavors = string.Join(",", new HashSet<string>(res.Value.Item1).ToArray());
                    var favIngredients = string.Join(",", new HashSet<string>(res.Value.Item2).ToArray());
                    var favCuisines = string.Join(",", new HashSet<int>(res.Value.Item3).ToArray());

                    using (SqlCommand command = new SqlCommand($"INSERT INTO dbo.Preference VALUES ({userid}, {favCuisines}, '{favIngredients}', '{favCuisines}')", connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        private static async Task<List<int>> GetUsers()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = Constants.DbConnectionString,
                UserID = Constants.DbUsername,
                Password = Constants.DbPassword,
                InitialCatalog = Constants.DbCatalog
            };
            var DbConnectionString = builder.ConnectionString;
            var users = new List<int>();
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand($"SELECT Id FROM dbo.Users", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            users.Add(id);
                        }
                    }
                }
            }
            return users;
        }

        private static async Task UpdateDbInventory(string merchant_id, string inventoryJson)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = Constants.DbConnectionString,
                UserID = Constants.DbUsername,
                Password = Constants.DbPassword,
                InitialCatalog = Constants.DbCatalog
            };
            var DbConnectionString = builder.ConnectionString;
            int restaurantId = 0;
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand($"SELECT Id FROM dbo.restaurants WHERE CloverId = '{merchant_id}'", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            restaurantId = reader.GetInt32(0);
                        }
                    }
                }

                // Delete dishes for this Restaurant
                var query = $"DELETE FROM dbo.dishes WHERE RestaurantId = {restaurantId}";
                using (SqlCommand deleteCommand = new SqlCommand(query, connection))
                {
                    await deleteCommand.ExecuteNonQueryAsync();
                }

                // Insert dishes to this Restaurant
                var sb = new StringBuilder();
                sb.Append($"INSERT INTO dbo.dishes (RestaurantId, Name, Price) VALUES");

                var inventories = JsonConvert.DeserializeObject<CloverInventoryWrapper>(inventoryJson);
                var lastItem = inventories.elements.Last();
                foreach (var i in inventories.elements)
                {
                    sb.Append($" ({restaurantId}, '{i.name}', {i.price / 100})");
                    if (!i.Equals(lastItem))
                    {
                        sb.Append(", ");
                    }
                }
                using (SqlCommand insertCommand = new SqlCommand(sb.ToString(), connection))
                {
                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
        }

        private static async Task<string> GetInventoryFromClover(string merchant_id, string access_token)
        {
            string url = Base_URL.Replace("{MERCHANT_ID}", merchant_id).Replace("{Access_Token}", access_token);

            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }
    }
}
