using System;
using System.Collections.Generic;

namespace Yeni_Web_Projem.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
