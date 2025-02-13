using System;
using System.Collections.Generic;

namespace Cosmetics.Models;

public partial class Pickuppoint
{
    public int Pickuppointid { get; set; }

    public string Pickuppointaddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
