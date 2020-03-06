using System;
using System.Collections.Generic;

namespace WubbaLubStore.Models
{
  public class Order
  {
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public int AmountOrdered { get; set; }
    public string Email { get; set; }
    public DateTime DatePlaced { get; set; } = DateTime.Now;
    public List<ItemOrder> ItemOrders { get; set; } = new List<ItemOrder>();

  }
}