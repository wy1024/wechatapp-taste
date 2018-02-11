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
        [Route("orders/{userid}")]
        public async Task<JsonResult<List<Order>>> GetOrdersForUser(string userid)
        {
            var orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT Orders.DateTime, Orders.Details");
                sb.Append("FROM Orders");
                sb.Append("JOIN Users ON Users.Id = Orders.UserId");
                sb.Append($"WHERE Users.UserId = '{userid}';");

                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                DateTime = reader.GetDateTime(0),
                                Details = reader.GetString(1)
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return Json(orders);
        }

        [HttpGet]
        [Route("order/{orderid}")]
        public async Task<JsonResult<Order>> GetOrder(string orderid)
        {
            Order order = null;
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT Orders.DateTime, Orders.Details ");
                sb.Append("FROM Orders ");
                sb.Append("JOIN Users ON Users.Id = Orders.UserId ");
                sb.Append($"WHERE Orders.Id = '{orderid}';");

                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            order = new Order
                            {
                                DateTime = reader.GetDateTime(0),
                                Details = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return Json(order);
        }
    }
}