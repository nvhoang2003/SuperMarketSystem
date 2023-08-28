using System;
using System.Collections.Generic;

namespace SuperMarketSystem.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string PhoneNumber { get; set; }

    public string CreditCardNumber { get; set; }

    public DateTime CreditCardExpiry { get; set; }
    public virtual ICollection<ApplicationUser> Users { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
}
