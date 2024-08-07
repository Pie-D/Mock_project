using CommonWeb.Dto;
using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;

namespace BusinessLayer.Interfaces
{
    public interface ISupportingMaterialService
    {
        Task<PagedList<SupportingMaterialDto>> GetSupportingMaterialAsync(SupportingMaterialParams supportingMaterialParams);
        Task<bool> ActivateSupportingMaterialAsync(int id, string updatedBy);
        Task<bool> UpdateSupportingMaterialAsync(int id, UpdateSupporttingMaterial updateSupporttingMaterial, string updatedBy);
    }
}
