﻿using System;
using System.Collections.Generic;


namespace SuperMarketSystem.Models;

public partial class Category
{
    public Category()
    {
        Products = new HashSet<Product>();
    }
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
