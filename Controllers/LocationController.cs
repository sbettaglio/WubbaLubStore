using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WubbaLubStore.Models;

namespace WubbaLubStore.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LocationController : ControllerBase
  {
    public DatabaseContext db { get; set; } = new DatabaseContext();

    [HttpGet]
    public async Task<ActionResult<List<Location>>> GetAllLocations()
    {
      return await db.Locations.OrderBy(o => o.Address).ToListAsync();
    }
    [HttpPost]
    public async Task<ActionResult<Location>> CreateNewLocation(Location location)
    {
      await db.Locations.AddAsync(location);
      await db.SaveChangesAsync();
      return Ok(location);
    }

  }
}