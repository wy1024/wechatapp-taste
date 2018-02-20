using Newtonsoft.Json;
using ScheduledRunner.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taste_Service.Models;

namespace ScheduledRunner
{
    public class PreferenceSetter
    {
        private string DbConnectionString;

        public PreferenceSetter()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Constants.DbConnectionString;
            builder.UserID = Constants.DbUsername;
            builder.Password = Constants.DbPassword;
            builder.InitialCatalog = Constants.DbCatalog;
            DbConnectionString = builder.ConnectionString;
        }

        public Dictionary<int, List<Order>> GetOrders()
        {
            var userOrders = new Dictionary<int, List<Order>>();
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                String sql = "SELECT Orders.Datetime, Orders.Details, Users.Id FROM dbo.Orders JOIN dbo.Users ON Users.Id = Orders.UserId;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                DateTime = reader.GetDateTime(0),
                                Details = reader.GetString(1)
                            };
                            var userid = reader.GetInt32(2);
                            if (userOrders.ContainsKey(userid))
                            {
                                var orders = userOrders[userid];
                                orders.Add(order);
                            }
                            else
                            {
                                userOrders.Add(userid, new List<Order>() { order });
                            }
                        }
                    }
                }
            }

            return userOrders;
        }

        public Dictionary<int, Tuple<List<string>, List<string>, List<int>>> SummarizePreference(Dictionary<int, List<Order>> userOrders)
        {
            // fetch all dish info
            // id -> dish
            Dictionary<int, Dish> dishIdToDetails = new Dictionary<int, Dish>();
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * from dbo.Dishes", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dishId = reader.GetInt32(0);
                            var dish = new Dish
                            {
                                Cuisine = reader.GetInt32(2),
                                Name = reader.GetString(3),
                                Description = reader.GetString(4),
                                Flavors = reader.GetString(5),
                                Ingredients = reader.GetString(6),
                                Category = reader.GetString(7),
                                Price = reader.GetDouble(8)
                            };
                            dishIdToDetails.Add(dishId, dish);
                        }
                    }
                }
            }

            var userPreferences = new Dictionary<int, Tuple<List<string>, List<string>, List<int>>>();
            foreach(var userOrder in userOrders)
            {
                var user = userOrder.Key;
                var pastOrders = userOrder.Value;
                var favFlavors = new List<string>();
                var favIngredients = new List<string>();
                var favCuisines = new List<int>();

                foreach (var order in pastOrders)
                {
                    var details = order.Details;
                    var orderedDishes = JsonConvert.DeserializeObject<DishOrders>(details);
                    foreach(var dishOrder in orderedDishes.Details)
                    {
                        var did = dishOrder.DishId;
                        var dishDetails = dishIdToDetails[did];
                        favFlavors.Add(dishDetails.Flavors);
                        favIngredients.Add(dishDetails.Ingredients);
                        favCuisines.Add(dishDetails.Cuisine);
                    }
                }

                userPreferences.Add(user, new Tuple<List<string>, List<string>, List<int>>(favFlavors, favIngredients, favCuisines));
            }

            return userPreferences;
        }
    }
}
