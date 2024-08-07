using CommonWeb.Models;
namespace BusinessLayer.Interfaces;

public interface IRegionService
{ 
    Task<List<GetOrganisationGORModel>> GetOrganisationGOR(int countyId);
}