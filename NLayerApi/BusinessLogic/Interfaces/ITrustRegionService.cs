using CommonWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ITrustRegionService
    {
        Task<List<GetTrustRegionModel>> GetTrustRegions();

        Task<bool> AddTrustRegion(GetTrustRegionModel trustRegion); // only to add trustregion

        Task<bool> TrustRegionExists(int id);
    }
}
