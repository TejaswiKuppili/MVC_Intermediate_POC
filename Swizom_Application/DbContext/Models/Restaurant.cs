﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwizomDbContext.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        [Required, MaxLength(15)]
        public string ContactNumber { get; set; }

        public List<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();
    }
}
