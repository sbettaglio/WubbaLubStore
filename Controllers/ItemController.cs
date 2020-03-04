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
    public async Task<ActionResult<Item>> GetOneItem(int id)
    {
      var item = await db.Items.FirstOrDefaultAsync(i => i.Id == id);
      if (item == null)
      {
        return NotFound();
      }
      return Ok(item);
    }
    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<Item>> SearchSku(int sku)
    {
      var item = await db.Items.FirstOrDefaultAsync(i => i.SKU == sku);
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
    [HttpPut("{id}")]
    public async Task<ActionResult<Item>> UpdateOneItem(int id, Item newData)
    {
      newData.Id = id;
      db.Entry(newData).State = EntityState.Modified;
      await db.SaveChangesAsync();
      return Ok(newData);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOne(int id)
    {
      var item = await db.Items.FirstOrDefaultAsync(f => f.Id == id);
      if (item == null)
      {
        return NotFound();
      }
      db.Items.Remove(item);
      await db.SaveChangesAsync();
      return Ok();
    }
    [HttpGet("soldout")]
    public async Task<ActionResult<List<string>>> OutOfStock()
    {
      var outOfStock = await db.Items.Where(i => i.NumberInStock == 0).ToListAsync();
      var outOfStockList = new List<string>();
      foreach (var pet in outOfStock)
      {
        outOfStockList.Add(pet.Name);
      }
      return Ok(outOfStockList);

    }

  }
}