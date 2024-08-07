using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Custom
{
    public static class DocumentedErrors
    {
        public static readonly GeneralError InvalidOrgId = new("InputInvalid.OrgId", "Org Id Invalid");
        public static readonly GeneralError InvalidContactId = new("InputInvalid.ContactId", "Contact Id Invalid");
        public static readonly GeneralError InvalidAddressId = new("InputInvalid.OrgId", "Org Id Invalid");
        public static readonly GeneralError InvalidPremiseId = new("InputInvalid.PreId", "Premise Id Invalid");
        public static readonly GeneralError InvalidId = new("InputInvalid", "Input Invalid"); //General


        public static readonly GeneralError NullInvalid = new("InputNull", "Input is null");

        public static GeneralError UpdateFailedAtEntry(string name)
        {
            return new("UpdateFailed", $"Problem at {name}");
        }

    }
}
