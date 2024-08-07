using CommonWeb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class UpdateAddressModel
    {
        public int AddressIdUpdate { get; set; }
        public AddressDto AddressDto { get; set; }
    }
}
