using DoctorApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Linq;

namespace DoctorApplication.Classes
{
    public class StrLocalizer : IStringLocalizer
    {
        private readonly DoctorAppDbContext context;

        public StrLocalizer(DoctorAppDbContext context)
        {
            this.context = context;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new StrLocalizer(context);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCulture)
        {
            return context.resources
                .Include(r => r.culture)
                .Where(r => r.culture.name == CultureInfo.CurrentCulture.Name)
                .Select(r => new LocalizedString(r.key, r.value));
        }

        private string GetString(string name)
        {
            return context.resources
                .Include(r => r.culture)
                .Where(r => r.culture.name == CultureInfo.CurrentCulture.Name)
                .FirstOrDefault(r => r.key == name)?.value;
        }
    }
}
