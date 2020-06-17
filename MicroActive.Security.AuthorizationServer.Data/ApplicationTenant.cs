using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroActive.Security.AuthorizationServer.Data
{
    public class ApplicationTenant
    {
        public ApplicationTenant()
        {
            Id = Guid.NewGuid().ToString();
            ApplicationUsers = new List<ApplicationUser>();
        }

        public ApplicationTenant(string name) : this()
        {
            Name = name;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string BuildingNameNo { get; set; }
        public string StreetName { get; set; }
        public string TownCity { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
