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
  public class ItemController : ControllerBase
  {
    public DatabaseContext db { get; set; } = new DatabaseContext();

    [HttpGet]
    public async Task<ActionResult<List<Item>>> GetAllItems()
    {
      return await db.Items.OrderBy(o => o.Name).ToListAsync();
    }
    [HttpGet("{id}")]
    public ActionResult<Item> GetOneItem(int id)
    {
      var item = db.Items.FirstOrDefault(i => i.Id == id);
      if (item == null)
      {
        return NotFound();
      }
      return Ok(item);
    }
    [HttpPost]
    public async Task<ActionResult<Item>> CreateNewItem(Item item)
    {
      await db.Items.AddAsync(item);
      await db.SaveChangesAsync();
      return Ok(item);
    }
  }
}