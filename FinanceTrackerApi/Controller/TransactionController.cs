using FinanceTracker.Models;
using FinanceTrackerApi.Data;
using Microsoft.AspNetCore.Mvc;

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
           if (!Enum.IsDefined(typeof(Transaction.TransactionType), t.Type))
        return BadRequest("Type must be income or expense.");

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
               if (!Enum.IsDefined(typeof(Transaction.TransactionType), t.Type))
        return BadRequest("Type must be either 'income' or 'expense'.");

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

        // ---------------------- FILTER ----------------------
        [HttpGet("Filter")]
        public IActionResult Filter(
            int userId,
            DateTime? start,
            DateTime? end,
            string? category,
            string? type)
        {
            try
    {
        var data = _service.Filter(userId, start, end, category, type);
        return Ok(data);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
}
    }
}