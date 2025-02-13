using System;
using System.Collections.Generic;

namespace Cosmetics.Models;

public partial class Product
{
    public string Productarticlenumber { get; set; } = null!;

    public string Productname { get; set; } = null!;

    public string Productdescription { get; set; } = null!;

    public int Productcategoryid { get; set; }

    public string? Productphoto { get; set; }

    public int Productmanufacturerid { get; set; }

    public decimal Productcost { get; set; }

    public short? Productdiscountamount { get; set; }

    public int Productquantityinstock { get; set; }

    public int Productstatusid { get; set; }

    public virtual Category Productcategory { get; set; } = null!;

    public virtual Manufacturer Productmanufacturer { get; set; } = null!;

    public virtual Productstatus Productstatus { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
