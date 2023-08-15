using System;
using System.Collections.Generic;

namespace SuperMarketSystem.Models;

public partial class Bill
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public float BillAmount { get; set; }

    public string CreditCardNumber { get; set; }

    public DateTime CreditCardExpiry { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Order Order { get; set; }
}
