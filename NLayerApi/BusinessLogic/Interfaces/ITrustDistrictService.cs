using CommonWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ITrustDistrictService
    {
        Task<List<GetTrustDistrictModel>> GetTrustDistrictByTrustRegionId(int id);
        Task<bool> AddTrustDistrict(GetTrustDistrictModel trustDistrict); //add trust district 

        Task<bool> TrustDistrictExist(int id);
    }
}
