using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authorizationRoles.Models
{
    public class Contact
{
        public string OwnerID { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; } 
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ContactStatus Status { get; set; }

    }

    public enum ContactStatus
    {
        Submittet,
        Approved,
        Rejected
    }
}
