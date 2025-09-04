using System;
using System.Collections.Generic;

namespace Yeni_Web_Projem.Models;

public partial class UserResponsibleEmail
{
    public int UserId { get; set; }

    public int EmailId { get; set; }

    public DateTime? PermissionStartDate { get; set; }

    public DateTime? PermissionEndDate { get; set; }

    public virtual Email Email { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
