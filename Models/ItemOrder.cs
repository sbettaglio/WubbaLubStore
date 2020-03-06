using System.Collections.Generic;

namespace WubbaLubStore.Models
{
  public class ItemOrder
  {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }

  }
}