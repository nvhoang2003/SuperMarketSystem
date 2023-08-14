using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DataObject;

public class Product
{
    public Product() 
    {
        Rates = new HashSet<Rate>();
    }
    public int Id { get; set; }

    public string Name { get; set; }

    public int CategoryId { get; set; }

    //public string ImageUrl { get; set; }

    public int Quantity { get; set; }

    public float UnitCost { get; set; }

    public float TotalAmount { get; set; }
    public bool IsProductOfTheWeek { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
}
