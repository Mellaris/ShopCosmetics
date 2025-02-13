﻿using System;
using System.Collections.Generic;

namespace Cosmetics.Models;

public partial class Productstatus
{
    public int Statusid { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
