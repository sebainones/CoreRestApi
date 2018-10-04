using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CountryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Country> Get()
        {
            return context.Countries.ToList();
        }

        [HttpGet("{id}", Name = "currentCountry")]
        public IActionResult Get(int id)
        {
            var currentCountry = context.Countries.Include(c => c.States).SingleOrDefault(c => c.Id == id);

            if (currentCountry == null)
            {
                return NotFound();
            }

            return Ok(currentCountry);
        }

        // POST api/countries
        [HttpPost]
        public IActionResult Post([FromBody] Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Countries.Add(country);
                    context.SaveChanges();
                    return new CreatedAtRouteResult("currentCountry", new { id = country.Id }, country);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest(ModelState);
        }
    }
}