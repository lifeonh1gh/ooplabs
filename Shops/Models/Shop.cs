﻿using System;

namespace Shops.Models
{
    public class Shop
    {
        public Shop(int id, string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            Id = id;
            Name = name;
            Address = address;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}