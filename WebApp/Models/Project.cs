namespace WebApp.Models;

public class Project
{
    public string Id { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Budget { get; set; }
    public Status Status { get; set; } = null!;

    //Kanske ???
    //public EditProjectFormModel Form { get; set; } = new();
}
