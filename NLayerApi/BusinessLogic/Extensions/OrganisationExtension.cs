using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Extensions
{
    public static class OrganisationExtension
    {
        public static IQueryable<Organisation> TakeFirstTen(this IQueryable<Organisation> query)
        {
            return query.Take(10);
        }

        public static IQueryable<Organisation> Search(this IQueryable<Organisation> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.OrgName.ToLower().StartsWith(lowerCaseSearchTerm));
        }

        public static IQueryable<Organisation> Active(this IQueryable<Organisation> query, bool inActive)
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