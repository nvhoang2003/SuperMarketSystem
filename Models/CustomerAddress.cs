﻿using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models
{
    public class CustomerAddress
    {
        [Key]
        public int Id { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}
