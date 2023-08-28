using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketSystem.Models;

public class Product
{
    public Product()
    {
        Rates = new HashSet<Rate>();
    }
    public int Id { get; set; }

    public string Name { get; set; }

    public int CategoryId { get; set; }

    public int Quantity { get; set; }

    public float UnitCost { get; set; }

    public int BrandId { get; set; }

    public virtual ICollection<Image> Image { get; set; } = new List<Image>();
    public string ShoppingCartId { get; set; }
    public virtual ICollection<ShoppingCartItem> ShoppingCartItems {get;set;}

    public virtual Category Category { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
}
