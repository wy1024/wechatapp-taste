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
    public class MenuController : ApiController
    {
        string DbConnectionString = string.Empty;

        public MenuController()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Constants.DbConnectionString;
            builder.UserID = Constants.DbUsername;
            builder.Password = Constants.DbPassword;
            builder.InitialCatalog = Constants.DbCatalog;
            DbConnectionString = builder.ConnectionString;
        }


        [HttpGet]
        [Route("menu/{restaurantId}")]
        public async Task<JsonResult<List<DishesGroup>>> GetMenuForRestaurant(string restaurantId)
        {
            List<DishesGroup> result = null;
            var dishes = new List<Dish>();
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                await connection.OpenAsync();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT *");
                sb.Append($"FROM Dishes WHERE RestaurantId = {restaurantId};");

                using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var dish = new Dish
                            {
                                Name = reader.GetString(3),
                                Flavor = reader.GetString(4),
                                Ingredients = reader.GetString(5),
                                Category = reader.GetString(6),
                                Price = reader.GetFloat(7),
                                Image = reader.GetStream(8)
                            };
                            dishes.Add(dish);
                        }
                    }
                }
            }

            // Put dishes into categories
            var createdCategories = new List<string>();
            foreach(var currentDish in dishes)
            {
                var category = currentDish.Category;
                DishesGroup group = new DishesGroup();
                if (createdCategories.Contains(category))
                {
                    group = result.Where(dg => dg.Category.Equals(category)).First();
                    var dishesInGroup = group.Dishes;
                    dishesInGroup.Add(currentDish);
                } 
                else
                {
                    result.Add(group);
                    group.Dishes = new List<Dish>() { currentDish };
                }

            }
            return Json(result);
        }

        //[HttpGet]
        //[Route("preference/{userid}")]
        //public string GetPreferenceForUser(string userid)
        //{
        //    string result = string.Empty;
        //    using (SqlConnection connection = new SqlConnection(DbConnectionString))
        //    {
        //        connection.Open();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("SELECT TOP 1 *");
        //        sb.Append("FROM dbo.preference;");
        //        String sql = sb.ToString();

        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    result = reader.GetString(0);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
    }
}