# Placement Dashboard — Practical 9 (.NET Technologies)

This project is based on the uploaded Marwadi University Practical 9 reference. It uses ASP.NET Core MVC, C#, ADO.NET (`Microsoft.Data.SqlClient`) and SQL Server LocalDB. It includes dashboard data controls, placement-status filtering, company-wise summaries and a printable HTML report as a Crystal Reports alternative.

## Crystal Reports alternative
The report is an HTML print view. Open **Open Report / Save PDF**, then click **Print / Save as PDF** and select **Microsoft Print to PDF** or **Save as PDF** in the browser print dialog. No Crystal Reports runtime or report designer is required.

## Requirements
- Visual Studio 2022 with the **ASP.NET and web development** workload
- .NET 8 SDK
- SQL Server LocalDB (normally included with Visual Studio) or SQL Server/SQL Express
- Internet access for the first NuGet package restore

## Database setup (required)
1. Open Visual Studio.
2. Select **View → SQL Server Object Explorer**. Expand **SQL Server** and connect to `(localdb)\MSSQLLocalDB`.
3. Alternatively, open SQL Server Management Studio and connect to `(localdb)\MSSQLLocalDB`.
4. Open and execute `Database/01_CreateDatabase.sql` first.
5. Open and execute `Database/02_CreateTablesAndSampleData.sql` second. **Note:** this script drops and recreates the three project tables, so running it again resets the sample data.
6. Optional: execute `Database/03_ReportQueries.sql` to see the report queries.

The database schema and sample data follow the supplied PDF: Student, Company and Placement tables, with six students and four companies.

## Run the project
1. Extract the ZIP file.
2. Open `PlacementDashboard.slnx` (or `PlacementDashboard/PlacementDashboard.csproj`) in Visual Studio.
3. If your SQL Server is LocalDB, the default connection string in `appsettings.json` should work as-is. For SQL Express, replace it with:
   `Server=.\\SQLEXPRESS;Database=PlacementDB;Trusted_Connection=True;TrustServerCertificate=True;`
4. Allow NuGet packages to restore.
5. Press **Ctrl+F5** or click **Run**.
6. The app launches at `http://localhost:5168` or the HTTPS address shown by Visual Studio.

## Theme
The whole interface (dashboard, report, error pages) uses a black and blue theme (`wwwroot/css/site.css` and `wwwroot/css/report.css`).
The printable report is dark on screen and switches to a clean white page with blue accents when you print / save as PDF.

## Features
- **NEW – Live search:** type in the search box to instantly filter the placement table by name, enrollment, company or role.
- **NEW – Export CSV:** the *Export CSV* button downloads the currently filtered records as a CSV file that opens in Excel.
- Placement-rate progress bar on the *Students Placed* card.
- Summary cards: total students, placed students, company count, average package for placed students.
- Filter records by All, Placed, In Process or Not Placed.
- Placement records joined across Student, Company and Placement.
- Company-wise placed-student counts.
- Printable report with browser Save as PDF support.
- Responsive layout and a database setup error page.

## Important note
The ZIP contains SQL scripts to create and seed the SQL Server database; it does not contain a SQL Server `.mdf` database file. Run the scripts once on your machine before launching the app. This follows the SQL Server LocalDB approach shown in the reference PDF.
