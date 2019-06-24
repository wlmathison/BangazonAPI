﻿/*
    Done by: Jameka Echols
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Budget { get; set; }
    }
}
