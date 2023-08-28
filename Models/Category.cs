using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    public Guid CategoryCode { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public Category()
    {
        Products = new HashSet<Product>();
    }
}
