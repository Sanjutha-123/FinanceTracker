
using FinanceTracker.Models;
using FinanceTrackerApi.Data;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace FinanceTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _service;

        public TransactionController(TransactionService service)
        {
            _service = service;
        }

        // ---------------------- CREATE ----------------------
     [HttpPost("Add")]
public IActionResult Add([FromBody] Transaction t)
{
    // Validate Type as string
    if (string.IsNullOrWhiteSpace(t.Type) || 
        !(t.Type.ToLower() == "income" || t.Type.ToLower() == "expense"))
    {
        return BadRequest("Type must be either 'income' or 'expense'.");
    }

    var result = _service.AddTransaction(t);
    return Ok(result);
}

    
  // ---------------------- GET BY USER ----------------------
        [HttpGet("User/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            var data = _service.GetByUser(userId);
            return Ok(data);
        }

        // ---------------------- UPDATE ----------------------
   [HttpPut("Update/{id}")]
public IActionResult Update(int id, [FromBody] Transaction t)
{
    // Validate Type as string
    if (string.IsNullOrWhiteSpace(t.Type) || 
        !(t.Type.ToLower() == "income" || t.Type.ToLower() == "expense"))
    {
        return BadRequest("Type must be either 'income' or 'expense'.");
    }

    bool ok = _service.UpdateTransaction(id, t);

    if (!ok)
        return NotFound("Transaction not found");

    return Ok("Updated successfully");
}

        // ---------------------- DELETE ----------------------
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            bool ok = _service.DeleteTransaction(id);

            if (!ok)
                return NotFound("Transaction not found");

            return Ok("Deleted successfully");
        }
 
       [HttpGet("filter")]
public async Task<IActionResult> FilterTransactions(
    [FromQuery] int userId,
    [FromQuery] string? start,
    [FromQuery] string? end,
    [FromQuery] string? category,
    [FromQuery] string? type)
{
    DateTime? startDate = null;
    DateTime? endDate = null;

    if (!string.IsNullOrEmpty(start))
        startDate = DateTime.ParseExact(start, "dd-MM-yyyy", CultureInfo.InvariantCulture);

    if (!string.IsNullOrEmpty(end))
        endDate = DateTime.ParseExact(end, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                          .AddHours(23).AddMinutes(59).AddSeconds(59); // include full day
    var data = await _service.Filter(userId, startDate, endDate, category, type);
    return Ok(data);

}
    }
}


