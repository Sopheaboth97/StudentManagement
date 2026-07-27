using Microsoft.EntityFrameworkCore;

 public class DB_connection:DbContext{
    public DB_connection(DbContextOptions<DB_connection> options) : base(options){}
    public DbSet<Student> Students { get; set; }
    public DbSet<Majors> Majors { get; set; }
 
    public DbSet<Groups> Groups {get; set;}

    public DbSet<Teachers> Teachers {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }
}