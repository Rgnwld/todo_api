using System;

namespace webapinew.Models;
public class TodoTable
{
    public string? id { get; set; } = "";
    public string title { get; set; } = "";
    public string? description { get; set; } = "";
    public List<TodoList>? lists { get; set; } = new List<TodoList>();
}

public class TodoList
{
    public string id { get; set; } = "";
    public string? title { get; set; } = "";
    public string description { get; set; } = "";
    public string finished { get; set; } = "";
    public List<TodoItem>? items { get; set; } = new List<TodoItem>();

}
public class TodoItem
{
    public string id { get; set; } = "";
    public string title { get; set; } = "";
    public bool finished { get; set; } = false;
}
