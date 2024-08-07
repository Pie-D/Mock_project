using Common.Dto;
using CommonWeb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class UpdateDepartmentModel
    {
        public int DepartmentId { get; set; }
        public UpdateDepartmentDto? DepartmentDto { get; set; }
        public AddressDto? AddressDto { get; set; }
        public string? CopyAddressFrom { get; set; }

        public string? DepartmentFullDescription { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
    }
}
