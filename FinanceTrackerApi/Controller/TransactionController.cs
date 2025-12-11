using FinanceTracker.Models;
using FinanceTrackerApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("Add")]
    public IActionResult AddTransaction([FromBody] Transaction request)
    {
        var saved = _transactionService.AddTransaction(request);
        return Ok(saved);
    }

    [HttpGet("User/{userId}")]
    public IActionResult GetUserTransactions(int userId)
    {
        return Ok(_transactionService.GetByUser(userId));
    }

    [HttpPut("Update")]
    public IActionResult Update([FromBody] Transaction t)
    {
        bool ok = _transactionService.UpdateTransaction(t);
        if (!ok) return NotFound("Transaction not found");

        return Ok("Updated successfully");
    }

    [HttpDelete("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        bool ok = _transactionService.DeleteTransaction(id);
        if (!ok) return NotFound();

        return Ok("Deleted successfully");
    }
}
}