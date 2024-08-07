using AutoMapper;
using Common.Dto;
using Common.Models;
using CommonWeb.Dto;
using CommonWeb.Models;
using DataAccess.Entities;

namespace BusinessLayer.AutoMapperProfile;

public class BusinessLogicProfile : Profile
{
    public BusinessLogicProfile()
    {
        CreateMap<Organisation, OrganisationDto>();
        CreateMap<OrganisationDto, Organisation>();

        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();

        CreateMap<CompanyContact, CompanyContactDto>();
        CreateMap<CompanyContactDto, CompanyContact>();

        CreateMap<Organisation, CreateOrganisationDto>();
        CreateMap<CreateOrganisationDto, Organisation>();

        CreateMap<GetOrganisationPremisesModel, Premise>();
        CreateMap<Premise, GetOrganisationPremisesModel>();

        CreateMap<GetOrganisationGORModel, GovOfficeRegion>();
        CreateMap<GovOfficeRegion, GetOrganisationGORModel>();

        CreateMap<Directorate, DirectorateDto>();
        CreateMap<DirectorateDto, Directorate>();

        CreateMap<Directorate, CreateDirectorateDto>();
        CreateMap<CreateDirectorateDto, Directorate>();

        CreateMap<Directorate, UpdateDirectorateDto>();
        CreateMap<UpdateDirectorateDto, Directorate>();

        CreateMap<Department, DepartmentDto>();
        CreateMap<DepartmentDto, Department>();

        CreateMap<Team, TeamDto>();
        CreateMap<TeamDto, Team>();

        CreateMap<Contact, ContactDto>();
        CreateMap<ContactDto, Contact>();

        CreateMap<Team, CreateTeamDto>();
        CreateMap<CreateTeamDto, Team>();

        CreateMap<Department, CreateDepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();

        CreateMap<Department, UpdateDepartmentDto>();
        CreateMap<UpdateDepartmentDto, Department>();

        CreateMap<SupportingMaterial, SupportingMaterialDto>();
        CreateMap<SupportingMaterialDto, SupportingMaterial>();

        CreateMap<OrganisationProgramme, OrganisationProgrammeDto>();
        CreateMap<OrganisationProgrammeDto, OrganisationProgramme>();

        CreateMap<OrganisationService, OrganisationServiceDto>();
        CreateMap<OrganisationServiceDto, OrganisationService>();

        CreateMap<CompanyContact, CompanyContactDto>();
        CreateMap<CompanyContactDto, CompanyContact>();

        CreateMap<CompanyContact, CreateCompanyContactModel>();
        CreateMap<CreateCompanyContactModel, CompanyContact>();

        CreateMap<CreateCompanyContactModel, CompanyContactDto>();
        CreateMap<CompanyContactDto, CreateCompanyContactModel>();

        CreateMap<TrustDistrict, TrustDistrictDto>();
        CreateMap<TrustDistrictDto, TrustDistrict>();

        CreateMap<TrustRegion, TrustRegionDto>();
        CreateMap<TrustRegionDto, TrustRegion>();

        CreateMap<Service, ServiceDto>();
        CreateMap<ServiceDto, Service>();

        // CreateMap<GovOfficeRegion, GovOfficeRegionDto>();
        // CreateMap<GovOfficeRegionDto, GovOfficeRegion>();

        CreateMap<Premise, PremiseDto>();
        CreateMap<PremiseDto, Premise>();

        CreateMap<Town, TownDto>();
        CreateMap<TownDto, Town>();

        //For ViewModels
        //---------------------------------------*****------------------------------------
        CreateMap<GetGovOfficeRegionModel, GovOfficeRegion>();
        CreateMap<GovOfficeRegion, GetGovOfficeRegionModel>();

        CreateMap<TrustDistrict, GetTrustDistrictModel>();
        CreateMap<GetTrustDistrictModel, TrustDistrict>();

        CreateMap<TrustRegion, GetTrustRegionModel>().ForMember(dest => dest.TrustRegion,
            init => init.MapFrom(k => k));

        CreateMap<Premise, GetPremiseModel>()
            .ForMember(init => init.Premise, dest
                => dest.MapFrom(dest => dest));

    }
}
