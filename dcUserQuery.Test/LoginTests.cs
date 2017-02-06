using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dcUserQuery;
using System.DirectoryServices;

namespace dcUserQuery.Test
{
    [TestClass]
    public class LoginTests
    {
        string _testDomain, _testUserName, _testPassword, _serverAddress;


        //DirectoryEntry entry = new DirectoryEntry("LDAP://raven", "mikey", "HotSoop1");

        [TestMethod]
        public void Login_InvalidUser_IsNotSuccessful()
        {
            //_testPassword = "DogC0w1!";
            GetEnvironmentSettings("testing");

            _testUserName = "notAUser";
            _testPassword = "badPassword01!";

            //_testUserName = "userNormal";
            //_testPassword = "p@ssW0rd";

            DcUserQuery userQuery = new DcUserQuery();
            string response = userQuery.LoginUser(_serverAddress, _testDomain, _testUserName, _testPassword);
            Assert.AreNotEqual("OK", response);
        }

        private void GetEnvironmentSettings(string environment)
        {
            switch (environment)
            {
                case "prod":
                    _serverAddress = "LDAP://34.198.174.9";
                    _testDomain = "TST";

                    break;
                case "test":
                default:
                    _serverAddress = "LDAP://raven";
                    _testDomain = "quhar";
                    break;
            }
        }

        [TestMethod]
        public void Login_InvalidPassword_IsNotSuccessful()
        {
            GetEnvironmentSettings("testing");
            //_testUserName = "mfarquhar";
            //_testPassword = "DogC0w1!";

            _testUserName = "userNormal";
            _testPassword = "badPassword01!";

            DcUserQuery userQuery = new DcUserQuery();
            string response = userQuery.LoginUser(_serverAddress, _testDomain, _testUserName, _testPassword);
            Assert.AreNotEqual("OK", response);
        }


        [TestMethod]
        public void Login_ValidUser_IsSuccessful()
        {
            GetEnvironmentSettings("test");
            //_testUserName = "mfarquhar";
            //_testPassword = "DogC0w1!";

            _testUserName = "userNormal";
            _testPassword = "p@ssW0rd";

            DcUserQuery  userQuery = new DcUserQuery();
            string response = userQuery.LoginUser(_serverAddress, _testDomain, _testUserName, _testPassword);
            Assert.AreEqual("OK", response);
        }

        [TestMethod]
        public void AdminUser_LookupValidUserGroup_IsSuccessful()
        {
            GetEnvironmentSettings("test");
            _testUserName = "administrator";
            _testPassword = "DogC0w1!";

            DcUserQuery userQuery = new DcUserQuery();
            //string results = GetDepartment_v2("mfarquhar");
            var depts = userQuery.GetUserDepartments(_serverAddress, _testDomain, _testUserName, _testPassword); 
        }

        [TestMethod]
        public void Login_ValidUser_ReturnsGroups()
        {
            //_serverAddress = "LDAP://raven";
            _serverAddress = "34.198.174.9";
            _testDomain = "quhar";
            _testUserName = "userNormal";
            _testPassword = "p@ssW0rd";

            DcUserQuery userQuery = new DcUserQuery();
            string response = userQuery.GetUserGroups(_serverAddress, _testDomain, _testUserName, _testPassword);
            Assert.IsTrue(0 == response.Length);
            ////string response = userQuery.LoginUser(_serverAddress, _testDomain, _testUserName, _testPassword);
            //Assert.AreEqual("OK", response);
        }

        [TestMethod]
        public void Login_DisabledUser_IsNotSuccessful()
        {
            _testDomain = "quhar";
            _testUserName = "mfarquhar";
            //testPassword = "DogC0w1!";
            _testPassword = "badPassword01!";
        }

        [TestMethod]
        public void GetADDirectoryEntry_WithAdminCredentials_IsNotNull()
        {
            DcUserQuery dcQuery = new DcUserQuery();
            var result = dcQuery.GetDirectoryObject();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetADUserTest()
        {
            DcUserQuery dcQuery = new DcUserQuery();
            var result = dcQuery.GetUser("mikey");
        }
    }


}
