using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Taste_Service.Models;

namespace Taste_Service.Controllers
{
    [RoutePrefix("api")]
    public class RestaurantController : ApiController
    {
        string DbConnectionString = string.Empty;

        public RestaurantController()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Constants.DbConnectionString;
            builder.UserID = Constants.DbUsername;
            builder.Password = Constants.DbPassword;
            builder.InitialCatalog = Constants.DbCatalog;
            DbConnectionString = builder.ConnectionString;
        }

        [HttpGet]
        [Route("restaurants/all")]
        public async Task<JsonResult<List<Restaurant>>> GetAllRestaurants()
        {
            var restaurants = new List<Restaurant>();
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT *");
                sb.Append("FROM dbo.restaurants;");

                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var restaurant = new Restaurant
                            {
                                Name = reader.GetString(1),
                                Location = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Owner = reader.GetString(4),
                                //Image = reader.GetStream(5)
                            };
                            restaurants.Add(restaurant);
                        }
                    }
                }
            }
            return Json(restaurants);
        }

        [HttpGet]
        [Route("restaurants/{id}")]
        public async Task<JsonResult<Restaurant>> GetRestaurant(string id)
        {
            Restaurant restaurant = null;
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT *");
                sb.Append("FROM dbo.restaurants;");
                sb.Append($"WHERE Id = {id};");

                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            restaurant = new Restaurant
                            {
                                Name = reader.GetString(1),
                                Location = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Owner = reader.GetString(4),
                                //Image = reader.GetStream(5)
                            };
                        }
                    }
                }
            }
            return Json(restaurant);
        }
    }
}