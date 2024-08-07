using DataAccess.Entities;

namespace CommonWeb.Models;

public class GetOrganisationPremisesModel
{
    public int PremiseId { get; set; } // Primary Key
    public string KnowAs { get; set; }
    public Address Address { get; set; }
    public bool PrimaryLocation { get; set; }
    public CompanyContact CompanyContact { get; set; }
}