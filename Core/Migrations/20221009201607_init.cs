using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            return;
            migrationBuilder.CreateTable(
                name: "batches",
                columns: table => new
                {
                    BatchID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batches", x => x.BatchID);
                });

            migrationBuilder.CreateTable(
                name: "CardFees",
                columns: table => new
                {
                    BatchID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    Semester = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CardFeesID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardFees = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardFees", x => new { x.BatchID, x.Semester, x.FacultyNumber });
                });

            migrationBuilder.CreateTable(
                name: "Course97",
                columns: table => new
                {
                    subjectcode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Departments = table.Column<double>(type: "float", nullable: true),
                    semester = table.Column<double>(type: "float", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Hours = table.Column<double>(type: "float", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValueSql: "('10')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course97", x => x.subjectcode);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyNo = table.Column<int>(type: "int", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FacultyNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacultyNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyNumber);
                });

            migrationBuilder.CreateTable(
                name: "Gradepoints",
                columns: table => new
                {
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    semester = table.Column<long>(type: "bigint", nullable: true),
                    BatchID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    s_points = table.Column<float>(type: "real", nullable: true),
                    s_hours = table.Column<short>(type: "smallint", nullable: true),
                    gpa = table.Column<float>(type: "real", nullable: true),
                    c_points = table.Column<float>(type: "real", nullable: true),
                    c_hours = table.Column<short>(type: "smallint", nullable: true),
                    cgpa = table.Column<float>(type: "real", nullable: true),
                    pcgpa = table.Column<float>(type: "real", nullable: true),
                    ppcgpa = table.Column<float>(type: "real", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MaxSemester",
                columns: table => new
                {
                    SemesterNo = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    TotalHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaxSemester", x => new { x.SemesterNo, x.FacultyNumber, x.ProgramID });
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramNameA = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    programNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramID);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationTypes",
                columns: table => new
                {
                    RegistrationTypeID = table.Column<int>(type: "int", nullable: false),
                    RegistrationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SemBeginFinish",
                columns: table => new
                {
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    BatchID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    Semester = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    FinishDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Semster",
                columns: table => new
                {
                    semster = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semster", x => x.semster);
                });

            migrationBuilder.CreateTable(
                name: "signature",
                columns: table => new
                {
                    reg_nameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    reg_nameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dean_facultyA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dean_facultyE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dean_affairsA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dean_affairsE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExamOfficerA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExamOfficerE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacultyNumber = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StudentOutOfResult",
                columns: table => new
                {
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentOutOfResult", x => new { x.StudentNumber, x.semester });
                });

            migrationBuilder.CreateTable(
                name: "Studentstemp",
                columns: table => new
                {
                    serial = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Facultynumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApplicationNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    StudyFees = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Proiority = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentstemp", x => x.serial);
                });

            migrationBuilder.CreateTable(
                name: "SubjectMaxDegree",
                columns: table => new
                {
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Semester = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    BatchDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    MaxDegree = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectMaxDegree", x => new { x.SubjectCode, x.Semester, x.BatchDescription, x.ProgramID });
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectCodeID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    departmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    facultyNumber = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SubjectNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectHour = table.Column<short>(type: "smallint", nullable: true),
                    ProgramID = table.Column<int>(type: "int", nullable: true),
                    OrederInResult = table.Column<decimal>(type: "numeric(18,0)", nullable: true, defaultValueSql: "((0))"),
                    required = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectCodeID);
                });

            migrationBuilder.CreateTable(
                name: "subjectsall",
                columns: table => new
                {
                    subjectnameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subjectcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectHours = table.Column<short>(type: "smallint", nullable: true),
                    Facultynumber = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ProgramID = table.Column<int>(type: "int", nullable: true),
                    subjectnameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SubjectTypes",
                columns: table => new
                {
                    SubjectTypeID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    SubjectTypeA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectTypeE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TypeDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTypes", x => x.SubjectTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    ArEnResult = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    DblHoursInReGrade = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    SemsterNo = table.Column<long>(type: "bigint", nullable: true),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DblWieghtInReGrade = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblArchive",
                columns: table => new
                {
                    StudentID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddmissionDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddmissionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GraduationDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CertificateType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddmissionFormNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Birthdate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalityE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HomeLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudyFees = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RegistrationFees = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProgramID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProgramNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProgramNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacultyNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacultyNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Semester = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectHour = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Degree = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectGrade = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    SubjectGradeA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Regrade = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    RegradeA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reregrade = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ReregradeA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Register = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    s_Hours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    s_Points = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GPA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    c_Hours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    c_Points = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CGPA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchGradeID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchSubjectID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblGrade",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    GradeA = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    xMin = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    xMax = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGrade", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TblStatus",
                columns: table => new
                {
                    StatusID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    Staus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    UserTypeID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    UserTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.UserTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DegreeOfferE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DegreeOfferA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => new { x.FacultyNumber, x.DepartmentNumber });
                    table.ForeignKey(
                        name: "FK_Departments_Faculties",
                        column: x => x.FacultyNumber,
                        principalTable: "Faculties",
                        principalColumn: "FacultyNumber");
                });

            migrationBuilder.CreateTable(
                name: "TblSections",
                columns: table => new
                {
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProgramID = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    Semester = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SectionID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectCodeID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSections", x => new { x.SubjectCode, x.ProgramID, x.Semester, x.DepartmentNumber, x.FacultyNumber });
                    table.ForeignKey(
                        name: "FK_TblSections_Subjects",
                        column: x => x.SubjectCodeID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectCodeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserPass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes",
                        column: x => x.UserTypeID,
                        principalTable: "UserTypes",
                        principalColumn: "UserTypeID");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudentID = table.Column<decimal>(type: "numeric(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentNameA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentNameE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProgramID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    addmissiontype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddmissionDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(((1)/(1))/(2000))"),
                    AddmissionFormNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValueSql: "((12345))"),
                    First_semster = table.Column<byte>(type: "tinyint", nullable: true, defaultValueSql: "((1))"),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalityE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true, defaultValueSql: "((0))"),
                    CertificateType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Std_Picture = table.Column<byte[]>(type: "image", nullable: true),
                    specialization = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GraduationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(((1)/(1))/(2000))"),
                    specializationE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    studentstatus = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    StudyFees = table.Column<decimal>(type: "numeric(24,2)", nullable: true, defaultValueSql: "((0))"),
                    RegistrationFees = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    Birthdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(((1)/(1))/(2000))"),
                    HomeLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastOfLists = table.Column<int>(type: "int", nullable: true),
                    OrederInResult = table.Column<decimal>(type: "numeric(18,0)", nullable: true, defaultValueSql: "((0))"),
                    currencyNo = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((1))"),
                    StudentPercent = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    RegistrationType = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentNumber);
                    table.ForeignKey(
                        name: "FK_Students_batches",
                        column: x => x.BatchID,
                        principalTable: "batches",
                        principalColumn: "BatchID");
                    table.ForeignKey(
                        name: "FK_Students_Departments",
                        columns: x => new { x.FacultyNumber, x.DepartmentNumber },
                        principalTable: "Departments",
                        principalColumns: new[] { "FacultyNumber", "DepartmentNumber" });
                    table.ForeignKey(
                        name: "FK_Students_Programs",
                        column: x => x.ProgramID,
                        principalTable: "Programs",
                        principalColumn: "ProgramID");
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Semester = table.Column<decimal>(type: "numeric(3,0)", nullable: false),
                    SlipNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValueSql: "((0))"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RegistrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValueSql: "((0))"),
                    RegistrationFees = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    StudyFees = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    RegCourses = table.Column<decimal>(type: "numeric(18,0)", nullable: true, defaultValueSql: "((0))"),
                    RegistrationTypeID = table.Column<decimal>(type: "numeric(10,0)", nullable: true, defaultValueSql: "((1))"),
                    Discount = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    PayedStudyFees = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    Note = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CardFees = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    Feed = table.Column<decimal>(type: "numeric(18,0)", nullable: true, defaultValueSql: "((0))"),
                    StudentID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    SlipID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => new { x.StudentNumber, x.Semester, x.SlipNumber });
                    table.ForeignKey(
                        name: "FK_Registration_Students",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                });

            migrationBuilder.CreateTable(
                name: "StudentSubjects",
                columns: table => new
                {
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectCodeID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    Semester = table.Column<long>(type: "bigint", nullable: false),
                    SubjectHour = table.Column<long>(type: "bigint", nullable: true, defaultValueSql: "((0))"),
                    Weight = table.Column<decimal>(type: "numeric(18,2)", nullable: true, defaultValueSql: "((0))"),
                    Degree = table.Column<float>(type: "real", nullable: true, defaultValueSql: "((0))"),
                    SubjectGrade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SubjectGradeA = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ReGrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReGradeA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReReGrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReReGradeA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    GradeCounter = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    Register = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeID = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSubjects", x => new { x.StudentNumber, x.SubjectCode, x.SubjectCodeID });
                    table.ForeignKey(
                        name: "FK_StudentSubjects_Students",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                    table.ForeignKey(
                        name: "FK_StudentSubjects_Subjects",
                        column: x => x.SubjectCodeID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectCodeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_BatchID",
                table: "Students",
                column: "BatchID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyNumber_DepartmentNumber",
                table: "Students",
                columns: new[] { "FacultyNumber", "DepartmentNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProgramID",
                table: "Students",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_SubjectCodeID",
                table: "StudentSubjects",
                column: "SubjectCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_TblSections_SubjectCodeID",
                table: "TblSections",
                column: "SubjectCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeID",
                table: "Users",
                column: "UserTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            return;
            migrationBuilder.DropTable(
                name: "CardFees");

            migrationBuilder.DropTable(
                name: "Course97");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Gradepoints");

            migrationBuilder.DropTable(
                name: "MaxSemester");

            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "RegistrationTypes");

            migrationBuilder.DropTable(
                name: "SemBeginFinish");

            migrationBuilder.DropTable(
                name: "Semster");

            migrationBuilder.DropTable(
                name: "signature");

            migrationBuilder.DropTable(
                name: "StudentOutOfResult");

            migrationBuilder.DropTable(
                name: "Studentstemp");

            migrationBuilder.DropTable(
                name: "StudentSubjects");

            migrationBuilder.DropTable(
                name: "SubjectMaxDegree");

            migrationBuilder.DropTable(
                name: "subjectsall");

            migrationBuilder.DropTable(
                name: "SubjectTypes");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "tblArchive");

            migrationBuilder.DropTable(
                name: "tblGrade");

            migrationBuilder.DropTable(
                name: "TblSections");

            migrationBuilder.DropTable(
                name: "TblStatus");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropTable(
                name: "batches");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Faculties");
        }
    }
}
