using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models;

public partial class Rate
{
    [Key]
    public int Id { get; set; }

    public sbyte Star { get; set; }

    public string Content { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Product Product { get; set; }
}
