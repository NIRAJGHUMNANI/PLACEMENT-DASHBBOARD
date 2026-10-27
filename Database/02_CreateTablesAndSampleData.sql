USE PlacementDB;
GO
DROP TABLE IF EXISTS dbo.Placement;
DROP TABLE IF EXISTS dbo.Company;
DROP TABLE IF EXISTS dbo.Student;
GO
CREATE TABLE dbo.Student(
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentNo VARCHAR(20) NOT NULL UNIQUE,
    StudentName NVARCHAR(100) NOT NULL,
    Department NVARCHAR(80) NOT NULL,
    Email VARCHAR(120) NULL,
    CGPA DECIMAL(3,2) NULL
);
CREATE TABLE dbo.Company(
    CompanyId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(100) NOT NULL,
    JobRole NVARCHAR(100) NOT NULL
);
CREATE TABLE dbo.Placement(
    PlacementId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL REFERENCES dbo.Student(StudentId),
    CompanyId INT NOT NULL REFERENCES dbo.Company(CompanyId),
    PackageLPA DECIMAL(8,2) NOT NULL CHECK(PackageLPA >= 0),
    PlacementStatus NVARCHAR(20) NOT NULL CHECK(PlacementStatus IN ('Placed','In Process','Not Placed')),
    PlacementDate DATE NULL
);
GO
INSERT INTO dbo.Student(EnrollmentNo,StudentName,Department,Email,CGPA) VALUES
('CE001','Aarav Patel','Computer Engineering','aarav@example.com',8.40),
('CE002','Diya Shah','Computer Engineering','diya@example.com',9.10),
('CE003','Krish Mehta','Information Technology','krish@example.com',7.80),
('CE004','Anaya Desai','Computer Engineering','anaya@example.com',8.75),
('CE005','Vivaan Joshi','Information Technology','vivaan@example.com',7.25),
('CE006','Isha Trivedi','Computer Engineering','isha@example.com',8.05);
INSERT INTO dbo.Company(CompanyName,JobRole) VALUES
('TechNova','Software Developer'),('CloudBridge','Junior Engineer'),
('DataSphere','Data Analyst'),('WebCraft','Web Developer');
INSERT INTO dbo.Placement(StudentId,CompanyId,PackageLPA,PlacementStatus,PlacementDate) VALUES
(1,1,6.50,'Placed','2026-03-12'),
(2,2,8.00,'Placed','2026-03-15'),
(3,3,4.50,'In Process',NULL),
(4,1,7.25,'Placed','2026-03-20'),
(5,4,3.80,'Not Placed',NULL),
(6,3,5.75,'In Process',NULL);
GO
SELECT * FROM dbo.Student;
SELECT * FROM dbo.Company;
SELECT * FROM dbo.Placement;
