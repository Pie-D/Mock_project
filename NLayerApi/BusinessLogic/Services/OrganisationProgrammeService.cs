using AutoMapper;
using BusinessLayer.Interfaces;
using Common.Dto;
using Common.Models;
using CommonWeb.Dto;
using DataAccess;
using DataAccess.Entities;

namespace BusinessLayer.Services;

//public class OrganisationProgrammeService(DataContext dataContext, IMapper mapper) : IOrganisationProgrammeService
//{
public class OrganisationProgrammeService : IOrganisationProgrammeService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    public OrganisationProgrammeService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<OrganisationProgrammeDto>?> CreateOrganisationProgramme(CreateOrganisationProgrammeModel createOrganisationProgramme, string createdBy)
    {
        var organisationProgrammesDto =
            createOrganisationProgramme.OrganisationProgrammesDto;
        var organisationProgrammesMap = _mapper.Map<IEnumerable<OrganisationProgramme>>(organisationProgrammesDto);
        var organisationProgrammes = organisationProgrammesMap.ToList();
        foreach (var item in organisationProgrammes)
        {
            item.CreatedBy = createdBy;
            item.CreatedDate = DateTime.Now;
        }
        await _context.OrganisationProgrammes.AddRangeAsync(organisationProgrammes);
        var result = await _context.SaveChangesAsync() > 0;
        return result ? organisationProgrammesDto : null;
    }
}