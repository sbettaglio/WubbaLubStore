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

    [HttpGet("location/{locationId}")]
    public async Task<ActionResult<List<Item>>> GetAllItems(int locationId)
    {
      return await db.Items.Where(o => o.LocationId == locationId).ToListAsync();
    }
    [HttpGet("{id}/{locationId}")]
    public async Task<ActionResult<Item>> GetOneItem(int id, int locationId)
    {
      var item = await db.Items.FirstOrDefaultAsync(i => i.Id == id && i.LocationId == locationId);
      if (item == null)
      {
        return NotFound();
      }
      return Ok(item);
    }
    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<List<Item>>> SearchSku(int sku)
    {
      var item = await db.Items.Where(i => i.SKU == sku).ToListAsync();
      if (item == null)
      {
        return NotFound();
      }
      return Ok(item);
    }
    [HttpPost("{locationId}")]
    public async Task<ActionResult<Item>> CreateNewItem(Item item, int locationId)
    {
      item.LocationId = locationId;
      await db.Items.AddAsync(item);
      await db.SaveChangesAsync();
      return Ok(item);
    }
    [HttpPut("{id}/{locationId}")]
    public async Task<ActionResult<Item>> UpdateOneItem(int id, int locationId, Item newData)
    {
      newData.Id = id;
      newData.LocationId = locationId;
      db.Entry(newData).State = EntityState.Modified;
      await db.SaveChangesAsync();
      return Ok(newData);
    }
    [HttpDelete("{id}/{locationId}")]
    public async Task<ActionResult> DeleteOne(int id, int locationId)
    {
      var item = await db.Items.FirstOrDefaultAsync(f => f.Id == id && f.LocationId == locationId);
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
    [HttpGet("soldout/{locationId}")]
    public async Task<ActionResult<List<Item>>> OutOfStock(int locationId)
    {
      var outOfStock = await db.Items.Where(i => i.LocationId == locationId && i.NumberInStock == 0).ToListAsync();

      return Ok(outOfStock);

    }

  }
}