using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class OrganisationReferenceData
    {
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }

        public int RefId { get; set; }
        public ReferenceData ReferenceData { get; set; }
    }
}
