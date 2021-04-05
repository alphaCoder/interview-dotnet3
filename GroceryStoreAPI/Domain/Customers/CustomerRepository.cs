using GroceryStoreModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GroceryStoreAPI.Domain.Customers
{
    public interface ICustomerRepository
    {
        int Add(string customerName);
        Customer Get(int id);
        IEnumerable<Customer> GetAll();
        int Update(Customer customer);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _filePath = Path.Combine(Environment.CurrentDirectory, "database.json");
        private DbContext _dbContext;

        public CustomerRepository()
        {
            var data = File.ReadAllText(_filePath);
            _dbContext = JsonConvert.DeserializeObject<DbContext>(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }, Formatting = Formatting.Indented });
        }
        public int Add(string customerName)
        {
            var id = _dbContext.Customers.Select(c => c.Id).Max() + 1;
            _dbContext.Customers.Add(new Customer { Id = id, Name = customerName });
            SaveCustomers();
            return id;
        }

        public Customer Get(int id) 
        {
            return _dbContext.Customers.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _dbContext.Customers;
        }

        public int Update(Customer customer)
        {
            var existingCustomer = _dbContext.Customers.FirstOrDefault(c => c.Id == customer.Id);
            
            if (existingCustomer == null)
            {
                return -1;
            }

            existingCustomer.Name = customer.Name;
            SaveCustomers();
            return 1;
        }

        private void SaveCustomers()
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_dbContext, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }, Formatting = Formatting.Indented }));
        }
    }
}
