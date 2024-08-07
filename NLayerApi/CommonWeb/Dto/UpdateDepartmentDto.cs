using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class UpdateDepartmentDto
    {
        public int DepartmentId { get; set; } // Primary Key
        public int DirectorateId { get; set; } // Foreign Key
        public int ContactId { get; set; } // Foreign Key

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string? LeadContact { get; set; }
        public bool IsActive { get; set; }

        public int CompanyContactId { get; set; }

        public int AddressId { get; set; }
    }
}
