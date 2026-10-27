using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PlacementDashboard.Data;

namespace PlacementDashboard.Controllers;

public sealed class PlacementController : Controller
{
    private static readonly string[] AllowedStatuses = { "All", "Placed", "In Process", "Not Placed" };

    private readonly PlacementRepository _repository;
    public PlacementController(PlacementRepository repository) => _repository = repository;

    public async Task<IActionResult> Index(string status = "All")
    {
        if (!AllowedStatuses.Contains(status)) status = "All";
        try { return View(await _repository.GetDashboardAsync(status)); }
        catch (Exception ex)
        {
            ViewBag.DatabaseError = "Could not connect to PlacementDB. Run the SQL scripts in the Database folder and verify appsettings.json. Details: " + ex.Message;
            return View("DatabaseError");
        }
    }

    public async Task<IActionResult> Report(string status = "All")
    {
        if (!AllowedStatuses.Contains(status)) status = "All";
        try { return View(await _repository.GetDashboardAsync(status)); }
        catch (Exception ex)
        {
            return Content("Could not load report. Please set up PlacementDB and check appsettings.json. Details: " + ex.Message, "text/plain");
        }
    }

    // NEW FEATURE: download the (filtered) placement records as a CSV file that opens in Excel.
    public async Task<IActionResult> ExportCsv(string status = "All")
    {
        if (!AllowedStatuses.Contains(status)) status = "All";
        try
        {
            var vm = await _repository.GetDashboardAsync(status);
            var sb = new StringBuilder();
            sb.AppendLine("Enrollment No,Student Name,Department,Company,Job Role,Package (LPA),Status,Placement Date");
            foreach (var p in vm.Placements)
            {
                sb.AppendLine(string.Join(",",
                    Csv(p.EnrollmentNo), Csv(p.StudentName), Csv(p.Department), Csv(p.CompanyName), Csv(p.JobRole),
                    p.PackageLPA.ToString("0.00", CultureInfo.InvariantCulture),
                    Csv(p.PlacementStatus),
                    p.PlacementDate?.ToString("yyyy-MM-dd") ?? ""));
            }
            var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
            var bytes = utf8.GetPreamble().Concat(utf8.GetBytes(sb.ToString())).ToArray();
            var fileName = $"placements_{status.Replace(' ', '-').ToLowerInvariant()}_{DateTime.Now:yyyyMMdd}.csv";
            return File(bytes, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            return Content("Could not export CSV. Please set up PlacementDB and check appsettings.json. Details: " + ex.Message, "text/plain");
        }
    }

    public IActionResult Error() => View("Error");

    private static string Csv(string value)
    {
        // Guard against spreadsheet formula injection, then quote the value.
        if (value.Length > 0 && "=+-@".Contains(value[0])) value = "'" + value;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }
}
