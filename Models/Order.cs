using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public List<OrderDetails> OrderLines { get; set; }

    public int CustomerId { get; set; }

    public DateTime DateOfPurchase { get; set; }

    public decimal OrderTotal { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Customer Customer { get; set; }
}
