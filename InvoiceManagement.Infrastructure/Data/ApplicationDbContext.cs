
using InvoiceManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(50);
            entity.Property(i => i.CustomerName).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Date).IsRequired();
            entity.Property(i => i.TotalAmount)
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0m);
            entity.HasMany(i => i.Items)
                  .WithOne(ii => ii.Invoice)
                  .HasForeignKey(ii => ii.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(ii => ii.Id);
            entity.Property(ii => ii.Description).IsRequired().HasMaxLength(200);
            entity.Property(ii => ii.Quantity).IsRequired();
            entity.Property(ii => ii.UnitPrice)
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0m);
        });
    }
}