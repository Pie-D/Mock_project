using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Extensions
{
    public static class DirectorateExtension
    {
        public static IQueryable<Directorate> TakeFirstTen(this IQueryable<Directorate> query)
        {
            return query.Take(10);
        }

        public static IQueryable<Directorate> Search(this IQueryable<Directorate> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().StartsWith(lowerCaseSearchTerm));
        }

        public static IQueryable<Directorate> Active(this IQueryable<Directorate> query, bool inActive)
        {
            if (inActive)
            {
                return query; // Return all organizations
            }
            else
            {
                return query.Where(p => p.IsActive == true); // Return only active organizations
            }
        }
    }
}