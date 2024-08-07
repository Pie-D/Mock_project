using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Extensions
{
    public static class DepartmentExtension
    {
        public static IQueryable<Department> TakeFirstTen(this IQueryable<Department> query)
        {
            return query.Take(10);
        }

        public static IQueryable<Department> Search(this IQueryable<Department> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().StartsWith(lowerCaseSearchTerm));
        }

        public static IQueryable<Department> Active(this IQueryable<Department> query, bool inActive)
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
