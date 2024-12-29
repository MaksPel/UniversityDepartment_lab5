using System;
using System.Collections.Generic;

namespace UniversityDepartment_lab5.Models;

public partial class Specialty
{
    public Guid SpecialtyId { get; set; }

    public string Name { get; set; } = null!;

    public Guid DepartmentId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Department Department { get; set; } = null!;
}
