using BulkyBookWeb.Data;
using BulkyBookWeb.HttpServices;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly IChainResource _chainResource;

        public ApiController(IChainResource chainResource)
        {
            _chainResource = chainResource;
        }

        //GET
        public async Task<ActionResult> GetFromApi()
        {
            try
            {
                var rates = await _chainResource.GetValue();
                if (rates == null)
                {
                    return BadRequest("err");
                }
                return Ok(rates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
