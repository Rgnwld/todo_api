
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using webapinew.Models;
using webapinew.Connections;
using webapinew.Services;


namespace webapinew.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController
    {
        Connection dbconn = new Connection("todo_project");

        [HttpPost(Name = "GetTeste")]
        public async Task<IResult> ClaimToken([FromBody] InternalUser user)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = dbconn.Startup();

            cmd.CommandText = $"SELECT id, name, role FROM users WHERE name='{user.name}' AND password='{user.password}'";
            var rdr = await cmd.ExecuteReaderAsync();
            PublicUser map_data = new PublicUser();

            try
            {
                while (rdr.Read())
                {
                    map_data = (new PublicUser
                    {
                        id = rdr.GetInt32(0),
                        name = rdr.GetString(1),
                        role = rdr.GetString(2),
                    });
                }
            }
            catch (Exception)
            {
                return Results.Problem("Something went wrong");
            }
            finally
            {
                dbconn.Dispose();
            }

            if (map_data == null) return Results.NotFound("Not found");

            string token = TokenService.GenerateUserToken(map_data);

            return Results.Ok(token);
        }
    }
}