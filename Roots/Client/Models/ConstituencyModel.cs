namespace EDC.Client.Models;

public class ConstituencyModel
{
    public int? Id { get; set; }

    public int ParliamentConstituencyNo { get; set; }

    public string Name { get; set; }

    public string TamilName { get; set; }

    public bool? IsActive { get; set; }
}
