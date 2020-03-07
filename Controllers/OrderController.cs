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
  public class OrderController : ControllerBase
  {
    public DatabaseContext db { get; set; } = new DatabaseContext();

    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
      return await db.Orders.OrderBy(o => o.Id).ToListAsync();
    }
    [HttpGet("items/{orderNumber}")]
    public async Task<ActionResult<List<Order>>> GetAllItemsInOrder(int orderNumber)
    {
      var orderExist = db.Orders.Any(o => o.OrderNumber == orderNumber);
      if (orderExist == true)
      {
        var itemsInOrder = db.Orders.Where(o => o.OrderNumber == orderNumber);
        return await itemsInOrder.ToListAsync();
      }
      else
      {
        return Ok(new { message = "That order is not in our system" });
      }
    }
    [HttpDelete("delete/order/{orderNumber}/{itemId}")]
    public async Task<ActionResult> RemoveItemFromOrder(int orderNumber, int itemId)
    {
      var itemToRemove = db.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber && o.Id == itemId);
      var itemOrderToRemove = db.ItemOrders.FirstOrDefault(i => i.OrderId == itemId && i.OrderId == itemToRemove.Id);
      var returnItemToInventory = db.Items.FirstOrDefault(r => r.Id == itemId);
      if (itemToRemove == null)
      {
        return NotFound();
      }
      returnItemToInventory.NumberInStock += itemToRemove.AmountOrdered;
      db.Orders.Remove(itemToRemove);
      db.ItemOrders.Remove(itemOrderToRemove);
      await db.SaveChangesAsync();
      return Ok(new { message = "Order has been updated" });
    }



    [HttpPost("{itemId}")]
    public async Task<ActionResult<List<Order>>> CreateNewOrder(Order order, int itemId)
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
        order.ItemOrders = null;
        return Ok(order);
      }
    }
    [HttpPatch("update/{Id}/add/{amountAdded}")]
    public async Task<ActionResult<Order>> AddToOneOrder(int id, int amountAdded)
    {
      var orderAmountUpdate = await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
      var itemOrderLink = await db.ItemOrders.FirstOrDefaultAsync(i => i.OrderId == id);
      var itemInStock = await db.Items.FirstOrDefaultAsync(s => s.Id == itemOrderLink.ItemId);
      if (amountAdded > itemInStock.NumberInStock)
      {
        return Ok(new { message = "Inventory is not sufficient to fulfill this order. Please enter a lower amount" });
      }
      else
      {
        itemInStock.NumberInStock -= amountAdded;
        orderAmountUpdate.AmountOrdered += amountAdded;
        await db.SaveChangesAsync();
        return Ok(new { message = $"Your new order amount is: {orderAmountUpdate.AmountOrdered}" });
      }
    }
    [HttpPatch("update/{Id}/remove/{amountRemoved}")]
    public async Task<ActionResult<Order>> RemoveFromOneOrder(int id, int amountRemoved)
    {
      var orderAmountUpdate = await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
      var itemOrderLink = await db.ItemOrders.FirstOrDefaultAsync(i => i.OrderId == id);
      var itemInStock = await db.Items.FirstOrDefaultAsync(s => s.Id == itemOrderLink.ItemId);

      itemInStock.NumberInStock += amountRemoved;
      orderAmountUpdate.AmountOrdered -= amountRemoved;
      await db.SaveChangesAsync();
      return Ok(new { message = $"Your new order amount is: {orderAmountUpdate.AmountOrdered}" });

    }




  }
}