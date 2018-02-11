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
    public class OrderController : ApiController
    {
        string DbConnectionString = string.Empty;

        public OrderController()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Constants.DbConnectionString;
            builder.UserID = Constants.DbUsername;
            builder.Password = Constants.DbPassword;
            builder.InitialCatalog = Constants.DbCatalog;
            DbConnectionString = builder.ConnectionString;
        }

        [HttpGet]
        [Route("order/{userid}")]
        public async Task<JsonResult<List<Restaurant>>> GetOrdersForUser(string userid)
        {
            var restaurants = new List<Restaurant>();
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT *");
                sb.Append("FROM dbo.users");
                sb.Append("JOIN dbo.orders ON users.userid = orders.userid");

                sb.Append($"WHERE UserId = '{userid}';");

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
    }
}