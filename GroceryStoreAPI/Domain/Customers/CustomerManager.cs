using GroceryStoreModels;
using System;
using System.Collections.Generic;

namespace GroceryStoreAPI.Domain.Customers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Result<Customer, HttpErrorCode> Add(string customerName)
        {
            try
            {
                if (string.IsNullOrEmpty(customerName))
                {
                    return Result<Customer, HttpErrorCode>.Error(HttpErrorCode.BadRequest);
                }

                var result = _customerRepository.Add(customerName);
                if (result == -1)
                {
                    return Result<Customer, HttpErrorCode>.Error(HttpErrorCode.ItemAlreadyExists);
                }
                return Result<Customer, HttpErrorCode>.Ok(_customerRepository.Get(result));
            }
            catch (Exception e)
            {
                // todo: Log it
                return Result<Customer, HttpErrorCode>.Error(HttpErrorCode.Unknown);
            }
        }

        public Result<bool, HttpErrorCode> Update(Customer customer)
        {
            try
            {
                if (customer == null || string.IsNullOrEmpty(customer.Name))
                {
                    return Result<bool, HttpErrorCode>.Error(HttpErrorCode.BadRequest);
                }

                var result = _customerRepository.Update(customer);
                if (result == -1)
                {
                    return Result<bool, HttpErrorCode>.Error(HttpErrorCode.ItemNotFound);
                }

                return Result<bool, HttpErrorCode>.Ok(true);
            }
            catch (Exception e)
            {
                // todo: Log it
                return Result<bool, HttpErrorCode>.Error(HttpErrorCode.Unknown);
            }
        }

        public Result<Customer, HttpErrorCode> Get(int customerId)
        {
            try
            {
                var customer = _customerRepository.Get(customerId);
                if (customer == null)
                {
                    return Result<Customer, HttpErrorCode>.Error(HttpErrorCode.ItemNotFound);
                }
                return Result<Customer, HttpErrorCode>.Ok(customer);
            }
            catch (Exception e)
            {
                // todo: Log it
                return Result<Customer, HttpErrorCode>.Error(HttpErrorCode.Unknown);
            }
        }

        public Result<IEnumerable<Customer>, HttpErrorCode> GetAll()
        {
            try
            {
                return Result<IEnumerable<Customer>, HttpErrorCode>.Ok(_customerRepository.GetAll());
            }
            catch (Exception e)
            {
                // todo: Log it
                return Result<IEnumerable<Customer>, HttpErrorCode>.Error(HttpErrorCode.Unknown);
            }
        }
    }
}
