using System;

namespace ThreeTier_CRUD.BLL
{
    /// <summary>
    /// Custom exception for business validation errors
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
