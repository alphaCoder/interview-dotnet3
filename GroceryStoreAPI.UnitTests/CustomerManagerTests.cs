using GroceryStoreAPI.Domain.Customers;
using GroceryStoreModels;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryStoreAPI.UnitTests
{
    [TestFixture]
    public class CustomerManagerTests
    {
        private ICustomerRepository _mockCustomerRepository;

        [SetUp]
        public void Setup()
        {
            _mockCustomerRepository = Substitute.For<ICustomerRepository>();
        }

        [Test]
        public void Given_A_Empty_Customer_When_Add_Should_Return_Bad_Request_Result()
        {
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Add(string.Empty);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.BadRequest, result.ErrorMessage);
        }

        [Test]
        public void Given_A_Duplicate_Customer_When_Add_Should_Return_Item_Already_Exists_Result()
        {
            _mockCustomerRepository.Add(Arg.Any<string>()).Returns(-1);
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Add("TestCustomer");
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.ItemAlreadyExists, result.ErrorMessage);
        }

        [Test]
        public void Given_A_Valid_Customer_When_Add_Should_Return_Correct_Result()
        {
            _mockCustomerRepository.Add(Arg.Any<string>()).Returns(1);
            _mockCustomerRepository.Get(Arg.Any<int>()).Returns(new Customer { Id = 1, Name = "TestCustomer" });
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Add("TestCustomer");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("TestCustomer", result.Data.Name);
        }

        [Test]
        public void Given_A_Customer_When_Add_Should_Return_Unknown_Result_On_Server_Error()
        {
            _mockCustomerRepository.Add(Arg.Any<string>()).Throws(new Exception());
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Add("TestCustomer");
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.Unknown, result.ErrorMessage);
        }

        [Test]
        public void Given_A_Null_Customer_When_Update_Should_Return_Bad_Request_Result()
        {
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Update(null);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.BadRequest, result.ErrorMessage);
        }

        [Test]
        public void Given_A_Empty_Customer_Name_When_Update_Should_Return_Bad_Request_Result()
        {
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Update(new Customer { Id = 1 });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.BadRequest, result.ErrorMessage);
        }

        [Test]
        public void Given_An_Invalid_Customer_When_Update_Should_Return_Item_Not_Found_Result()
        {
            _mockCustomerRepository.Update(Arg.Any<Customer>()).Returns(-1);
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Update(new Customer { Id = 1, Name = "TestCustomer" });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.ItemNotFound, result.ErrorMessage);
        }

        [Test]
        public void Given_A_Valid_Customer_When_Update_Should_Return_Ok_Result()
        {
            _mockCustomerRepository.Update(Arg.Any<Customer>()).Returns(1);
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Update(new Customer { Id = 1, Name = "TestCustomer" });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public void Given_A_Customer_When_Update_Should_Return_Unknown_Result_On_Server_Error()
        {
            _mockCustomerRepository.Update(Arg.Any<Customer>()).Throws(new Exception());
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Update(new Customer { Id = 1, Name = "TestCustomer" });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.Unknown, result.ErrorMessage);
        }

        [Test]
        public void Given_An_Invalid_Customer_Id_When_Get_Should_Return_Item_Not_Found_Result()
        {
            _mockCustomerRepository.Get(Arg.Any<int>()).Returns(default(Customer));
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Get(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.ItemNotFound, result.ErrorMessage);
        }

        [Test]
        public void Given_A_Valid_Customer_Id_When_Get_Should_Return_Ok_Result()
        {
            _mockCustomerRepository.Get(Arg.Any<int>()).Returns(new Customer { Id = 1, Name = "TestCustomer" });
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Get(1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(1 == result.Data.Id && "TestCustomer" == result.Data.Name);
        }

        [Test]
        public void Given_A_Customer_Id_When_Get_Should_Return_Unknown_Result_On_Server_Error()
        {
            _mockCustomerRepository.Get(Arg.Any<int>()).Throws(new Exception());
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.Get(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.Unknown, result.ErrorMessage);
        }

        [Test]
        public void Given_Customers_When_GetAll_Should_Return_Ok_Result()
        {
            _mockCustomerRepository.GetAll().Returns(new List<Customer> { new Customer { Id = 1, Name = "TestCustomer" } });
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.GetAll();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Data.Count());
            Assert.IsTrue(1 == result.Data.First().Id && "TestCustomer" == result.Data.First().Name);
        }

        [Test]
        public void Given_Customers_When_GetAll_Should_Return_Unknown_Result_On_Server_Error()
        {
            _mockCustomerRepository.GetAll().Throws(new Exception());
            var manager = new CustomerManager(_mockCustomerRepository);

            var result = manager.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpErrorCode.Unknown, result.ErrorMessage);
        }
    }
}
