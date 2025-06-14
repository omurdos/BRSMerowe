using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Core.Entities
{
    public partial class TSTDBContext : IdentityDbContext<APIUser, Role, string>
    {
        public TSTDBContext()
        {
        }

        public TSTDBContext(DbContextOptions<TSTDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<CardFee> CardFees { get; set; }
        public virtual DbSet<Course97> Course97s { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<GetArchive> GetArchives { get; set; }
        public virtual DbSet<GetStdGrade> GetStdGrades { get; set; }
        public virtual DbSet<Gradepoint> Gradepoints { get; set; }
        public virtual DbSet<MaxSemester> MaxSemesters { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<RegistrationType> RegistrationTypes { get; set; }
        public virtual DbSet<Resulttest> Resulttests { get; set; }
        public virtual DbSet<SemBeginFinish> SemBeginFinishes { get; set; }
        public virtual DbSet<Semster> Semsters { get; set; }
        public virtual DbSet<Signature> Signatures { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentOutOfResult> StudentOutOfResults { get; set; }
        public virtual DbSet<StudentSubject> StudentSubjects { get; set; }
        public virtual DbSet<Studentstemp> Studentstemps { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectMaxDegree> SubjectMaxDegrees { get; set; }
        public virtual DbSet<SubjectType> SubjectTypes { get; set; }
        public virtual DbSet<Subjectsall> Subjectsalls { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<TblArchive> TblArchives { get; set; }
        public virtual DbSet<TblGrade> TblGrades { get; set; }
        public virtual DbSet<TblSection> TblSections { get; set; }
        public virtual DbSet<TblStatus> TblStatuses { get; set; }
        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<CardRequest> CardRequests { get; set; }
        public virtual DbSet<RequestStatus> RequestStatuses { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<EnrollmentRequest> EnrollmentRequests { get; set; }
        public virtual DbSet<CertificateRequest> CertificateRequests { get; set; }
        public virtual DbSet<TranscriptRequest> TranscriptRequests { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<AdmissionType> AdmissionTypes { get; set; }
        public virtual DbSet<StudentAttachment> StudentAttachments { get; set; }
        public virtual DbSet<Guardian> Guardians { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<OTPCode> OTPCodes { get; set; }
        public virtual DbSet<SMSAccess> SMSAccesses { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<PushNotification> PushNotifications { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<SystemClaim> SystemClaims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            if (!optionsBuilder.IsConfigured)
            {
                //AWS
                //optionsBuilder.UseSqlServer("Server=database-1.cjwi4xe9w5ck.us-east-2.rds.amazonaws.com;Initial Catalog=SinnarRSDB;User Id=admin;Password=ByxB1(_^3Jn2=.Pf:XY_34$&YO");
                optionsBuilder.UseSqlServer("Server=62.164.219.146;Initial Catalog=MeroweDB;User Id=sa;Password=Passme@123;TrustServerCertificate=True;");
                
                //Local DB
                //optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Initial Catalog=BRS;User Id=sa;Password=Passme@123;TrustServerCertificate=True;");
                
                
                
                //optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=SinnarRSDB444444;Trusted_Connection=True;");
                //optionsBuilder.UseSqlServer("Data Source=SQL8003.site4now.net;Initial Catalog=db_a8fd7d_sbresult;User Id=db_a8fd7d_sbresult_admin;Password=rqw7iD!c!!q7Ebv");
                // optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=BRSphar33;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.ToTable("batches");

                entity.Property(e => e.BatchId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BatchID");

                entity.Property(e => e.BatchDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });




            modelBuilder.Entity<CardFee>(entity =>
            {
                entity.HasKey(e => new { e.BatchId, e.Semester, e.FacultyNumber });

                entity.Property(e => e.BatchId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchID");

                entity.Property(e => e.Semester).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.CardFees).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.CardFeesId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CardFeesID");

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");
            });

            modelBuilder.Entity<Course97>(entity =>
            {
                entity.HasKey(e => e.Subjectcode);

                entity.ToTable("Course97");

                entity.Property(e => e.Subjectcode)
                    .HasMaxLength(255)
                    .HasColumnName("subjectcode");

                entity.Property(e => e.FacultyNumber)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('10')");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ID");

                entity.Property(e => e.Semester).HasColumnName("semester");

                entity.Property(e => e.SubjectName).HasMaxLength(255);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CurrencyName).HasMaxLength(50);
            });




            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => new { e.FacultyNumber, e.DepartmentNumber });

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.DepartmentNumber).HasMaxLength(50);

                entity.Property(e => e.DegreeOfferA).HasMaxLength(50);

                entity.Property(e => e.DegreeOfferE).HasMaxLength(50);

                entity.Property(e => e.DepartmentNameA).HasMaxLength(50);

                entity.Property(e => e.DepartmentNameE).HasMaxLength(50);

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.FacultyNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Departments_Faculties");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.HasKey(e => e.FacultyNumber);

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.FacultyNameA).HasMaxLength(50);

                entity.Property(e => e.FacultyNameE).HasMaxLength(50);
            });


            modelBuilder.Entity<Announcement>(entity =>
                      {
                          entity.HasKey(e => e.Id);
                          entity.Property(e => e.Id).ValueGeneratedOnAdd();

                          entity.Property(e => e.Title).IsRequired();
                          entity.Property(e => e.Description).IsRequired();
                          entity.Property(e => e.IsDisplayed).HasDefaultValue(false);

                          entity.HasOne(d => d.Faculty)
                   .WithMany(p => p.Announcements)
                   .HasForeignKey(d => d.FacultyNumber)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Announcements_Faculties");

                          entity.HasOne(d => d.Department)
                                            .WithMany(p => p.Announcements)
                                            .HasForeignKey(d => new { d.FacultyNumber, d.DepartmentNumber })
                                            .OnDelete(DeleteBehavior.ClientSetNull)
                                            .HasConstraintName("FK_Announcements_Departments");

                          entity.HasOne(d => d.Batch)
                                            .WithMany(p => p.Announcements)
                                            .HasForeignKey(d => d.BatchId)
                                            .OnDelete(DeleteBehavior.ClientSetNull)
                                            .HasConstraintName("FK_Announcements_Batches");


                          entity.HasOne(d => d.Program)
                                            .WithMany(p => p.Announcements)
                                            .HasForeignKey(d => d.ProgramId)
                                            .OnDelete(DeleteBehavior.ClientSetNull)
                                            .HasConstraintName("FK_Announcements_Program");


                      });


            modelBuilder.Entity<GetArchive>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetArchive");

                entity.Property(e => e.AddmissionDate).HasColumnType("datetime");

                entity.Property(e => e.AddmissionFormNo).HasMaxLength(10);

                entity.Property(e => e.Addmissiontype)
                    .HasMaxLength(50)
                    .HasColumnName("addmissiontype");

                entity.Property(e => e.Address).HasMaxLength(400);

                entity.Property(e => e.BatchDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BatchGradeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchGradeID");

                entity.Property(e => e.BatchId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchID");

                entity.Property(e => e.BatchSubjectId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchSubjectID");

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.CHours).HasColumnName("c_hours");

                entity.Property(e => e.CPoints).HasColumnName("c_points");

                entity.Property(e => e.CertificateType).HasMaxLength(20);

                entity.Property(e => e.Cgpa).HasColumnName("cgpa");

                entity.Property(e => e.DepartmentNameA).HasMaxLength(50);

                entity.Property(e => e.DepartmentNameE).HasMaxLength(50);

                entity.Property(e => e.DepartmentNumber).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FacultyNameA).HasMaxLength(50);

                entity.Property(e => e.FacultyNameE).HasMaxLength(50);

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(5);

                entity.Property(e => e.Gpa).HasColumnName("gpa");

                entity.Property(e => e.GraduationDate).HasColumnType("datetime");

                entity.Property(e => e.HomeLanguage).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NationalityE).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");

                entity.Property(e => e.ProgramNameA).HasMaxLength(15);

                entity.Property(e => e.ProgramNameE)
                    .HasMaxLength(50)
                    .HasColumnName("programNameE");

                entity.Property(e => e.ReGrade).HasMaxLength(50);

                entity.Property(e => e.ReGradeA).HasMaxLength(50);

                entity.Property(e => e.ReReGrade).HasMaxLength(50);

                entity.Property(e => e.ReReGradeA).HasMaxLength(50);

                entity.Property(e => e.RegistrationFees).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.SHours).HasColumnName("s_hours");

                entity.Property(e => e.SPoints).HasColumnName("s_points");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.StudentId)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("StudentID");

                entity.Property(e => e.StudentNameA).HasMaxLength(50);

                entity.Property(e => e.StudentNameE).HasMaxLength(50);

                entity.Property(e => e.StudentNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StudyFees).HasColumnType("numeric(24, 2)");

                entity.Property(e => e.SubjectCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SubjectGrade).HasMaxLength(5);

                entity.Property(e => e.SubjectGradeA).HasMaxLength(5);

                entity.Property(e => e.SubjectNameA).HasMaxLength(50);

                entity.Property(e => e.SubjectNameE).HasMaxLength(50);

                entity.Property(e => e.Weight).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<GetStdGrade>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetStdGrades");

                entity.Property(e => e.Grade).HasMaxLength(107);

                entity.Property(e => e.GradeA).HasMaxLength(107);

                entity.Property(e => e.StudentNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SubjectCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SubjectGrade).HasMaxLength(5);

                entity.Property(e => e.SubjectGradeA).HasMaxLength(5);
            });

            modelBuilder.Entity<Gradepoint>(entity =>
            {
                entity.HasNoKey();

                //entity.Property(e => e.BatchId)
                //    .HasColumnType("numeric(18, 0)")
                //    .HasColumnName("BatchID");

                entity.Property(e => e.CHours).HasColumnName("c_hours");
                //real
                entity.Property(e => e.CPoints).HasColumnName("c_points");

                entity.Property(e => e.Cgpa).HasColumnName("cgpa");
                //real
                entity.Property(e => e.Gpa).HasColumnName("gpa");

                //entity.Property(e => e.Pcgpa).HasColumnName("pcgpa").HasColumnType("real").HasPrecision(17, 3);

                //entity.Property(e => e.Ppcgpa).HasColumnName("ppcgpa");

                entity.Property(e => e.SHours).HasColumnName("s_hours");

                //modelBuilder.Entity<MyEntity>().Property(x => x.Double1).HasColumnType("decimal").HasPrecision(17, 3);
                //real
                entity.Property(e => e.SPoints).HasColumnName("s_points");

                entity.Property(e => e.Semester).HasColumnName("semester");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.StudentNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<MaxSemester>(entity =>
            {
                entity.HasKey(e => new { e.SemesterNo, e.FacultyNumber, e.ProgramId });

                entity.ToTable("MaxSemester");

                entity.Property(e => e.SemesterNo).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");

                entity.Property(e => e.TotalHours).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<Program>(entity =>
            {
                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ProgramID");

                entity.Property(e => e.ProgramNameA).HasMaxLength(15);

                entity.Property(e => e.ProgramNameE)
                    .HasMaxLength(50)
                    .HasColumnName("programNameE");
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => new { e.StudentNumber, e.Semester, e.SlipNumber });

                entity.ToTable("Registration");

                entity.Property(e => e.StudentNumber).HasMaxLength(50);

                entity.Property(e => e.Semester).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.SlipNumber)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CardFees)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Discount)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Feed)
                    .HasColumnType("numeric(18, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Note).HasMaxLength(400);

                entity.Property(e => e.PayedStudyFees)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RegCourses)
                    .HasColumnType("numeric(18, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationFees)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RegistrationStatus)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RegistrationTypeId)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("RegistrationTypeID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SlipId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SlipID");

                entity.Property(e => e.StudentId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("StudentID");

                entity.Property(e => e.StudyFees)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.StudentNumberNavigation)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.StudentNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_Students");
            });

            modelBuilder.Entity<RegistrationType>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.RegistrationType1)
                    .HasMaxLength(50)
                    .HasColumnName("RegistrationType");

                entity.Property(e => e.RegistrationTypeId).HasColumnName("RegistrationTypeID");
            });

            modelBuilder.Entity<Resulttest>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("resulttest");

                entity.Property(e => e.CHours).HasColumnName("c_Hours");

                entity.Property(e => e.CPoints).HasColumnName("c_Points");

                entity.Property(e => e.Cgpa).HasColumnName("CGPA");

                entity.Property(e => e.Gpa).HasColumnName("GPA");

                //entity.Property(e => e.OrederInResult).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Pcgpa).HasColumnName("PCGPA");

                entity.Property(e => e.Ppcgpa).HasColumnName("PPCGPA");

                entity.Property(e => e.SHours).HasColumnName("s_Hours");

                entity.Property(e => e.SPoints).HasColumnName("s_Points");

                entity.Property(e => e.Sb1)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB1");

                entity.Property(e => e.Sb2)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB2");

                entity.Property(e => e.Sb3)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB3");

                entity.Property(e => e.Sb4)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB4");

                entity.Property(e => e.Sb5)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB5");

                entity.Property(e => e.Sb6)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB6");

                entity.Property(e => e.Sb7)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB7");

                entity.Property(e => e.Sb8)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB8");

                entity.Property(e => e.Sb9)
                    .IsRequired()
                    .HasMaxLength(107)
                    .HasColumnName("SB9");

                entity.Property(e => e.Sbd1).HasColumnName("SBd1");

                entity.Property(e => e.Sbd2).HasColumnName("SBd2");

                entity.Property(e => e.Sbd3).HasColumnName("SBd3");

                entity.Property(e => e.Sbd4).HasColumnName("SBd4");

                entity.Property(e => e.Sbd5).HasColumnName("SBd5");

                entity.Property(e => e.Sbd6).HasColumnName("SBd6");

                entity.Property(e => e.Sbd7).HasColumnName("SBd7");

                entity.Property(e => e.Sbd8).HasColumnName("SBd8");

                entity.Property(e => e.Sbd9).HasColumnName("SBd9");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.StudentId)
                    .HasColumnType("numeric(38, 0)")
                    .HasColumnName("StudentID");

                entity.Property(e => e.StudentNameA).HasMaxLength(50);

                entity.Property(e => e.StudentNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("studentNumber");
            });

            modelBuilder.Entity<SemBeginFinish>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SemBeginFinish");

                entity.Property(e => e.BatchId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchID");

                entity.Property(e => e.BeginDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentNumber).HasMaxLength(50);

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.FinishDate).HasColumnType("datetime");

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");

                entity.Property(e => e.Semester).HasMaxLength(50);
            });

            modelBuilder.Entity<Semster>(entity =>
            {
                entity.HasKey(e => e.Semster1);

                entity.ToTable("Semster");

                entity.Property(e => e.Semster1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("semster");
            });

            modelBuilder.Entity<Signature>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("signature");

                entity.Property(e => e.DeanAffairsA)
                    .HasMaxLength(50)
                    .HasColumnName("dean_affairsA");

                entity.Property(e => e.DeanAffairsE)
                    .HasMaxLength(50)
                    .HasColumnName("dean_affairsE");

                entity.Property(e => e.DeanFacultyA)
                    .HasMaxLength(50)
                    .HasColumnName("dean_facultyA");

                entity.Property(e => e.DeanFacultyE)
                    .HasMaxLength(50)
                    .HasColumnName("dean_facultyE");

                entity.Property(e => e.ExamOfficerA).HasMaxLength(50);

                entity.Property(e => e.ExamOfficerE).HasMaxLength(50);

                entity.Property(e => e.FacultyNumber).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.RegNameA)
                    .HasMaxLength(50)
                    .HasColumnName("reg_nameA");

                entity.Property(e => e.RegNameE)
                    .HasMaxLength(50)
                    .HasColumnName("reg_nameE");
            });


            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentNumber);
                entity.Property(p => p.StudentId)
    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(p => p.StudentId)
   .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.StudentNumber).HasMaxLength(50);

                entity.Property(e => e.AddmissionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)/(1))/(2000))");

                entity.Property(e => e.AddmissionFormNo)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("((12345))");

                entity.Property(e => e.Addmissiontype)
                    .HasMaxLength(50)
                    .HasColumnName("addmissiontype");

                entity.Property(e => e.Address).HasMaxLength(400);

                entity.Property(e => e.BatchId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchID");

                entity.Property(e => e.Birthdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)/(1))/(2000))");

                entity.Property(e => e.CertificateType).HasMaxLength(20);

                entity.Property(e => e.CurrencyNo)
                    .HasColumnName("currencyNo")
                    .HasDefaultValueSql("((1))");


                entity.Property(e => e.DepartmentNumber).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.FirstSemster)
                    .HasColumnName("First_semster")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Gender)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GraduationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)/(1))/(2000))");

                entity.Property(e => e.HomeLanguage).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NationalityE).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(50);

                //entity.Property(e => e.OrederInResult)
                //    .HasColumnType("numeric(18, 0)")
                //    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");

                entity.Property(e => e.RegistrationFees)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                //entity.Property(e => e.RegistrationType).HasDefaultValueSql("((0))");

                entity.Property(e => e.Specialization)
                    .HasMaxLength(50)
                    .HasColumnName("specialization");

                entity.Property(e => e.SpecializationE)
                    .HasMaxLength(50)
                    .HasColumnName("specializationE");

                //entity.Property(e => e.StdPicture)
                //    .HasColumnType("image")
                //    .HasColumnName("Std_Picture");

                entity.Property(e => e.StudentId)
                    .HasColumnType("numeric(38, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("StudentID");

                entity.Property(e => e.StudentNameA).HasMaxLength(50);

                entity.Property(e => e.StudentNameE).HasMaxLength(50);

                entity.Property(e => e.StudentPercent)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Studentstatus)
                    .HasColumnName("studentstatus")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StudyFees)
                    .HasColumnType("numeric(24, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.BatchId)
                    .HasConstraintName("FK_Students_batches");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ProgramId)
                    .HasConstraintName("FK_Students_Programs");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => new { d.FacultyNumber, d.DepartmentNumber })
                    .HasConstraintName("FK_Students_Departments");
            });

            modelBuilder.Entity<StudentOutOfResult>(entity =>
            {
                entity.HasKey(e => new { e.StudentNumber, e.Semester });

                entity.ToTable("StudentOutOfResult");

                entity.Property(e => e.StudentNumber).HasMaxLength(50);

                entity.Property(e => e.Semester).HasColumnName("semester");
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                //Original
                //entity.HasKey(e => new { e.StudentNumber, e.SubjectCode, e.SubjectCodeId });
                entity.HasKey(e => new { e.StudentNumber, e.SubjectCode });

                entity.Property(e => e.StudentNumber).HasMaxLength(50);

                entity.Property(e => e.SubjectCode).HasMaxLength(50);

                //entity.Property(e => e.SubjectCodeId)
                //    .HasColumnType("numeric(18, 0)")
                //    .HasColumnName("SubjectCodeID");

                entity.Property(e => e.BatchId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchID");

                entity.Property(e => e.Degree).HasDefaultValueSql("((0))");

                entity.Property(e => e.GradeCounter).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.GradeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("GradeID");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ReGrade).HasMaxLength(50);

                entity.Property(e => e.ReGradeA).HasMaxLength(50);

                entity.Property(e => e.ReReGrade).HasMaxLength(50);

                entity.Property(e => e.ReReGradeA).HasMaxLength(50);

                entity.Property(e => e.Register).HasDefaultValueSql("((0))");

                entity.Property(e => e.SubjectGrade).HasMaxLength(5);

                entity.Property(e => e.SubjectGradeA).HasMaxLength(5);

                entity.Property(e => e.SubjectHour).HasDefaultValueSql("((0))");

                entity.Property(e => e.Weight)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.StudentNumberNavigation)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.StudentNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentSubjects_Students");

                entity.HasOne(d => d.SubjectCodeNavigation)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.SubjectCode)
                    .HasConstraintName("FK_StudentSubjects_Subjects");
            });

            modelBuilder.Entity<Studentstemp>(entity =>
            {
                entity.HasKey(e => e.Serial)
                    .HasName("PK_studentstemp");

                entity.ToTable("Studentstemp");

                entity.Property(e => e.Serial)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("serial");

                entity.Property(e => e.ApplicationNumber).HasMaxLength(16);

                entity.Property(e => e.BatchId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BatchID");

                entity.Property(e => e.DepartmentNumber).HasMaxLength(50);

                entity.Property(e => e.Facultynumber).HasMaxLength(50);

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");

                entity.Property(e => e.Proiority).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.StudentNameA).HasMaxLength(50);

                entity.Property(e => e.StudentNumber).HasMaxLength(50);

                entity.Property(e => e.StudyFees).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.SubjectCode);

                entity.Property(e => e.SubjectCode)
                    .HasColumnType("nvarchar(50)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SubjectCode");

                entity.Property(e => e.DepartmentNumber)
                    .HasMaxLength(50)
                    .HasColumnName("departmentNumber");

                entity.Property(e => e.FacultyNumber)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("facultyNumber");

                //entity.Property(e => e.OrederInResult)
                //    .HasColumnType("numeric(18, 0)")
                //    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

                //entity.Property(e => e.Required).HasColumnName("required");

                entity.Property(e => e.SubjectCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SubjectNameA).HasMaxLength(50);

                entity.Property(e => e.SubjectNameE).HasMaxLength(50);
            });

            modelBuilder.Entity<SubjectMaxDegree>(entity =>
            {
                entity.HasKey(e => new { e.SubjectCode, e.Semester, e.BatchDescription, e.ProgramId });

                entity.ToTable("SubjectMaxDegree");

                entity.Property(e => e.SubjectCode).HasMaxLength(50);

                entity.Property(e => e.Semester).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.BatchDescription).HasMaxLength(50);

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");

                entity.Property(e => e.MaxDegree).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<SubjectType>(entity =>
            {
                entity.Property(e => e.SubjectTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SubjectTypeID");

                entity.Property(e => e.SubjectTypeA).HasMaxLength(50);

                entity.Property(e => e.SubjectTypeE).HasMaxLength(50);

                entity.Property(e => e.TypeDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<Subjectsall>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("subjectsall");

                entity.Property(e => e.Facultynumber).HasMaxLength(3);

                entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

                entity.Property(e => e.Subjectcode).HasMaxLength(50);

                entity.Property(e => e.SubjectnameA)
                    .HasMaxLength(50)
                    .HasColumnName("subjectnameA");

                entity.Property(e => e.SubjectnameE)
                    .HasMaxLength(50)
                    .HasColumnName("subjectnameE");
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ArEnResult).HasDefaultValueSql("((0))");

                entity.Property(e => e.DblHoursInReGrade).HasDefaultValueSql("((0))");

                entity.Property(e => e.DblWieghtInReGrade).HasDefaultValueSql("((0))");

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ProgramID");
            });

            modelBuilder.Entity<TblArchive>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblArchive");

                entity.Property(e => e.AddmissionDate).HasMaxLength(50);

                entity.Property(e => e.AddmissionFormNo).HasMaxLength(50);

                entity.Property(e => e.AddmissionType).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.BatchDescription).HasMaxLength(50);

                entity.Property(e => e.BatchGradeId)
                    .HasMaxLength(50)
                    .HasColumnName("BatchGradeID");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(50)
                    .HasColumnName("BatchID");

                entity.Property(e => e.BatchSubjectId)
                    .HasMaxLength(50)
                    .HasColumnName("BatchSubjectID");

                entity.Property(e => e.Birthdate).HasMaxLength(50);

                entity.Property(e => e.CHours)
                    .HasMaxLength(50)
                    .HasColumnName("c_Hours");

                entity.Property(e => e.CPoints)
                    .HasMaxLength(50)
                    .HasColumnName("c_Points");

                entity.Property(e => e.CertificateType).HasMaxLength(50);

                entity.Property(e => e.Cgpa)
                    .HasMaxLength(50)
                    .HasColumnName("CGPA");

                entity.Property(e => e.Degree).HasMaxLength(50);

                entity.Property(e => e.DepartmentNameA).HasMaxLength(50);

                entity.Property(e => e.DepartmentNameE).HasMaxLength(50);

                entity.Property(e => e.DepartmentNumber).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FacultyNameA).HasMaxLength(50);

                entity.Property(e => e.FacultyNameE).HasMaxLength(50);

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.Gpa)
                    .HasMaxLength(50)
                    .HasColumnName("GPA");

                entity.Property(e => e.GraduationDate).HasMaxLength(50);

                entity.Property(e => e.HomeLanguage).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NationalityE).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.ProgramId)
                    .HasMaxLength(50)
                    .HasColumnName("ProgramID");

                entity.Property(e => e.ProgramNameA).HasMaxLength(50);

                entity.Property(e => e.ProgramNameE).HasMaxLength(50);

                entity.Property(e => e.Register).HasMaxLength(50);

                entity.Property(e => e.RegistrationFees).HasMaxLength(50);

                entity.Property(e => e.Regrade).HasMaxLength(3);

                entity.Property(e => e.RegradeA).HasMaxLength(50);

                entity.Property(e => e.Reregrade).HasMaxLength(3);

                entity.Property(e => e.ReregradeA).HasMaxLength(50);

                entity.Property(e => e.SHours)
                    .HasMaxLength(50)
                    .HasColumnName("s_Hours");

                entity.Property(e => e.SPoints)
                    .HasMaxLength(50)
                    .HasColumnName("s_Points");

                entity.Property(e => e.Semester).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(50)
                    .HasColumnName("StudentID");

                entity.Property(e => e.StudentNameA).HasMaxLength(50);

                entity.Property(e => e.StudentNameE).HasMaxLength(50);

                entity.Property(e => e.StudentNumber).HasMaxLength(50);

                entity.Property(e => e.StudyFees).HasMaxLength(50);

                entity.Property(e => e.SubjectCode).HasMaxLength(50);

                entity.Property(e => e.SubjectGrade).HasMaxLength(3);

                entity.Property(e => e.SubjectGradeA).HasMaxLength(50);

                entity.Property(e => e.SubjectHour).HasMaxLength(50);

                entity.Property(e => e.SubjectNameA).HasMaxLength(50);

                entity.Property(e => e.SubjectNameE).HasMaxLength(50);

                entity.Property(e => e.Weight).HasMaxLength(50);
            });

            modelBuilder.Entity<TblGrade>(entity =>
            {
                entity.ToTable("tblGrade");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Grade)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.GradeA).HasMaxLength(5);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Weight).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.XMax)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("xMax");

                entity.Property(e => e.XMin)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("xMin");
            });

            modelBuilder.Entity<TblSection>(entity =>
            {
                entity.HasKey(e => new { e.SubjectCode, e.ProgramId, e.Semester, e.DepartmentNumber, e.FacultyNumber });

                entity.Property(e => e.SubjectCode).HasMaxLength(50);

                entity.Property(e => e.ProgramId)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ProgramID");

                entity.Property(e => e.DepartmentNumber).HasMaxLength(50);

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.SectionId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SectionID");

                entity.Property(e => e.SubjectCode)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("SubjectCode");

                entity.HasOne(d => d.SubjectCodeNavigation)
                    .WithMany(p => p.TblSections)
                    .HasForeignKey(d => d.SubjectCode)
                    .HasConstraintName("FK_TblSections_Subjects");
            });

            modelBuilder.Entity<TblStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("TblStatus");

                entity.Property(e => e.StatusId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("StatusID");

                entity.Property(e => e.Staus).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UserID");

                entity.Property(e => e.FacultyNumber).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserPass).HasMaxLength(50);

                entity.Property(e => e.UserTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("UserTypeID");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserTypes");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.UserTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("UserTypeID");

                entity.Property(e => e.UserTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Service>(
                e =>
                {
                    e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                    e.Property(e => e.Name).IsRequired();
                    e.Property(e => e.Fee).IsRequired();
                    e.Property(e => e.NameAr).IsRequired();
                    e.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
                }
                );

            modelBuilder.Entity<CardRequest>(
                e =>
                {

                    e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                }

                );
            modelBuilder.Entity<RequestStatus>(
               e =>
               {

                   e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                   e.Property(e => e.Name).IsRequired();
               }

               );

            modelBuilder.Entity<Payment>(e =>
            {

                e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                e.Property(e => e.CardRequestId).IsRequired(false);
                e.Property(e => e.CertificateRequestId).IsRequired(false);
                e.Property(e => e.TranscriptRequestId).IsRequired(false);
            });

            modelBuilder.Entity<Invoice>(e =>
            {

                e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            });


            modelBuilder.Entity<EnrollmentRequest>(
                e =>
                {

                    e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                }

                );
            modelBuilder.Entity<CertificateRequest>(
                e =>
                {

                    e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                }

                );
            modelBuilder.Entity<TranscriptRequest>(
                e =>
                {

                    e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

                }

                );

            modelBuilder.Entity<Country>(
              e =>
              {

                  e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                  //e.HasMany(e => e.States).WithOne(s => s.Country);

              }

              );

            modelBuilder.Entity<State>(
            e =>
            {

                e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                e.HasOne(s => s.Country).WithMany(e => e.States);
            }

            );

            modelBuilder.Entity<AdmissionType>(
           e =>
           {

               e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
           }

           );

            modelBuilder.Entity<StudentAttachment>(
          e =>
          {

              e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
              e.HasOne(e => e.Student).WithOne(s => s.Attachment).HasForeignKey<StudentAttachment>(s => s.StudentNumber);
          }

          );

            modelBuilder.Entity<Guardian>(
          e =>
          {

              e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
              e.HasOne(e => e.Student).WithOne(s => s.Guardian).HasForeignKey<Guardian>(s => s.StudentNumber);

          }

          );

            modelBuilder.Entity<OTPCode>(
         e =>
         {

             e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

         }

         );


            modelBuilder.Entity<Device>(
        e =>
        {

            e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            e.HasOne(e => e.APIUser).WithMany(a => a.Devices).HasForeignKey(e => e.APIUserId);

        }

        );


            modelBuilder.Entity<SMSAccess>(
       e =>
       {

           e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();


       }

       );



            modelBuilder.Entity<News>(
    e =>
    {

        e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        e.Property(e => e.Title).IsRequired();
        e.Property(e => e.Text).IsRequired();


    }

    );

            modelBuilder.Entity<PushNotification>(
  e =>
  {

      e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
      e.Property(e => e.Title).IsRequired();
      e.Property(e => e.Message).IsRequired();


  }

  );


            modelBuilder.Entity<Ticket>(
     e =>
     {

         e.Property(e => e.TicketId).IsRequired().ValueGeneratedOnAdd();
         e.Property(e => e.TicketTitle).IsRequired();
         e.Property(e => e.TicketDescription).IsRequired();
         e.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
         e.Property(e => e.UpdatedAt).HasDefaultValueSql("getdate()");
     }

     );


            modelBuilder.Entity<SystemClaim>(
     e =>
     {

         e.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
         e.Property(e => e.ClaimType).IsRequired();
         e.Property(e => e.ClaimValue).IsRequired();

     }

     );



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
