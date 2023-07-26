using System;
using System.Collections.Generic;

namespace DataAccessLayer.DataObject;

public partial class Bill
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public float BillAmount { get; set; }

    public string CreditCardNumber { get; set; }

    public DateTime CreditCardExpiry { get; set; }

    public Customer Customer { get; set; }

    public Order Order { get; set; }
}
