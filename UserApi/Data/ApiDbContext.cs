using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models;

namespace UserApi.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<CardDetail> CardDetails { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<CardPrefix> CardPrefixes { get; set; }
        public DbSet<Bank> Banks { get; set; }
        //public DbSet<TransactionLog> TransactionLogs { get; set; } // درست کردن نام DbSet
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Store> Stores => Set<Store>();

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductBarcode> ProductBarcodes => Set<ProductBarcode>();

        public DbSet<PriceList> PriceLists => Set<PriceList>();
        public DbSet<ProductPrice> ProductPrices => Set<ProductPrice>();
        public DbSet<PriceChangeLog> PriceChangeLogs => Set<PriceChangeLog>();

        public DbSet<InventoryLot> InventoryLots => Set<InventoryLot>();

        //public DbSet<JobRequest> JobRequests { get; set; }
        //public DbSet<NotificationRequest> NotificationRequests { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Otp>()
            .HasIndex(o => new { o.PhoneNumber, o.Code })
            .IsUnique(); // جلوگیری از ایجاد OTPهای تکراری
            // User ↔ CardDetail (One-to-Many)
            modelBuilder.Entity<CardDetail>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.UserId)
                .IsRequired();

            // Bank ↔ CardDetail (One-to-Many)
            modelBuilder.Entity<CardDetail>()
                .HasOne(c => c.Bank)
                .WithMany(b => b.CardDetails)
                .HasForeignKey(c => c.BankId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade here


            modelBuilder.Entity<CardPrefix>()
                .HasOne(p => p.Bank)
                .WithMany(b => b.CardPrefixes)
                .HasForeignKey(p => p.BankId);
            // ارتباط User ↔ Contact (One-to-Many)
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.User)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.UserId)
                .IsRequired();
            modelBuilder.Entity<Bank>().HasData(
                 new Bank { Id = 1, Name = "بانک ملی ایران", SwiftCode = "BKAB12" },
                 new Bank { Id = 2, Name = "بانک سپه", SwiftCode = "BKB123" },
                 new Bank { Id = 3, Name = "بانک اقتصاد نوین", SwiftCode = "BKEN34" },
                 new Bank { Id = 4, Name = "بانک سامان", SwiftCode = "BKSAM56" },
                 new Bank { Id = 5, Name = "بانک ملت", SwiftCode = "BKML78" },
                 new Bank { Id = 6, Name = "بانک صادرات ایران", SwiftCode = "BKEX90" },
                 new Bank { Id = 7, Name = "بانک کشاورزی", SwiftCode = "BKAG12" },
                 new Bank { Id = 8, Name = "بانک مسکن", SwiftCode = "BKHS34" },
                 new Bank { Id = 9, Name = "بانک پاسارگاد", SwiftCode = "BKPS56" },
                 new Bank { Id = 10, Name = "بانک پارسیان", SwiftCode = "BKPR78" }
             );
            modelBuilder.Entity<CardPrefix>()
                .HasIndex(c => c.Prefix)
                    .IsUnique();
            modelBuilder.Entity<CardPrefix>().HasData(
              new CardPrefix { Id = 1, Prefix = "603799", BankName = "بانک ملی ایران", BankId = 1 },
              new CardPrefix { Id = 2, Prefix = "589210", BankName = "بانک سپه", BankId = 2 },
              new CardPrefix { Id = 3, Prefix = "627412", BankName = "بانک اقتصاد نوین", BankId = 3 },
              new CardPrefix { Id = 4, Prefix = "621986", BankName = "بانک سامان", BankId = 4 },
              new CardPrefix { Id = 5, Prefix = "610433", BankName = "بانک ملت", BankId = 5 },
              new CardPrefix { Id = 6, Prefix = "603769", BankName = "بانک صادرات ایران", BankId = 6 },
              new CardPrefix { Id = 7, Prefix = "639217", BankName = "بانک کشاورزی", BankId = 7 },
              new CardPrefix { Id = 8, Prefix = "628023", BankName = "بانک مسکن", BankId = 8 },
              new CardPrefix { Id = 9, Prefix = "502229", BankName = "بانک پاسارگاد", BankId = 9 },
              new CardPrefix { Id = 10, Prefix = "622106", BankName = "بانک پارسیان", BankId = 10 },
              new CardPrefix { Id = 11, Prefix = "639347", BankName = "بانک پاسارگاد", BankId = 9 },
              new CardPrefix { Id = 12, Prefix = "627884", BankName = "بانک پارسیان", BankId = 10 },
              new CardPrefix { Id = 13, Prefix = "505785", BankName = "بانک ایران‌زمین", BankId = 6 },
              new CardPrefix { Id = 14, Prefix = "606373", BankName = "بانک قرض‌الحسنه مهر ایران", BankId = 8 }
          );

            // تنظیمات بیشتر برای TransactionLog
            // =======================
            // Luxury Market Mapping
            // =======================

            // Category self-reference
            modelBuilder.Entity<Category>(b =>
            {
                b.ToTable("Categories");
                b.HasKey(x => x.CategoryId);

                b.Property(x => x.Name).HasMaxLength(150).IsRequired();

                b.HasOne(x => x.ParentCategory)
                 .WithMany(x => x.Children)
                 .HasForeignKey(x => x.ParentCategoryId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Brand>(b =>
            {
                b.ToTable("Brands");
                b.HasKey(x => x.BrandId);

                b.Property(x => x.Name).HasMaxLength(150).IsRequired();
                b.HasIndex(x => x.Name).IsUnique();

                b.Property(x => x.Country).HasMaxLength(80);
                b.Property(x => x.IsPremium).HasDefaultValue(true);
            });

            modelBuilder.Entity<Unit>(b =>
            {
                b.ToTable("Units");
                b.HasKey(x => x.UnitId);

                b.Property(x => x.Name).HasMaxLength(50).IsRequired();
                b.Property(x => x.Symbol).HasMaxLength(20);
            });

            modelBuilder.Entity<Supplier>(b =>
            {
                b.ToTable("Suppliers");
                b.HasKey(x => x.SupplierId);

                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Phone).HasMaxLength(30);
                b.Property(x => x.City).HasMaxLength(80).HasDefaultValue("Tehran");
                b.Property(x => x.Address).HasMaxLength(400);

                b.Property(x => x.IsImporter).HasDefaultValue(false);
            });

            modelBuilder.Entity<Store>(b =>
            {
                b.ToTable("Stores");
                b.HasKey(x => x.StoreId);

                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Area).HasMaxLength(120);
                b.Property(x => x.Address).HasMaxLength(400);
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.ToTable("Products");
                b.HasKey(x => x.ProductId);

                b.Property(x => x.SKU).HasMaxLength(40).IsRequired();
                b.HasIndex(x => x.SKU).IsUnique();

                b.Property(x => x.NameFa).HasMaxLength(250).IsRequired();
                b.Property(x => x.NameEn).HasMaxLength(250);

                b.Property(x => x.NetWeightValue).HasPrecision(12, 3);
                b.Property(x => x.NetWeightUnit).HasMaxLength(20);

                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.IsActive).HasDefaultValue(true);

                b.HasOne(x => x.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Brand)
                 .WithMany(br => br.Products)
                 .HasForeignKey(x => x.BrandId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.Unit)
                 .WithMany(u => u.Products)
                 .HasForeignKey(x => x.UnitId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductBarcode>(b =>
            {
                b.ToTable("ProductBarcodes");
                b.HasKey(x => x.BarcodeId);

                b.Property(x => x.Barcode).HasMaxLength(30).IsRequired();
                b.Property(x => x.BarcodeType).HasMaxLength(10).IsRequired();
                b.Property(x => x.IsPrimary).HasDefaultValue(false);

                b.HasIndex(x => new { x.ProductId, x.Barcode }).IsUnique();

                b.HasOne(x => x.Product)
                 .WithMany(p => p.Barcodes)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PriceList>(b =>
            {
                b.ToTable("PriceLists");
                b.HasKey(x => x.PriceListId);

                b.Property(x => x.Name).HasMaxLength(120).IsRequired();
                b.Property(x => x.Currency).HasMaxLength(10).HasDefaultValue("IRR");
                b.Property(x => x.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<ProductPrice>(b =>
            {
                b.ToTable("ProductPrices");
                b.HasKey(x => x.ProductPriceId);

                b.Property(x => x.PriceIRR).IsRequired();
                b.Property(x => x.ValidFrom).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => new { x.PriceListId, x.ProductId, x.ValidFrom }).IsUnique();

                b.HasOne(x => x.PriceList)
                 .WithMany(x => x.ProductPrices)
                 .HasForeignKey(x => x.PriceListId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Product)
                 .WithMany(x => x.Prices)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PriceChangeLog>(b =>
            {
                b.ToTable("PriceChangeLog");
                b.HasKey(x => x.LogId);

                b.Property(x => x.ChangedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.ChangedBy).HasMaxLength(120);
                b.Property(x => x.Reason).HasMaxLength(300);

                b.HasOne(x => x.Product)
                 .WithMany()
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.PriceList)
                 .WithMany()
                 .HasForeignKey(x => x.PriceListId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InventoryLot>(b =>
            {
                b.ToTable("InventoryLots");

                // ✅ این خط مشکل اصلی تو را حل می‌کند
                b.HasKey(x => x.LotId);

                b.Property(x => x.QtyOnHand).HasPrecision(18, 3);
                b.Property(x => x.ReceivedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.LotNumber).HasMaxLength(60);

                // DateOnly -> SQL type date
                b.Property(x => x.ExpireDate).HasColumnType("date");

                b.HasOne(x => x.Store)
                 .WithMany(s => s.InventoryLots)
                 .HasForeignKey(x => x.StoreId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Product)
                 .WithMany(p => p.InventoryLots)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Supplier)
                 .WithMany(su => su.InventoryLots)
                 .HasForeignKey(x => x.SupplierId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasIndex(x => x.ExpireDate).HasDatabaseName("IX_InventoryLots_ExpireDate");
            });
            modelBuilder.Entity<Fee>()
            .Property(x => x.FeeAmount)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Wallet>()
                .Property(x => x.Balance)
                .HasPrecision(18, 2);

        }

    }
}
