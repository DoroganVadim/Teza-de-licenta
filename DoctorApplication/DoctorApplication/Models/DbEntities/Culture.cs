using Org.BouncyCastle.Asn1.Cmp;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DoctorApplication.Models.DbEntities
{
    public class Culture
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<Resource> resources { get; set; }
        public Culture()
        {
            resources = new Collection<Resource>();
        }
    }
}
