using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webapinew.Models;
using webapinew.Connections;
using Microsoft.AspNetCore.Authorization;


namespace webapinew.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    Connection dbconn = new Connection("todo_project");

    [Authorize(Roles = "admin")]
    [HttpGet(Name = "GetAllUsers")]
    public IResult GetAll()
    {
        var cmd = new MySqlCommand();
        cmd.Connection = dbconn.Startup();

        cmd.CommandText = "SELECT * FROM users";
        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<PublicUser> map_data = new List<PublicUser>();
        try
        {
            while (rdr.Read())
            {
                map_data.Add(new PublicUser
                {
                    id = rdr.GetInt32(0),
                    name = rdr.GetString(1),
                    role = rdr.GetString(2),
                });
            }
        }
        catch (Exception)
        {
            Results.Problem("Something went wrong");
        }
        finally
        {
            dbconn.Dispose();
        }

        return Results.Ok(map_data);
    }

    // [Authorize(Roles = "admin")]
    [HttpPost(Name = "SetUser")]
    public IResult PostUser(string name, string password, string role)
    {
        var cmd = new MySqlCommand();
        cmd.Connection = dbconn.Startup();

        Random rnd = new Random();
        int id = rnd.Next();

        try
        {
            cmd.CommandText = $"INSERT INTO users (id, name, password, role) VALUES ({id}, '{name}', '{password}', '{role}')";
            cmd.ExecuteNonQuery();
            return Results.CreatedAtRoute(value: new PublicUser { id = id, name = name, role = role });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("Something went wrong");
        }
        finally
        {
            dbconn.Dispose();
        }
    }

    [HttpGet("{id}", Name = "GetUser")]
    public async Task<IResult> GetUser(int id)
    {
        var cmd = new MySqlCommand();
        cmd.Connection = dbconn.Startup();

        cmd.CommandText = $"SELECT * FROM users WHERE id={id}";
        try
        {
            var rdr = await cmd.ExecuteReaderAsync();
            InternalUser? map_data = null;
            while (rdr.Read())
            {
                map_data = (new InternalUser
                {
                    id = rdr.GetInt32(0),
                    name = rdr.GetString(1),
                    password = rdr.GetString(2),
                    role = rdr.GetString(3),
                });
            }

            if (map_data == null) return Results.NotFound("Not found");

            return Results.Ok(map_data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("Not found"); ;
        }
        finally
        {
            dbconn.Dispose();
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}", Name = "UpdateUser")]
    public IResult UpdateUser(string id, string name, string type)
    {
        var cmd = new MySqlCommand();
        cmd.Connection = dbconn.Startup();

        Random rnd = new Random();

        try
        {
            cmd.CommandText = $"UPDATE users SET name='{name}', type='{type}' WHERE id='{id}'";
            cmd.ExecuteNonQuery();
            return Results.Ok("User Updated!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("Something went wrong");
        }
        finally
        {
            dbconn.Dispose();
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}", Name = "DeleteUser")]
    public IResult DeleteUser(int id)
    {
        var cmd = new MySqlCommand();
        cmd.Connection = dbconn.Startup();

        Random rnd = new Random();

        try
        {
            cmd.CommandText = $"DELETE FROM users WHERE id={id};";
            cmd.ExecuteNonQuery();
            return Results.Ok("User Deleted!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("Something went wrong");
        }
        finally
        {
            dbconn.Dispose();
        }
    }
}


