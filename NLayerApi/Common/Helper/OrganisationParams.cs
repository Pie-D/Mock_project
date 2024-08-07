using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class OrganisationParams : PaginationParams
    {
        public bool FirstTenPart { get; set; } = false;
        public string? SearchTerm { get; set; }
        public bool InActive { get; set; }

    }
}
