using DoctorApplication.Models.DbEntities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;
using System.Text.RegularExpressions;

namespace DoctorApplication.Models
{
    public class DoctorAppDbContext : DbContext
    {
        public DoctorAppDbContext(DbContextOptions<DoctorAppDbContext> options) : base(options) {;}
        public DbSet<DbEntities.Account> accounts { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Culture> cultures { get; set; }
        public DbSet<DbEntities.Doctor> doctors { get; set; }
        public DbSet<DoctorSpecialitie> doctorSpecialities { get; set; }
        public DbSet<LogEvent> logEvents { get; set; }
        public DbSet<Resource> resources { get; set; }
    }
}
