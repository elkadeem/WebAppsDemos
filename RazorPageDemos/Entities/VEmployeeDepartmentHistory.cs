﻿using System;
using System.Collections.Generic;

namespace RazorPageDemos.Entities;

public partial class VEmployeeDepartmentHistory
{
    public int BusinessEntityId { get; set; }

    public string? Title { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? Suffix { get; set; }

    public string Shift { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
