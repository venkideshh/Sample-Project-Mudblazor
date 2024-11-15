﻿namespace EDC.Client.Models;

public class MeetingEntryModel
{
    public int Id { get; set; }

    public int? DistrictId { get; set; }

    public int? ParliamentConstituencyId { get; set; }

    public int? ConstituencyId { get; set; }

    public int? AreaTypeId { get; set; }

    public string AreaName { get; set; }

    public int? BoothNo { get; set; }

    public string Name { get; set; }

    public string PhoneNo { get; set; }

    public string Gender { get; set; }

    public string Address { get; set; }

    public string Image { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsActive { get; set; }

    public byte[] Content { get; set; }
}
