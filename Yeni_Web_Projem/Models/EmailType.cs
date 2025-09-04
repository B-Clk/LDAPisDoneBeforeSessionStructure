using System;
using System.Collections.Generic;

namespace Yeni_Web_Projem.Models;

public partial class EmailType
{
    public int EmailTypeId { get; set; }

    public string? EmailTypeName { get; set; }

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
}
