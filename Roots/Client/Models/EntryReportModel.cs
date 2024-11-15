namespace EDC.Client.Models;

public class EntryReportModel
{
    public int Id { get; set; }
    public string CreatedBy { get; set; }

    public int? RecordCount { get; set; }

    public DateOnly? EntryDate { get; set; }
}
