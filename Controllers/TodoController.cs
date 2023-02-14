using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using webapinew.Models;
using webapinew.Connections;
using webapinew.Services;

namespace webapinew.Controllers;



[ApiController]
[Route("api/[controller]")]
public class TodoController
{
    Connection dbconn = new Connection("todo_project");

    [Authorize(Roles = "admin")]
    [HttpGet(Name = "GetToDos")]
    public async Task<IResult> GetTodos([FromHeader] string Authorization)
    {
        var cmd = new MySqlCommand();
        cmd.Connection = dbconn.Startup();
        Random rnd = new Random();

        try
        {
            PublicUser authUser = TokenService.DecodeUserToken(Authorization);
            cmd.CommandText = $"Select id, title, description from todotables WHERE userId='{authUser.id}'";
            Console.WriteLine(cmd.CommandText);
            var rdr = await cmd.ExecuteReaderAsync();
            List<TodoTable> data = new List<TodoTable>();

            while (rdr.Read())
            {
                data.Add(new TodoTable
                {
                    id = rdr.GetString(0),
                    title = rdr.GetString(1),
                    description = rdr.GetString(2),
                });
            }

            return Results.Ok(data);
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
    [HttpPost(Name = "CreateToDo")]
    public IResult CreateTodo([FromHeader] string Authorization, [FromBody] TodoTable table)
    {
        var cmd = new MySqlCommand();
        cmd.Connection = dbconn.Startup();

        Random rnd = new Random();
        int id = rnd.Next();

        try
        {
            PublicUser authUser = TokenService.DecodeUserToken(Authorization);
            if (table.description != null)
                cmd.CommandText = $"INSERT INTO todotables (id, userId, title, description) VALUES ({id}, '{authUser.id}', '{table.title}', '{table.description}')";
            var rdr = cmd.ExecuteNonQuery();

            return Results.Ok("Table Created");
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
