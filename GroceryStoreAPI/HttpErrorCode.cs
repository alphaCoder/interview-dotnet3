using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI
{
    public enum HttpErrorCode
    {
        ItemAlreadyExists,
        ItemNotFound,
        BadRequest,
        Unknown
    }
}
