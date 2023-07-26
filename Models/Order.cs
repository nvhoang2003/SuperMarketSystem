using System;
using System.Collections.Generic;

namespace DataAccessLayer.DataObject;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime DateOfPurchase { get; set; }

    public float Amount { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public Customer Customer { get; set; }
}
