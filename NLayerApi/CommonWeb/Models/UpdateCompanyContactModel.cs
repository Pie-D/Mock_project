using CommonWeb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class UpdateCompanyContactModel
    {
        public int CompanyContactIdUpdate { get; set; }
        public CompanyContactDto CompanyContactDto { get; set; }
    }
}
