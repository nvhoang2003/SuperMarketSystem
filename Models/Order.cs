using System;
using System.Collections.Generic;

namespace SuperMarketSystem.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime DateOfPurchase { get; set; }

    public float Amount { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Customer Customer { get; set; }
}
