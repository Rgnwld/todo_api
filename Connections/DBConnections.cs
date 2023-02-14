using System;
using MySql.Data.MySqlClient;
using webapinew.Models;

namespace webapinew.Connections;

public class Connection : IDisposable
{
    public MySqlConnection conn = new MySqlConnection();
    string server = "127.0.0.1";
    string port = "3306";
    string db = "example";
    string uid = "root";
    string pswd = "password";

    public Connection() { }

    public Connection(string _db)
    {
        this.db = _db;
    }

    public Connection(string _server, string _port, string _db, string _uid, string _pswd)
    {
        this.server = _server;
        this.port = _port;
        this.db = _db;
        this.uid = _uid;
        this.pswd = _pswd;
    }


    public MySqlConnection dataSource()
    {
        conn = new MySqlConnection($"server={server}; database={db}; Uid={uid}; password={pswd};  Port={port}; ");
        return conn;
    }

    public MySqlConnection Startup()
    {
        dataSource();
        conn.Open();

        return conn;
    }

    public void Dispose()
    {
        dataSource();
        conn.Close();
    }
}
