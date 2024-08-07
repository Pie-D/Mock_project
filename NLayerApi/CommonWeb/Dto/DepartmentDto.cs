
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWeb.Dto
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; } // Primary Key
        [Required]
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public OrganisationDto Organisation { get; set; }
        public ContactDto Contact { get; set; }
        public bool IsActive { get; set; }
    }
}
