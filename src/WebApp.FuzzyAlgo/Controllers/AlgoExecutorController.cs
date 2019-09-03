using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.FuzzyAlgo.Interfaces;
using WebApp.FuzzyAlgo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.FuzzyAlgo.Controllers
{
    [Route("api/[controller]")]
    public class AlgoExecutorController : Controller
    {
        private readonly IAlgoRunService _service;

        public AlgoExecutorController(IAlgoRunService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("run")]
        public async Task<IActionResult> Run([FromBody]AlgoRunRequest request)
        {
            var response =  await _service.RunAlgo(request);

            return Ok(response);
        }
    }
}
