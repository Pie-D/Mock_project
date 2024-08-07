using Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IProgrammeService
    {
        Task<IEnumerable<ProgrammeDto>> GetAllProgramme();

    }
}
