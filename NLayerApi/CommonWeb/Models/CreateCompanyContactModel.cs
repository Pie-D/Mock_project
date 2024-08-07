using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CreateCompanyContactModel
    {
        public string? PhoneNumber { get; set; }

        public string? Fax { get; set; }

        public string? Email { get; set; }

        public string WebAddress { get; set; }

        public string CharityNumber { get; set; }

        public string CompanyNumber { get; set; }

        public string TypeOfBusiness { get; set; }

        public string SICCode { get; set; }

        public string? FullDescription { get; set; }
    }
}
