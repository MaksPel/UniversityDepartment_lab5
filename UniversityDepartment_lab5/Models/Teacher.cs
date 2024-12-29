using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityDepartment_lab5.Models;

public partial class Teacher
{
    public Guid TeacherId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Midname { get; set; } = null!;

    public string Position { get; set; } = null!;

    [Range(18, 100, ErrorMessage = "Возраст должен быть в диапазоне от 18 до 100.")]
    public int Age { get; set; }

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
