﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EDC.Shared.Models;

public partial class Constituency
{
    public int Id { get; set; }

    public int ParliamentConstituencyNo { get; set; }

    public string Name { get; set; }

    public string TamilName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ParliamentConstituency ParliamentConstituencyNoNavigation { get; set; }
}