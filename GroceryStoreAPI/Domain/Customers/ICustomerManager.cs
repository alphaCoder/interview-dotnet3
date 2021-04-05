using GroceryStoreModels;
using System.Collections.Generic;

namespace GroceryStoreAPI.Domain.Customers
{
    public interface ICustomerManager
    {
        Result<Customer, HttpErrorCode> Add(string customerName);
        Result<Customer, HttpErrorCode> Get(int customerId);
        Result<IEnumerable<Customer>, HttpErrorCode> GetAll();
        Result<bool, HttpErrorCode> Update(Customer customer);
    }
}