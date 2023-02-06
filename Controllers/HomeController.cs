
using BulkyBookWeb.HttpServices;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChainResource _chainResource;

        public HomeController(IChainResource chainResource)
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
