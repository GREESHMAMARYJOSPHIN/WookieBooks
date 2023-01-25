using Microsoft.AspNetCore.Mvc;
using RestAPI.Contracts.Breakfast;

namespace RestAPI.Controllers
{
    [ApiController]
    public class BreakfastController : ControllerBase
    {
        [HttpPost("/breakfast")]
        public IActionResult CreateBreakfast(CreateRequest breakfastRequest)
        {
            return Ok(breakfastRequest);
        }

        [HttpGet("/breakfast/{id:guid}")]
        public IActionResult GetBreakfast(Guid id)
        {
            return Ok(id);
        }

        [HttpPut("/breakfast/{id:guid}")]
        public IActionResult UpdateBreakfast(Guid id, UpdateRequest breakfastRequest)
        {
            return Ok(breakfastRequest);
        }

        [HttpDelete("/breakfast/{id:guid}")]
        public IActionResult DeleteBreakfast(Guid id)
        {
            return Ok(id);
        }

    }
}
