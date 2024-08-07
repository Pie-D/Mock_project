using CommonWeb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class PremiseDto
    {
        public int PremiseId { get; set; }           // Primary Key
        public string LocationName { get; set; }     // Premises Name
        public bool PrimaryLocation { get; set; }    // Primary
        public int AddressId { get; set; }
        public AddressDto Address { get; set; }      // Address Line
        public int ContactId { get; set; }
        public ServiceDto Service { get; set; }
        public CompanyContactDto CompanyContact { get; set; }      // use Contact.OfficePhone // Contact.MobilePhone
    }
}
