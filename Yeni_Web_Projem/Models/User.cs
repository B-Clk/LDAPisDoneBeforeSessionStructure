using System;
using System.Collections.Generic;

namespace Yeni_Web_Projem.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Title { get; set; }

    public int? DepartmentId { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? FirstLoginDate { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<UserChangesEmail> UserChangesEmails { get; set; } = new List<UserChangesEmail>();

    public virtual ICollection<UserResponsibleEmail> UserResponsibleEmails { get; set; } = new List<UserResponsibleEmail>();
}
