namespace PlacementDashboard.Models;

public sealed class PlacementRecord
{
    public string EnrollmentNo { get; set; } = "";
    public string StudentName { get; set; } = "";
    public string Department { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public string JobRole { get; set; } = "";
    public decimal PackageLPA { get; set; }
    public string PlacementStatus { get; set; } = "";
    public DateTime? PlacementDate { get; set; }
}

public sealed class CompanySummary
{
    public string CompanyName { get; set; } = "";
    public string JobRole { get; set; } = "";
    public int PlacedCount { get; set; }
}

public sealed class PlacementDashboardViewModel
{
    public int TotalStudents { get; set; }
    public int PlacedStudents { get; set; }
    public int TotalCompanies { get; set; }
    public decimal AveragePackage { get; set; }
    public string StatusFilter { get; set; } = "All";
    public List<PlacementRecord> Placements { get; set; } = new();
    public List<CompanySummary> Companies { get; set; } = new();
}
