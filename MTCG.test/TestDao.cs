using MTCG.Data_Layer;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.test
{
    [TestFixture]
    internal class TestDao
    {
        User player1;
        UserDao userDao;

        [SetUp]
        public void Setup()
        {
            player1 = new User("name1", "password1");
            userDao = new UserDao();
        }

        [Test, Order(1)]
        public void TestUserAlreadyExists()
        {
            Assert.That(userDao.InsertUser(player1), Is.True);
            Assert.That(userDao.GetUserByName(player1.UserCredentials.Username), Is.True);

            //Register with same User should fail
            Assert.That(userDao.InsertUser(player1), Is.False);

        }

        [Test, Order(2)]
        public void TestUserLogin()
        {
            string username = player1.UserCredentials.Username;
            Assert.That(userDao.LoginUser(player1.UserCredentials), Is.Not.Null);
            Assert.That(userDao.LoginUser(player1.UserCredentials).Equals(username+"-mtcgToken"));
        }

        [Test, Order(3)]
        public void TestTokenExists()
        {
            //Assert.That(userDao.TokenExists());
        }

        [Test, Order(4)]
        public void TestUserDelete()
        {
            Assert.That(userDao.DeleteUser(player1.UserCredentials.Username), Is.True);
        }

        [Test]
        public void TestColumnsExists()
        {
            string GetColumn = @"SELECT column_name FROM information_schema.columns WHERE table_name = 'User'";
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(GetColumn, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
            }

            Assert.That(reader, Is.Not.Null);
        }

        [Test]
        public void TestTablesExist()
        {
            string getTablesQuery = @"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE'";
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(getTablesQuery, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
            }

            Assert.That(reader, Is.Not.Null);
        }
    }
}
