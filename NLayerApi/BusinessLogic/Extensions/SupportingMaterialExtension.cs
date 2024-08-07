using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Extensions
{
    public static class SupportingMaterialExtension
    {
        public static IQueryable<SupportingMaterial> Sort(this IQueryable<SupportingMaterial> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.CreatedDate);
            query = orderBy switch
            {
                "date" => query.OrderBy(p => p.CreatedDate),
                "dateDesc" => query.OrderByDescending(p => p.CreatedDate),
                _ => query.OrderBy(p => p.CreatedDate)
            };
            return query;
        }

        public static IQueryable<SupportingMaterial> Active(this IQueryable<SupportingMaterial> query, bool inActive)
        {
            if (inActive)
            {
                return query; // Return all organizations
            }
            else
            {
                return query.Where(p => p.Status == true); // Return only active organizations
            }
        }
    }
}
