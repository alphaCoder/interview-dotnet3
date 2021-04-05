using GroceryStoreAPI.Domain.Customers;
using NSubstitute;
using NUnit.Framework;
using GroceryStoreModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GroceryStoreAPI.UnitTests
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private ICustomerManager _mockManager;
        [SetUp]
        public void Setup()
        {
            _mockManager = Substitute.For<ICustomerManager>();
        }

        [Test]
        public void Given_Customers_When_GetAll_Should_Return_All_Customers()
        {
            var customers = new List<Customer> { new Customer { Id = 1, Name = "Tom" }, new Customer { Id = 2, Name = "Harry" } };
            var result = Result<IEnumerable<Customer>, HttpErrorCode>.Ok(customers);
            _mockManager.GetAll().Returns(result);
            var controller = new CustomersController(_mockManager);
            var actualResult = controller.GetAll() as OkObjectResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(200, actualResult.StatusCode);
            var acutalCustomers = ((List<Customer>)actualResult.Value);
            Assert.AreEqual(2, acutalCustomers.Count);
            Assert.IsTrue(acutalCustomers.Any(c => c.Id == customers[0].Id && c.Name == customers[0].Name));
            Assert.IsTrue(acutalCustomers.Any(c => c.Id == customers[1].Id && c.Name == customers[1].Name));
        }

        [Test]
        public void Given_No_Customers_When_GetAll_Should_Return_Zero_Customers()
        {
            var customers = new List<Customer> {};
            var result = Result<IEnumerable<Customer>, HttpErrorCode>.Ok(customers);
            _mockManager.GetAll().Returns(result);
            var controller = new CustomersController(_mockManager);
            var actualResult = controller.GetAll() as OkObjectResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(200, actualResult.StatusCode);
            var acutalCustomers = ((List<Customer>)actualResult.Value);
            Assert.AreEqual(0, acutalCustomers.Count);
        }

        [Test]
        public void Given_Customers_When_Get_A_Valid_Customer_Should_Return_the_Customer()
        {
            var customers = new List<Customer> { new Customer { Id = 1, Name = "Tom" }, new Customer { Id = 2, Name = "Harry" } };
            var result = Result<Customer, HttpErrorCode>.Ok(customers[0]);
            _mockManager.Get(Arg.Any<int>()).Returns(result);
            var controller = new CustomersController(_mockManager);
            var actualResult = controller.Get(1) as OkObjectResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(200, actualResult.StatusCode);
            var acutalCustomer = (Customer)actualResult.Value;

            Assert.IsNotNull(acutalCustomer);
            Assert.IsTrue(acutalCustomer.Id == 1 && acutalCustomer.Name == "Tom");
        }

        [Test]
        public void Given_Customers_When_Get_A_Valid_Customer_Should_Return_Not_Found_Status()
        {
            var result = Result<Customer, HttpErrorCode>.Error(HttpErrorCode.ItemNotFound);
            _mockManager.Get(Arg.Any<int>()).Returns(result);
            var controller = new CustomersController(_mockManager);
            var actualResult = controller.Get(3) as NotFoundResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(404, actualResult.StatusCode);
        }

        [Test]
        public void Given_A_Customer_When_Post_Should_Return_Ok_Result()
        {
            var result = Result<Customer, HttpErrorCode>.Ok(new Customer { Id = 1, Name = "TestCustomer" });
            _mockManager.Add(Arg.Any<string>()).Returns(result);

            var controller = new CustomersController(_mockManager);
            var actualResult = controller.Post("TestCustomer") as OkObjectResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(200, actualResult.StatusCode);
            var acutalCustomer = (Customer)actualResult.Value;

            Assert.IsNotNull(acutalCustomer);
            Assert.IsTrue(acutalCustomer.Id == 1 && acutalCustomer.Name == "TestCustomer");
        }

        [Test]
        public void Given_A_Empty_Customer_Name_When_Post_Should_Return_BadRequest_Result()
        {
            var result = Result<Customer, HttpErrorCode>.Error(HttpErrorCode.BadRequest);
            _mockManager.Add(Arg.Any<string>()).Returns(result);

            var controller = new CustomersController(_mockManager);
            var actualResult = controller.Post("") as BadRequestResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(400, actualResult.StatusCode);
        }

        [Test]
        public void Given_A_Customer_When_Put_Should_Return_Ok_Result()
        {
            var result = Result<bool, HttpErrorCode>.Ok(true);
            _mockManager.Update(Arg.Any<Customer>()).Returns(result);

            var controller = new CustomersController(_mockManager);
            var actualResult = controller.Put(new Customer { Id = 1, Name =  "TestCustomer1" }) as OkObjectResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(200, actualResult.StatusCode);
            Assert.IsTrue((bool)actualResult.Value);
        }


        [Test]
        public void Given_A_Empty_Customer_Name_When_Put_Should_Return_BadRequest_Result()
        {
            var result = Result<bool, HttpErrorCode>.Error(HttpErrorCode.BadRequest);
            _mockManager.Update(Arg.Any<Customer>()).Returns(result);

            var controller = new CustomersController(_mockManager);
            var actualResult = controller.Put(new Customer { Id = 1, Name = "" }) as BadRequestResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(400, actualResult.StatusCode);
        }

        [Test]
        public void Given_A_Invalid_Customer_When_Put_Should_Return_NotFound_Result()
        {
            var result = Result<bool, HttpErrorCode>.Error(HttpErrorCode.ItemNotFound);
            _mockManager.Update(Arg.Any<Customer>()).Returns(result);

            var controller = new CustomersController(_mockManager);
            var actualResult = controller.Put(new Customer { Id = 1, Name = "TestCustomer1" }) as NotFoundResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(404, actualResult.StatusCode);
        }
    }
}