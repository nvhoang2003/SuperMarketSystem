using System;
using System.Collections.Generic;

namespace DataAccessLayer.DataObject;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int CategoryId { get; set; }

    public int Quantity { get; set; }

    public float UnitCost { get; set; }

    public float TotalAmount { get; set; }

    public Category Category { get; set; }

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
}
