﻿namespace EDC.Client.Models;

public class ParliamentConstituencyModel
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public string TamilName { get; set; }

    public bool? IsActive { get; set; }

    public int? DistrictId { get; set; }
}