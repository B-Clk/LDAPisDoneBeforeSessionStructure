using System;
using System.Collections.Generic;

namespace Yeni_Web_Projem.Models;

public partial class Email
{
    public int EmailId { get; set; }

    public string? EmailName { get; set; }

    public DateTime? EmailCreationDate { get; set; }

    public string? EmailDescription { get; set; }

    public int? EmailTypeId { get; set; }

    public virtual EmailType? EmailType { get; set; }

    public virtual ICollection<UserResponsibleEmail> UserResponsibleEmails { get; set; } = new List<UserResponsibleEmail>();
}
