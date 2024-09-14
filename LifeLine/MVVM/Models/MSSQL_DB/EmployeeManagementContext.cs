using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class EmployeeManagementContext : DbContext
{
    public EmployeeManagementContext()
    {
    }

    public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessLevel> AccessLevels { get; set; }

    public virtual DbSet<Analysis> Analyses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentPatient> DocumentPatients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PositionList> PositionLists { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TimeTable> TimeTables { get; set; }

    public virtual DbSet<TypeDocument> TypeDocuments { get; set; }

    public virtual DbSet<TypeOfPersone> TypeOfPersones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=STYX;Database=EmployeeManagement;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLevel>(entity =>
        {
            entity.HasKey(e => e.IdAccessLevel);

            entity.ToTable("Access_level");

            entity.Property(e => e.IdAccessLevel).HasColumnName("id_access_level");
            entity.Property(e => e.AccessLevelName)
                .HasMaxLength(50)
                .HasColumnName("access_level_name");
        });

        modelBuilder.Entity<Analysis>(entity =>
        {
            entity.HasKey(e => e.IdAnalysis);

            entity.ToTable("Analysis");

            entity.Property(e => e.IdAnalysis).HasColumnName("id_analysis");
            entity.Property(e => e.AnalysisName).HasColumnName("analysis_name");
            entity.Property(e => e.DateTime)
                .HasMaxLength(50)
                .HasColumnName("date_time");
            entity.Property(e => e.IdPatient).HasColumnName("id_patient");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.ResultFile).HasColumnName("result_file");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.Analyses)
                .HasForeignKey(d => d.IdPatient)
                .HasConstraintName("FK_Analysis_Patient");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Analyses)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK_Analysis_Status");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.IdDepartment);

            entity.ToTable("Department");

            entity.Property(e => e.IdDepartment).HasColumnName("id_department");
            entity.Property(e => e.DepartmentName)
                .IsRequired()
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.IdDocument);

            entity.ToTable("Document");

            entity.Property(e => e.IdDocument).HasColumnName("id_document");
            entity.Property(e => e.DateOfIssue).HasColumnName("date_of_issue");
            entity.Property(e => e.DocumentFile).HasColumnName("document_file");
            entity.Property(e => e.DocumentImage).HasColumnName("document_image");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdTypeDocument).HasColumnName("id_type_document");
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .HasColumnName("number");
            entity.Property(e => e.PlaceOfIssue).HasColumnName("place_of_issue");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_Document_Employee");

            entity.HasOne(d => d.IdTypeDocumentNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdTypeDocument)
                .HasConstraintName("FK_Document_Type_document");
        });

        modelBuilder.Entity<DocumentPatient>(entity =>
        {
            entity.HasKey(e => e.IdDocumentPatient);

            entity.ToTable("Document_patient");

            entity.Property(e => e.IdDocumentPatient).HasColumnName("id_document_patient");
            entity.Property(e => e.DateOfIssue)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("date_of_issue");
            entity.Property(e => e.DocumentFile)
                .IsRequired()
                .HasColumnName("document_file");
            entity.Property(e => e.DocumentImage)
                .IsRequired()
                .HasColumnName("document_image");
            entity.Property(e => e.IdPatient).HasColumnName("id_patient");
            entity.Property(e => e.IdTypeDocument).HasColumnName("id_type_document");
            entity.Property(e => e.Number)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("number");
            entity.Property(e => e.PlaceOfIssue)
                .IsRequired()
                .HasColumnName("place_of_issue");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.DocumentPatients)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_patient_Patient");

            entity.HasOne(d => d.IdTypeDocumentNavigation).WithMany(p => p.DocumentPatients)
                .HasForeignKey(d => d.IdTypeDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_patient_Type_document");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee);

            entity.ToTable("Employee");

            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.IdPosition).HasColumnName("id_position");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("salary");
            entity.Property(e => e.SecondName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("second_name");

            entity.HasOne(d => d.IdGenderNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdGender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Gender");

            entity.HasOne(d => d.IdPositionNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdPosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Position");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.IdGender);

            entity.ToTable("Gender");

            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.GenderName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("gender_name");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient);

            entity.ToTable("Patient");

            entity.Property(e => e.IdPatient).HasColumnName("id_patient");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IdDepartment).HasColumnName("id_department");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.SecondName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("second_name");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Patients)
                .HasForeignKey(d => d.IdDepartment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_Department");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Patients)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_Employee");

            entity.HasOne(d => d.IdGenderNavigation).WithMany(p => p.Patients)
                .HasForeignKey(d => d.IdGender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_Gender");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.IdPosition);

            entity.ToTable("Position");

            entity.Property(e => e.IdPosition).HasColumnName("id_position");
            entity.Property(e => e.IdAccessLevel).HasColumnName("id_access_level");
            entity.Property(e => e.IdPositionList).HasColumnName("id_position_list");

            entity.HasOne(d => d.IdAccessLevelNavigation).WithMany(p => p.Positions)
                .HasForeignKey(d => d.IdAccessLevel)
                .HasConstraintName("FK_Position_Access_level");

            entity.HasOne(d => d.IdPositionListNavigation).WithMany(p => p.Positions)
                .HasForeignKey(d => d.IdPositionList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Position_Position_list");
        });

        modelBuilder.Entity<PositionList>(entity =>
        {
            entity.HasKey(e => e.IdPositionList);

            entity.ToTable("Position_list");

            entity.Property(e => e.IdPositionList).HasColumnName("id_position_list");
            entity.Property(e => e.IdDepartment).HasColumnName("id_department");
            entity.Property(e => e.PositionListName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("position_list_name");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.PositionLists)
                .HasForeignKey(d => d.IdDepartment)
                .HasConstraintName("FK_Position_list_Department");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.IdShift);

            entity.ToTable("Shift");

            entity.Property(e => e.IdShift).HasColumnName("id_shift");
            entity.Property(e => e.ShiftName)
                .HasMaxLength(50)
                .HasColumnName("shift_name");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus);

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<TimeTable>(entity =>
        {
            entity.HasKey(e => e.IdTimeTable);

            entity.ToTable("Time_table");

            entity.Property(e => e.IdTimeTable).HasColumnName("id_time_table");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdShift).HasColumnName("id_shift");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.TimeEnd)
                .HasMaxLength(5)
                .HasColumnName("time_end");
            entity.Property(e => e.TimeStart)
                .HasMaxLength(5)
                .HasColumnName("time_start");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TimeTables)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Time_table_Employee");

            entity.HasOne(d => d.IdShiftNavigation).WithMany(p => p.TimeTables)
                .HasForeignKey(d => d.IdShift)
                .HasConstraintName("FK_Time_table_Shift");
        });

        modelBuilder.Entity<TypeDocument>(entity =>
        {
            entity.HasKey(e => e.IdTypeDocument);

            entity.ToTable("Type_document");

            entity.Property(e => e.IdTypeDocument).HasColumnName("id_type_document");
            entity.Property(e => e.IdTypeOfPersone).HasColumnName("id_type_of_persone");
            entity.Property(e => e.TypeDocumentName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("type_document_name");

            entity.HasOne(d => d.IdTypeOfPersoneNavigation).WithMany(p => p.TypeDocuments)
                .HasForeignKey(d => d.IdTypeOfPersone)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Type_document_Type_of_persone");
        });

        modelBuilder.Entity<TypeOfPersone>(entity =>
        {
            entity.HasKey(e => e.IdTypeOfPersone);

            entity.ToTable("Type_of_persone");

            entity.Property(e => e.IdTypeOfPersone).HasColumnName("id_type_of_persone");
            entity.Property(e => e.TypeOfPersoneName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("type_of_persone_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
