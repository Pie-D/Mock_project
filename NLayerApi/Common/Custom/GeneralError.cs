using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Custom
{
    public sealed record GeneralError(string Error, string Description)
    {
        public static readonly GeneralError None = new(string.Empty, string.Empty);
    }
}
