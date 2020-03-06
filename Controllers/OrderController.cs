using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WubbaLubStore.Models;

namespace WubbaLubStore.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrderController : ControllerBase
  {
    public DatabaseContext db { get; set; } = new DatabaseContext();

    [HttpPost("{itemId}")]
    public async Task<ActionResult<Order>> CreateNewOrder(Order order, int itemId)
    {
      var itemInStock = db.Items.FirstOrDefault(i => i.Id == itemId);
      if (itemInStock.NumberInStock < 1)
      {
        return Ok(new { message = "That item is not in stock" });
      }
      else
      {
        await db.Orders.AddAsync(order);
        await db.SaveChangesAsync();
        var orderId = order.Id;
        var itemOrder = new ItemOrder
        {
          OrderId = orderId,
          ItemId = itemId
        };
        await db.ItemOrders.AddAsync(itemOrder);
        await db.SaveChangesAsync();
        return Ok(order);
      }
    }

  }
}