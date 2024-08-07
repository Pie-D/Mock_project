using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Extensions;
using BusinessLayer.Interfaces;
using CommonWeb.Dto;
using Common.Helper;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Common.Dto;

namespace BusinessLayer.Services
{
    public class SupportingMaterialService : ISupportingMaterialService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public SupportingMaterialService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> ActivateSupportingMaterialAsync(int id, string updatedBy)
        {
            var supportingMaterial = await _context.SupportingMaterials.FindAsync(id);

            if (supportingMaterial == null)
            {
                return false;
            }

            if (supportingMaterial.Status)
            {
                throw new InvalidOperationException("SupportingMaterial is already active.");
            }

            supportingMaterial.Status = true;
            supportingMaterial.UpdatedDate = DateTime.Now;
            supportingMaterial.UpdatedBy = updatedBy;
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<PagedList<SupportingMaterialDto>> GetSupportingMaterialAsync(SupportingMaterialParams supportingMaterialParams)
        {
            var query = _context.SupportingMaterials.Sort(supportingMaterialParams.OrderBy).AsQueryable();

            if (!supportingMaterialParams.InActive)
            {
                query = query.Active(supportingMaterialParams.InActive);
            }

            var supportingMaterialsQuery = query.ProjectTo<SupportingMaterialDto>(_mapper.ConfigurationProvider);

            return await PagedList<SupportingMaterialDto>.ToPagedList(supportingMaterialsQuery, supportingMaterialParams.PageNumber, supportingMaterialParams.PageSize);
        }

        public async Task<bool> UpdateSupportingMaterialAsync(int id, UpdateSupporttingMaterial updateSupporttingMaterial, string updatedBy)
        {
            var existingMaterial = await _context.SupportingMaterials.FirstOrDefaultAsync(m => m.MaterialId == id);

            if (existingMaterial == null)
            {
                return false;
            }

            // Update properties

            existingMaterial.Url = updateSupporttingMaterial.Url;
            existingMaterial.Description = updateSupporttingMaterial.Description;
            existingMaterial.Type = updateSupporttingMaterial.Type;
            existingMaterial.UpdatedBy = updatedBy;
            existingMaterial.UpdatedDate = DateTime.Now;

            _context.SupportingMaterials.Update(existingMaterial);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
