using Npgsql;
using NUnit.Framework;
using MTCG.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MTCG.Authentication;

namespace MTCG.Tests
{
    internal class UserControllerTests
    {

        //[Test]
        //public void Test_cmd_parameter_value()
        //{
        //    // Arrange
        //    HttpSvrEventArgs e = new HttpSvrEventArgs();
        //    e.Method = "POST";
        //    e.Payload = "{\"Username\":\"testuser\",\"Password\":\"testpassword\"}";

        //    NpgsqlConnection _Cn = new NpgsqlConnection();
        //    _Cn = new NpgsqlConnection("Server=localhost;Database=MTCG2-DB;Port=5432;User Id=postgres;Password=@Qedipost3@;");
        //    _Cn.Open();

        //    // Act
        //    UserController.AddUser(e, _Cn);

        //    // Assert
        //    NpgsqlCommand cmd = _Cn.CreateCommand();
        //    cmd.CommandText = "SELECT username FROM users WHERE username = 'testuser'";
        //    string username = (string)cmd.ExecuteScalar();
        //    Assert.AreEqual("testuser", username);
        //}

    }
}
