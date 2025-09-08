using Microsoft.EntityFrameworkCore;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccountDTO>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(e => e.PhoneNumber).HasColumnName("Phone");
        });
    }

    public DbSet<SleepDTO> Sleep { get; set; }
    public DbSet<UserAccountDTO> UserAccounts { get; set; }
}
