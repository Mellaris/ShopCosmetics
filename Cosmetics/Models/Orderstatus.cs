using System;
using System.Collections.Generic;

namespace Cosmetics.Models;

public partial class Orderstatus
{
    public int Statusid { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
