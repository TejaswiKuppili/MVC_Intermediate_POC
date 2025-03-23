using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Swizom.Utility
{
    public class ExceptionHandler
    {
        public async Task<T> HandleExceptionsAsync<T>(Func<Task<T>> func, string actionName)
        {
            try
            {
                return await func();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception in {actionName}: {ex.Message}");
                return default;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ArgumentNullException in {actionName}: {ex.Message}");
                return default;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"InvalidOperationException in {actionName}: {ex.Message}");
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {actionName}: {ex.Message}");
                return default;
            }
        }
    }
}
