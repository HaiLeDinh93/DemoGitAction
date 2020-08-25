using System;
using CarMD.Fleet.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarMD.Fleet.Repository.EntityFramework
{
    public partial class CarMDShellContext : DbContext
    {
        public CarMDShellContext()
        {
        }

        public CarMDShellContext(DbContextOptions<CarMDShellContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Consumer> Consumer { get; set; }
        public virtual DbSet<Coupon> Coupon { get; set; }
        public virtual DbSet<CouponApply> CouponApply { get; set; }
        public virtual DbSet<FeedBack> FeedBack { get; set; }
        public virtual DbSet<Kiosk> Kiosk { get; set; }
        public virtual DbSet<KioskPing> KioskPing { get; set; }
        public virtual DbSet<LoggingError> LoggingError { get; set; }
        public virtual DbSet<LoggingTime> LoggingTime { get; set; }
        public virtual DbSet<MaintenanceGroup> MaintenanceGroup { get; set; }
        public virtual DbSet<MaintenanceMapper> MaintenanceMapper { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportDtc> ReportDtc { get; set; }
        public virtual DbSet<ReportDtcDefinition> ReportDtcDefinition { get; set; }
        public virtual DbSet<ReportFix> ReportFix { get; set; }
        public virtual DbSet<ReportFixPart> ReportFixPart { get; set; }
        public virtual DbSet<ReportModule> ReportModule { get; set; }
        public virtual DbSet<ReportModuleDtc> ReportModuleDtc { get; set; }
        public virtual DbSet<ReportPredictiveDiagnostic> ReportPredictiveDiagnostic { get; set; }
        public virtual DbSet<ReportPredictiveDiagnosticFixPart> ReportPredictiveDiagnosticFixPart { get; set; }
        public virtual DbSet<ReportRecall> ReportRecall { get; set; }
        public virtual DbSet<ReportScheduledMaintenance> ReportScheduledMaintenance { get; set; }
        public virtual DbSet<ReportScheduledMaintenanceFixPart> ReportScheduledMaintenanceFixPart { get; set; }
        public virtual DbSet<ReportTemplate> ReportTemplate { get; set; }
        public virtual DbSet<ReportUnScheduledMaintenance> ReportUnScheduledMaintenance { get; set; }
        public virtual DbSet<ReportUnScheduledMaintenanceFixPart> ReportUnScheduledMaintenanceFixPart { get; set; }
        public virtual DbSet<ReportWarranty> ReportWarranty { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }
        public virtual DbSet<ViewCurrentReportNumber> ViewCurrentReportNumber { get; set; }
        public virtual DbSet<VimeoSetting> VimeoSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HashPassword)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastActiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastDeactiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastUpdatedBy).HasMaxLength(50);

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.MobilePhone)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Permission).IsRequired();

                entity.Property(e => e.TempPassword).HasMaxLength(50);

                entity.Property(e => e.TimeZone).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Consumer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.HashPassword)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Logo).HasMaxLength(200);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.TempPassword).HasMaxLength(50);

                entity.Property(e => e.TimeZone)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdditionalFinePrint).HasMaxLength(1000);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Logo).HasMaxLength(200);

                entity.Property(e => e.MaintenanceMapperIds).HasMaxLength(500);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.Coupon)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK_Coupon_Kiosk");
            });

            modelBuilder.Entity<CouponApply>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApplyTo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CouponCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.CouponApply)
                    .HasForeignKey<CouponApply>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouponApply_Coupon");
            });

            modelBuilder.Entity<FeedBack>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasMaxLength(2000);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerEmail).HasMaxLength(200);
            });

            modelBuilder.Entity<Kiosk>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Address1).HasMaxLength(200);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(20);

                entity.Property(e => e.Logo).HasMaxLength(100);

                entity.Property(e => e.MechanicUrl).HasMaxLength(500);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.ScannerId).HasMaxLength(50);

                entity.Property(e => e.EmailNotification).HasMaxLength(1000);
            });

            modelBuilder.Entity<KioskPing>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDateTimeUtc).HasColumnType("datetime");

                entity.Property(e => e.Status).IsRequired();

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.KioskPing)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK_KioskPing_Kiosk");
            });

            modelBuilder.Entity<LoggingError>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Imei).HasMaxLength(50);

                entity.Property(e => e.ToolFirmware).HasMaxLength(50);

                entity.Property(e => e.Vin).HasMaxLength(20);
            });

            modelBuilder.Entity<LoggingTime>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Imei)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.ToolFirmware).HasMaxLength(50);

                entity.Property(e => e.Vin).HasMaxLength(20);

                entity.Property(e => e.WorkOrderNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<MaintenanceGroup>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<MaintenanceMapper>(entity =>
            {
                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.MaintenanceName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.MaintenanceGroup)
                    .WithMany(p => p.MaintenanceMapper)
                    .HasForeignKey(d => d.MaintenanceGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaintenanceMapper_MaintenanceGroup");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Aaia).HasMaxLength(100);

                entity.Property(e => e.AbsCurrentCodes).HasMaxLength(500);

                entity.Property(e => e.AbsHistoryCodes).HasMaxLength(500);

                entity.Property(e => e.BannerImage).HasMaxLength(500);

                entity.Property(e => e.CreatedDateTimeUtc).HasColumnType("datetime");

                entity.Property(e => e.CurrentBatteryVoltage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DepreciationCost).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.EcmdtcsRaw).HasColumnName("ECMDTCsRaw");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EngineType).HasMaxLength(100);

                entity.Property(e => e.ErrorCodes).HasMaxLength(500);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FreezeFrame).HasColumnType("xml");

                entity.Property(e => e.FuelCost).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Imei)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InsuranceCost).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Lftire)
                    .HasColumnName("LFTire")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Lrtire)
                    .HasColumnName("LRTire")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaintenanceCost).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Make).HasMaxLength(100);

                entity.Property(e => e.Manufacture).HasMaxLength(100);

                entity.Property(e => e.Model).HasMaxLength(100);

                entity.Property(e => e.Monitor).HasColumnType("xml");

                entity.Property(e => e.MonitorFile).HasMaxLength(500);

                entity.Property(e => e.Obd1CurrentCodes).HasMaxLength(500);

                entity.Property(e => e.Obd1HistoryCodes).HasMaxLength(500);

                entity.Property(e => e.OpportunityDate).HasColumnType("datetime");

                entity.Property(e => e.OriginalBatteryVoltage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OriginalTirePressure).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PendingErrorCodes).HasMaxLength(500);

                entity.Property(e => e.PermanentCodes).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.PrimaryErrorCode).HasMaxLength(10);

                entity.Property(e => e.RepairCost).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Rftire)
                    .HasColumnName("RFTire")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rrtire)
                    .HasColumnName("RRTire")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SecondaryErrorCodes).HasMaxLength(500);

                entity.Property(e => e.SrsCurrentCodes).HasMaxLength(500);

                entity.Property(e => e.SrsHistoryCodes).HasMaxLength(500);

                entity.Property(e => e.StoredErrorCodes).HasMaxLength(500);

                entity.Property(e => e.TcmdtcsRaw).HasColumnName("TCMDTCsRaw");

                entity.Property(e => e.TirePressureUnit).HasMaxLength(10);

                entity.Property(e => e.ToolFirmware).HasMaxLength(50);

                entity.Property(e => e.ToolLedstatus).HasColumnName("ToolLEDStatus");

                entity.Property(e => e.TotalCostToOwn).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Transmission).HasMaxLength(100);

                entity.Property(e => e.TrimLevel).HasMaxLength(100);

                entity.Property(e => e.Vin)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.WorkOrderNumber).HasMaxLength(50);

                entity.Property(e => e.Year).HasMaxLength(10);
            });

            modelBuilder.Entity<ReportDtc>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportDtc)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ReportDtc_Report");
            });

            modelBuilder.Entity<ReportDtcDefinition>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BodyCode).HasMaxLength(50);

                entity.Property(e => e.EngineType).HasMaxLength(100);

                entity.Property(e => e.LaymansTermEffectOnVehicle).HasMaxLength(1000);

                entity.Property(e => e.LaymansTermResponsibleComponentOrSystem).HasMaxLength(1000);

                entity.Property(e => e.LaymansTermSeverityLevelDefinition).HasMaxLength(1000);

                entity.Property(e => e.LaymansTermWhyItsImportant).HasMaxLength(1000);

                entity.Property(e => e.LaymansTermsTitle).HasMaxLength(500);

                entity.Property(e => e.MessageIndicatorLampFile).HasMaxLength(50);

                entity.Property(e => e.MessageIndicatorLampFileUrl).HasMaxLength(500);

                entity.Property(e => e.MonitorFile).HasMaxLength(50);

                entity.Property(e => e.MonitorFileUrl).HasMaxLength(500);

                entity.Property(e => e.MonitorType).HasMaxLength(50);

                entity.Property(e => e.PassiveAntiTheftIndicatorLampFile).HasMaxLength(50);

                entity.Property(e => e.PassiveAntiTheftIndicatorLampFileUrl).HasMaxLength(500);

                entity.Property(e => e.ServiceThrottleSoonIndicatorLampFile).HasMaxLength(50);

                entity.Property(e => e.ServiceThrottleSoonIndicatorLampFileUrl).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.TransmissionControlIndicatorLampFile).HasMaxLength(50);

                entity.Property(e => e.TransmissionControlIndicatorLampFileUrl).HasMaxLength(500);

                entity.Property(e => e.Trips).HasMaxLength(50);

                entity.HasOne(d => d.ReportDtc)
                    .WithMany(p => p.ReportDtcDefinition)
                    .HasForeignKey(d => d.ReportDtcId)
                    .HasConstraintName("FK_ReportDtcDefinition_ReportDtc");
            });

            modelBuilder.Entity<ReportFix>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdditionalCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ErrorCode).HasMaxLength(500);

                entity.Property(e => e.LaborCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborHours).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.PartsCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PredictiveDiagnosticPercentInMileage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ReportDtc)
                    .WithMany(p => p.ReportFix)
                    .HasForeignKey(d => d.ReportDtcId)
                    .HasConstraintName("FK_ReportFix_ReportDtc");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportFix)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportFix_Report");
            });

            modelBuilder.Entity<ReportFixPart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ManufacturerName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PartNumber).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ReportFix)
                    .WithMany(p => p.ReportFixPart)
                    .HasForeignKey(d => d.ReportFixId)
                    .HasConstraintName("FK_ReportFixPart_ReportFix");
            });

            modelBuilder.Entity<ReportModule>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ModuleName).HasMaxLength(500);

                entity.Property(e => e.SubSystem).HasMaxLength(500);

                entity.Property(e => e.System).HasMaxLength(500);

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportModule)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ReportModule_Report");
            });

            modelBuilder.Entity<ReportModuleDtc>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Def).HasMaxLength(500);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.ReportModule)
                    .WithMany(p => p.ReportModuleDtc)
                    .HasForeignKey(d => d.ReportModuleId)
                    .HasConstraintName("FK_ReportModuleDtc_ReportModule");
            });

            modelBuilder.Entity<ReportPredictiveDiagnostic>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdditionalCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ErrorCode).HasMaxLength(500);

                entity.Property(e => e.LaborCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborHours).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.PartsCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PredictiveDiagnosticPercentInMileage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportPredictiveDiagnostic)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ReportPredictiveDiagnostic_ReportPredictiveDiagnostic");
            });

            modelBuilder.Entity<ReportPredictiveDiagnosticFixPart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ManufacturerName).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.PartNumber).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ReportPredictiveDiagnostic)
                    .WithMany(p => p.ReportPredictiveDiagnosticFixPart)
                    .HasForeignKey(d => d.ReportPredictiveDiagnosticId)
                    .HasConstraintName("FK_ReportPredictiveDiagnosticFixPart_ReportPredictiveDiagnostic");
            });

            modelBuilder.Entity<ReportRecall>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CampaignNumber).HasMaxLength(100);

                entity.Property(e => e.DefectConsequence).HasMaxLength(2000);

                entity.Property(e => e.DefectCorrectiveAction).HasMaxLength(2000);

                entity.Property(e => e.RecallDate).HasMaxLength(100);

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportRecall)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ReportRecall_Report");
            });

            modelBuilder.Entity<ReportScheduledMaintenance>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdditionalCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Category).HasMaxLength(500);

                entity.Property(e => e.ErrorCode).HasMaxLength(500);

                entity.Property(e => e.FixName).HasMaxLength(500);

                entity.Property(e => e.LaborCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborHours).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.PartsCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PredictiveDiagnosticPercentInMileage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportScheduledMaintenance)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ReportScheduledMaintenance_ReportScheduledMaintenance");
            });

            modelBuilder.Entity<ReportScheduledMaintenanceFixPart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ManufacturerName).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.PartNumber).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ReportScheduledMaintenance)
                    .WithMany(p => p.ReportScheduledMaintenanceFixPart)
                    .HasForeignKey(d => d.ReportScheduledMaintenanceId)
                    .HasConstraintName("FK_ReportScheduledMaintenanceFixPart_ReportScheduledMaintenance");
            });

            modelBuilder.Entity<ReportTemplate>(entity =>
            {
                entity.Property(e => e.BannerImage).HasMaxLength(500);

                entity.Property(e => e.HiddenContent).HasMaxLength(1000);

                entity.Property(e => e.ReportHtml).IsRequired();

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReportUnScheduledMaintenance>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdditionalCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Category).HasMaxLength(500);

                entity.Property(e => e.ErrorCode).HasMaxLength(500);

                entity.Property(e => e.FixName).HasMaxLength(500);

                entity.Property(e => e.LaborCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborHours).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LaborRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.PartsCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PredictiveDiagnosticPercentInMileage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportUnScheduledMaintenance)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ReportUnScheduledMaintenance_Report");
            });

            modelBuilder.Entity<ReportUnScheduledMaintenanceFixPart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ManufacturerName).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.PartNumber).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ReportUnScheduledMaintenance)
                    .WithMany(p => p.ReportUnScheduledMaintenanceFixPart)
                    .HasForeignKey(d => d.ReportUnScheduledMaintenanceId)
                    .HasConstraintName("FK_ReportUnScheduledMaintenanceFixPart_ReportUnScheduledMaintenance");
            });

            modelBuilder.Entity<ReportWarranty>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DescriptionFormatted).HasMaxLength(1000);

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.Property(e => e.WarrantyTypeDescription).HasMaxLength(500);

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportWarranty)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_ReportWarranty_Report");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HashPassword)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastActiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastDeactiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastUpdatedBy).HasMaxLength(50);

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.MobilePhone)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TempPassword).HasMaxLength(50);

                entity.Property(e => e.TimeZone).HasMaxLength(50);

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.KioskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Kiosk");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Aaia).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EngineType).HasMaxLength(100);

                entity.Property(e => e.Make).HasMaxLength(100);

                entity.Property(e => e.Manufacture).HasMaxLength(100);

                entity.Property(e => e.Model).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Transmission).HasMaxLength(100);

                entity.Property(e => e.TrimLevel).HasMaxLength(100);

                entity.Property(e => e.Vin)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Year).HasMaxLength(10);

                entity.HasOne(d => d.Consumer)
                    .WithMany(p => p.Vehicle)
                    .HasForeignKey(d => d.ConsumerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicle_Consumer");
            });

            modelBuilder.Entity<ViewCurrentReportNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_CurrentReportNumber");
            });

            modelBuilder.Entity<VimeoSetting>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PictureId).HasMaxLength(50);

                entity.Property(e => e.VideoId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
