using Asticom_BackendExam.Models;
using Asticom_BackendExam.Models.DbModel;
using Asticom_BackendExam.Models.Request;
using Asticom_BackendExam.Models.Response;
using Asticom_BackendExam.UserManagementService;
using FluentValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asticom_BackendExam_UnitTest
{
    public class Tests : BaseTests
    {
        private string databaseName = string.Empty;
        [SetUp]
        public void Setup()
        {
            databaseName = Guid.NewGuid().ToString();
            //Load Default Data
            //var context = BuildContext(databaseName);
        }

        #region Add User
        [Test]
        public async Task AddUser_Success()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();
            var userRequest = new AddUserRequest()
            {
                FirstName = "TestFirstName",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
            };
            var userRepository = new UserManagementRepository(context, mapper);
            var response = await userRepository.AddUser(userRequest);
            Assert.AreEqual(1, response.Id);
        }

        [Test]
        public void AddUser_Validation_Exception()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();
            var userRequest = new AddUserRequest()
            {
                FirstName = "as",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
            };
            var userRepository = new UserManagementRepository(context, mapper);
            Assert.ThrowsAsync(typeof(ValidationException), ()
              => userRepository.AddUser(userRequest));

        }
        #endregion

        #region Get User
        [Test]
        public async Task GetUser_Success()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //populate entry for us to have reference
            var userInfo = new UserInfo()
            {
                FirstName = "as",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
                Password = "P!2aswkfao",
                Id = 1
            };
            //save data on database
            context.UserInfo.Add(userInfo);
            context.SaveChanges();

            //create new context
            var contextGet = BuildContext(databaseName);
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = await quotationRepository.GetUser(1);
            Assert.IsInstanceOf(typeof(UserResponse), response);
        }
        [Test]
        public async Task GetUser_Not_Existing()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //create new context
            var contextGet = BuildContext(databaseName);
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = await quotationRepository.GetUser(4);
            Assert.AreEqual(null, response);
        }
        #endregion

        #region Delete User
        [Test]
        public async Task DeleteUser_Success()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //populate entry for us to have reference
            var userInfo = new UserInfo()
            {
                FirstName = "as",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
                Password = "P!2aswkfao",
                Id = 1
            };
            //save data on database
            context.UserInfo.Add(userInfo);
            context.SaveChanges();

            //create new context
            var contextGet = BuildContext(databaseName);
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = await quotationRepository.DeleteUser(1);
            Assert.AreEqual("Deleted Successfully", response);
        }
        [Test]
        public async Task DeleteUser_Not_Existing()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //create new context
            var contextGet = BuildContext(databaseName);
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = await quotationRepository.DeleteUser(4);
            Assert.AreEqual(null, response);
        }
        #endregion

        #region Update User
        [Test]
        public async Task UpdateUser_Success()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //populate entry for us to have reference
            var userInfo = new UserInfo()
            {
                FirstName = "testnamr",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
                Password = "P!2aswkfao",
                Id = 1
            };
            //save data on database
            context.UserInfo.Add(userInfo);
            context.SaveChanges();

            //create request
            var userRequest = new AddUserRequest()
            {
                FirstName = "TestFirstName_Updated",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
            };

            var context1 = BuildContext(databaseName);
            var userRepository = new UserManagementRepository(context1, mapper);
            var response = await userRepository.UpdateUser(userRequest, 1);
            Assert.IsInstanceOf(typeof(UserResponse), response);
            //do another checking for the updated field
            Assert.AreEqual("TestFirstName_Updated", response.FirstName);
        }
        [Test]
        public void UpdateUser_NotFound()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //create request
            var userRequest = new AddUserRequest()
            {
                FirstName = "TestFirstName_Updated",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
            };

            var context1 = BuildContext(databaseName);
            var userRepository = new UserManagementRepository(context1, mapper);
            Assert.ThrowsAsync(typeof(KeyNotFoundException), ()
                 => userRepository.UpdateUser(userRequest, 1));
        }
        #endregion

        #region ListUser
        [Test]
        public void ListUsers_HasData()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //populate entry for us to have reference
            var userInfo = new UserInfo()
            {
                FirstName = "as",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
                Password = "P!2aswkfao",
                Id = 1
            };
            //save data on database
            context.UserInfo.Add(userInfo);
            context.SaveChanges();

            //create request
            var request = new PaginationRequest();
            //create new context
            var contextGet = BuildContext(databaseName);
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = quotationRepository.GetAllUsers(request);
            Assert.IsInstanceOf(typeof(UserListResponse), response);
            Assert.AreEqual(1, response.TotalItems);

        }

        [Test]
        public void ListUsers_HasNoData()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //create request
            var request = new PaginationRequest();
            //create new context
            var contextGet = BuildContext(databaseName);
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = quotationRepository.GetAllUsers(request);
            Assert.IsInstanceOf(typeof(UserListResponse), response);
            Assert.AreEqual(0, response.TotalItems);

        }
        #endregion

        #region Delete List Of Users
        [Test]
        public async Task DeleteListOfUsers_Success()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //populate entry for us to have reference
            var userInfo = new UserInfo()
            {
                FirstName = "as",
                Email = "testemail@co.uk",
                LastName = "TestLastName",
                Address = "TestAddress",
                PostCode = "0000",
                PhoneNumber = "09101111111",
                UserName = "testemail@tesing.com",
                Password = "P!2aswkfao",
                Id = 1
            };
            //save data on database
            context.UserInfo.Add(userInfo);
            context.SaveChanges();

            //create new context
            var contextGet = BuildContext(databaseName);
            var listOfUsers = new List<int>() { 1, 2, 3, 4 };
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = await quotationRepository.DeleteListOfUsers(listOfUsers);
            Assert.AreEqual(1, response.Count); //has 1 id successfully deleted
        }
        [Test]
        public async Task DeleteListOfUsers_NothingDeleted()
        {
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            //create new context
            var contextGet = BuildContext(databaseName);
            var listOfUsers = new List<int>() { 1, 2, 3, 4 };
            var quotationRepository = new UserManagementRepository(contextGet, mapper);
            var response = await quotationRepository.DeleteListOfUsers(listOfUsers);
            Assert.AreEqual(null, response); //has 1 id successfully deleted
        }
        #endregion
    }
}