namespace EDC.Client.Models;

public class UserModel
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }
    public string MobileNo { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public bool? IsActive { get; set; }
}
