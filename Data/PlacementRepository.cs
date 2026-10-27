using Microsoft.Data.SqlClient;
using PlacementDashboard.Models;

namespace PlacementDashboard.Data;

public sealed class PlacementRepository
{
    private readonly DbConnectionFactory _db;
    public PlacementRepository(DbConnectionFactory db) => _db = db;

    public async Task<PlacementDashboardViewModel> GetDashboardAsync(string status = "All")
    {
        var vm = new PlacementDashboardViewModel { StatusFilter = status };
        await using var con = _db.CreateConnection();
        await con.OpenAsync();

        const string summarySql = @"
            SELECT (SELECT COUNT(*) FROM dbo.Student) AS TotalStudents,
                   (SELECT COUNT(DISTINCT StudentId) FROM dbo.Placement WHERE PlacementStatus='Placed') AS PlacedStudents,
                   (SELECT COUNT(*) FROM dbo.Company) AS TotalCompanies,
                   (SELECT ISNULL(AVG(PackageLPA),0) FROM dbo.Placement WHERE PlacementStatus='Placed') AS AveragePackage;";
        await using (var cmd = new SqlCommand(summarySql, con))
        await using (var r = await cmd.ExecuteReaderAsync())
        {
            if (await r.ReadAsync())
            {
                vm.TotalStudents = r.GetInt32(0);
                vm.PlacedStudents = r.GetInt32(1);
                vm.TotalCompanies = r.GetInt32(2);
                vm.AveragePackage = r.GetDecimal(3);
            }
        }

        const string placementsSql = @"
            SELECT s.EnrollmentNo,s.StudentName,s.Department,c.CompanyName,c.JobRole,
                   p.PackageLPA,p.PlacementStatus,p.PlacementDate
            FROM dbo.Placement p
            JOIN dbo.Student s ON s.StudentId=p.StudentId
            JOIN dbo.Company c ON c.CompanyId=p.CompanyId
            WHERE (@Status='All' OR p.PlacementStatus=@Status)
            ORDER BY s.StudentName;";
        await using (var cmd = new SqlCommand(placementsSql, con))
        {
            cmd.Parameters.AddWithValue("@Status", status);
            await using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
            {
                vm.Placements.Add(new PlacementRecord
                {
                    EnrollmentNo = r.GetString(0), StudentName = r.GetString(1),
                    Department = r.GetString(2), CompanyName = r.GetString(3),
                    JobRole = r.GetString(4), PackageLPA = r.GetDecimal(5),
                    PlacementStatus = r.GetString(6), PlacementDate = r.IsDBNull(7) ? null : r.GetDateTime(7)
                });
            }
        }

        const string companySql = @"
            SELECT c.CompanyName,c.JobRole,
                   SUM(CASE WHEN p.PlacementStatus='Placed' THEN 1 ELSE 0 END) AS PlacedCount
            FROM dbo.Company c LEFT JOIN dbo.Placement p ON p.CompanyId=c.CompanyId
            GROUP BY c.CompanyName,c.JobRole ORDER BY c.CompanyName;";
        await using (var cmd = new SqlCommand(companySql, con))
        await using (var r = await cmd.ExecuteReaderAsync())
        {
            while (await r.ReadAsync())
                vm.Companies.Add(new CompanySummary
                {
                    CompanyName = r.GetString(0), JobRole = r.GetString(1),
                    PlacedCount = r.IsDBNull(2) ? 0 : Convert.ToInt32(r.GetValue(2))
                });
        }
        return vm;
    }
}
