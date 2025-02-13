using System;
using System.Collections.Generic;

namespace Cosmetics.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int Orderstatus { get; set; }

    public DateTime Orderdeliverydate { get; set; }

    public int Orderpickuppoint { get; set; }

    public string Ordernumber { get; set; } = null!;

    public int Userid { get; set; }

    public DateTime Datenew { get; set; }

    public decimal Ordertotalcost { get; set; }

    public virtual Pickuppoint OrderpickuppointNavigation { get; set; } = null!;

    public virtual Orderstatus OrderstatusNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Product> Productarticlenumbers { get; set; } = new List<Product>();
}
