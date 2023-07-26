using System;
using System.Collections.Generic;

namespace DataAccessLayer.DataObject;

public partial class Rate
{
    public int Id { get; set; }

    public sbyte Star { get; set; }

    public string Content { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }

    public Product Product { get; set; }
}
