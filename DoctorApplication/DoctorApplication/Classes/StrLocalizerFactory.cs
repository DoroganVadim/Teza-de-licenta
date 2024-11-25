using DoctorApplication.Models;
using DoctorApplication.Models.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace DoctorApplication.Classes
{
    public class StrLocalizerFactory : IStringLocalizerFactory
    {
        string connectionString;
        public StrLocalizerFactory(string connection)
        {
            connectionString = connection;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return CreateStringLocalizer();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return CreateStringLocalizer();
        }

        private IStringLocalizer CreateStringLocalizer()
        {
            DoctorAppDbContext context = new DoctorAppDbContext(
                new DbContextOptionsBuilder<DoctorAppDbContext>()
                .UseSqlServer(connectionString)
                .Options);
            if (!context.cultures.Any())
            {
                context.AddRange(
                    new Culture
                    {
                        name = "en",
                        resources = new List<Resource>()
                        {
                            new Resource { key = "Name", value = "Name" },
                            new Resource { key = "Surrname", value = "Surname" }
                        }
                    },
                    new Culture
                    {
                        name = "ro",
                        resources = new List<Resource>()
                        {
                            new Resource { key = "Name", value = "Nume" },
                            new Resource { key = "Surrname", value = "Prenume" }
                        }
                    }
                );
                context.SaveChanges();
            }
            return new StrLocalizer(context);
        }
    }
}
