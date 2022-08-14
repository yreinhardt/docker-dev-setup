using Microsoft.EntityFrameworkCore;

namespace basicapi.DataAccess;

public class BasicDbContext: DbContext
{

    public BasicDbContext() { }
    public BasicDbContext(DbContextOptions opts) : base(opts) { }
    public DbSet<RequestBodyModel> RequestBodyModel { get; set; }

    // override default behavior to snakecase table names
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentitTableNames(modelBuilder);
    }

    private static void SnakeCaseIdentitTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequestBodyModel>(b => { b.ToTable("model"); });
    }

}
