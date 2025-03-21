using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Swizom.Utility
{
    public class ExceptionHandler
    {
        public async Task<IActionResult> HandleExceptionsAsync(Func<Task<IActionResult>> func, string actionName)
        {
            try
            {
                return await func();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception in {actionName}: {ex.Message}");
                return new StatusCodeResult(500);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ArgumentNullException in {actionName}: {ex.Message}");
                return new StatusCodeResult(500);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"InvalidOperationException in {actionName}: {ex.Message}");
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {actionName}: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
