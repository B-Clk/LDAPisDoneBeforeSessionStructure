using System;
using System.Collections.Generic;

namespace Yeni_Web_Projem.Models;

public partial class UserChangesEmail
{
    public int ChangeId { get; set; }

    public int UserId { get; set; }

    public int? PreviousEmailId { get; set; }

    public string? PreviousEmailName { get; set; }

    public int? NextEmailId { get; set; }

    public string? NextEmailName { get; set; }

    public DateTime? ChangeDate { get; set; }

    public virtual User User { get; set; } = null!;
}
