using System;
using System.Collections.Generic;

namespace Yeni_Web_Projem.Models;

public partial class PhoneHistory
{
    public int? UserId { get; set; }

    public string? OldPhoneNumber { get; set; }

    public string? NewPhoneNumber { get; set; }

    public DateTime? ChangeDate { get; set; }

    public int? PhoneChangeId { get; set; }

    public virtual User? User { get; set; }
}
