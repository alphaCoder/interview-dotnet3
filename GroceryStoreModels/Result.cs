namespace GroceryStoreModels
{
    public class Result<TKey, TError>
    {
        public TKey Data { get; set; }
        public TError ErrorMessage { get; set; }
        public bool Success { get; set; }
        public static Result<TKey, TError> Ok(TKey item)
        {
            return new Result<TKey, TError> { Data = item, Success = true };
        }

        public static Result<TKey, TError> Error(TError errorMessage)
        {
            return new Result<TKey, TError> { Success = false, ErrorMessage = errorMessage };
        }
    }
}
