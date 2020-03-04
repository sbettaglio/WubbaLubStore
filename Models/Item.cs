using System;

namespace WubbaLubStore.Models
{
  public class Item
  {
    public int Id { get; set; }
    public int SKU { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberInStock { get; set; }
    public float Price { get; set; }
    public DateTime DateOrdered { get; set; }
  }
}