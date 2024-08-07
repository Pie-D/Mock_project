using DataAccess.Entities;

namespace BusinessLayer.Extensions
{
    public static class TeamExtension
    {
        public static IQueryable<Team> TakeFirstTen(this IQueryable<Team> query)
        {
            return query.Take(10);
        }

        public static IQueryable<Team> Search(this IQueryable<Team> query, string searchTerm)
        {
            
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().StartsWith(lowerCaseSearchTerm));
            
        }

        public static IQueryable<Team> Active(this IQueryable<Team> query, bool inActive)
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
