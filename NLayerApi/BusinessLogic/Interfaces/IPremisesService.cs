using Common.Custom;
using Common.Models;

namespace BusinessLayer.Interfaces;

public interface IPremisesService
{
        Task<List<GetPremiseModel>> GetPremisesByOrgId(int id);
        Task<Result> UpdatePremise(UpdatePremiseModel? model);
        Task<Result> UpdatePremises(List<UpdatePremiseModel?> models);

        Task<bool> PremiseExists(int id);
    }