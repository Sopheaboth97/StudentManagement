using Microsoft.EntityFrameworkCore;

public class DB_connection : DbContext
{
    public DB_connection(DbContextOptions<DB_connection> options) : base(options) { }
    public DbSet<Student> Students { get; set; }
    public DbSet<Major> Majors { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<StudentFeePayment> StudentFeePayments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}