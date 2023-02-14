namespace webapinew.Models;

public class InternalUser
{
    public int? id { get; set; }
    public string name { get; set; } = "";
    public string password { get; set; } = "";
    public string? role { get; set; } = "";
}

public class PublicUser
{
    public int id { get; set; }
    public string name { get; set; } = "";
    public string role { get; set; } = "";
}



