﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Domain.Entities
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
		public int Stock { get; set; }
		public string? ImagePath { get; set; } // Dosya yolu için yeni property
	}
}
