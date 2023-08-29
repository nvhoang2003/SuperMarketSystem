using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models;

public partial class Customer 
{
    [Key]
    public int Id { get; set; }

    public Guid CustomerCode { get; set; }

    public string UserId { get; set; }

    public virtual ApplicationUser User { get; set; }

    public int CustomerInfoId { get; set; }

    public virtual CustomerInfo CustomerInfo { get; set; }

    public int CustomerAddressId { get; set; }

    public virtual CustomerAddress Address { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
}
