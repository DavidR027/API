using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Utility;

namespace API.Contexts;
public class BookingManagementDbContext : DbContext
{
    public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : base(options)
    {

    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder Builder)
    {
        base.OnModelCreating(Builder);

        Builder.Entity<Role>().HasData(new Role
        {
            Guid = Guid.Parse("e6314770-b685-4d19-8d42-a205c3ffe512"),
            Name = nameof(RoleLevel.User),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        },
        new Role
        {
            Guid = Guid.Parse("77d1b64c-baf4-49ad-6481-08db60bf173e"),
            Name = nameof(RoleLevel.Manager),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        },
        new Role
        {
            Guid = Guid.Parse("3a2877dd-fa5e-47e9-6482-08db60bf173e"),
            Name = nameof(RoleLevel.Admin),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        });

        Builder.Entity<Employee>().HasIndex(e => new
        {
            e.NIK, e.Email, e.PhoneNumber
        }).IsUnique();

        //Relation between Educatiton and University (One to Many)
        Builder.Entity<Education>().
            HasOne(u => u.University).
            WithMany(e => e.Educations).
            HasForeignKey(e => e.UniversityGuid);

        //Relation between Educatiton and Employee (One to One)
        Builder.Entity<Education>().
            HasOne(e => e.Employee).
            WithOne(e => e.Education).
            HasForeignKey<Education>(e => e.Guid);

        //Relation between Account and Employee (One to One)
        Builder.Entity<Account>().
            HasOne(e => e.Employee).
            WithOne(a => a.Account).
            HasForeignKey<Account>(a => a.Guid);

        //Relation between Account and Account Role (Many to One)
        Builder.Entity<Account>().
            HasMany(ar => ar.AccountRoles).
            WithOne(a => a.Account).
            HasForeignKey(ar => ar.AccountGuid);

        //Relation between Role and Account Role (Many to One)
        Builder.Entity<Role>().
            HasMany(ar => ar.AccountRoles).
            WithOne(r => r.Role).
            HasForeignKey(ar => ar.RoleGuid);

        //Relation between Booking and Employee (One to Many)
        Builder.Entity<Booking>().
            HasOne(e => e.Employee).
            WithMany(b => b.Bookings).
            HasForeignKey(b => b.EmployeeGuid);

        //Relation between Booking and Room (One to Many)
        Builder.Entity<Booking>().
            HasOne(r => r.Room).
            WithMany(b => b.Bookings).
            HasForeignKey(b => b.RoomGuid);

    }

}

