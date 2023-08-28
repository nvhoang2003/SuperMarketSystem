using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models;

public partial class Bill
{
    [Key]
    public int Id { get; set; }

    public Guid BillCode { get; set; }

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public decimal BillAmount { get; set; }

    public string CreditCardNumber { get; set; }

    public DateTime CreditCardExpiry { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Order Order { get; set; }
}
