using System;
using System.Collections.Generic;

namespace Cosmetics.Models;

public partial class Manufacturer
{
    public int Manufacturerid { get; set; }

    public string Manufacturername { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
