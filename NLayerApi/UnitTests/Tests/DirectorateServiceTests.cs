//using AutoMapper;
//using BusinessLayer.Interfaces;
//using BusinessLayer.Services;
//using CommonWeb.Dto;
//using DataAccess.Entities;
//using DataAccess;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using BusinessLayer.AutoMapperProfile;
//namespace UnitTests.Tests;
//public class DirectorateServiceTests
//{
//    private readonly DirectorateService _service;
//    private readonly Mock<IAddressService> _addressServiceMock;
//    private readonly Mock<ICompanyContactService> _companyContactServiceMock;
//    private readonly DataContext _context;
//    private readonly IMapper _mapper;

//    public DirectorateServiceTests()
//    {
//        var options = new DbContextOptionsBuilder<DataContext>()
//    .UseInMemoryDatabase(databaseName: "FinalProject10")
//    .Options;


//        _context = new DataContext(options);
//        _addressServiceMock = new Mock<IAddressService>();
//        _companyContactServiceMock = new Mock<ICompanyContactService>();

//        var config = new MapperConfiguration(cfg => {
//            cfg.AddProfile<BusinessLogicProfile>(); // Assume you have a mapping profile
//        });
//        _mapper = config.CreateMapper();

//        _service = new DirectorateService(_context, _mapper, _companyContactServiceMock.Object, _addressServiceMock.Object);
//    }

//    [Fact]
//    public async Task AddDirectorateAsync_CreatesDirectorate()
//    {
//        // Arrange
//        var organisationId = 4;
//        var contactId = 1;

//        var createDirectorate = new CreateDirectorateDto
//        {
//            Name = "New Directorate",
//            ShortDescription = "Test",
//            LeadContact = "String",
//            Address = new AddressDto {
//                Address1 = "Dai",
//                Address2 = "Phung",
//                Address3 = "Lang",
//                PostCode = "56754",
//                City="Phung Town",
//                TownId=1
            
//            },
//            CompanyContact = new CompanyContactDto
//            {
//                PhoneNumber = "1234567890",
//                Email = "email@example.com",
//                FullDescription = "Full Description",
//                TypeOfBusiness = "Retail",
//                SICCode = "554332",
//                WebAddress = "http://example.com"
//            },
//            GetAddressFrom = "none"
//        };

//        // Mock dependencies
//        _addressServiceMock
//            .Setup(x => x.HandleAddressAsync(It.IsAny<AddressDto>(), It.IsAny<string>()))
//            .ReturnsAsync(new Address { PostCode = "56754" });

//        _companyContactServiceMock
//            .Setup(x => x.HandleCompanyContactAsync(It.IsAny<CompanyContactDto>(), It.IsAny<string>()))
//            .ReturnsAsync(new CompanyContact
//            {
//                Email = "email@example.com",
//                TypeOfBusiness = "Retail",
//                SICCode = "554332",
//                WebAddress = "http://example.com"
//            });

//        // Act
//        var result = await _service.AddDirectorateAsync(organisationId, contactId, createDirectorate, "admin");

//        // Assert
//        Assert.True(result);
//        var directorate = await _context.Directorates.FirstOrDefaultAsync(d => d.Name == "New Directorate");
//        Assert.NotNull(directorate);
//        Assert.Equal("New Directorate", directorate.Name);
//        Assert.Equal("56754", directorate.Address.PostCode);
//        Assert.Equal("email@example.com", directorate.CompanyContact.Email);
//        Assert.Equal("Retail", directorate.CompanyContact.TypeOfBusiness);
//    }
//    private void InitializeDatabase()
//    {
//        // Thêm dữ liệu
//        _context.Organisations.Add(new Organisation { OrganisationId = 4 });
//        _context.Contacts.Add(new Contact { ContactId = 1 });
//        _context.SaveChanges();

//        // Xác nhận dữ liệu
//        var organisations = _context.Organisations.ToList();
//        var contacts = _context.Contacts.ToList();

//        Console.WriteLine("Organisations:");
//        foreach (var org in organisations)
//        {
//            Console.WriteLine($"ID: {org.OrganisationId}");
//        }

//        Console.WriteLine("Contacts:");
//        foreach (var contact in contacts)
//        {
//            Console.WriteLine($"ID: {contact.ContactId}");
//        }
//    }

//    //[Fact]
//    //public async Task AddDirectorateAsync_CreatesDirectorate()
//    //{
//    //    // Arrange
//    //    //var organisation = Organisation { OrganisationId = 4};
//    //    //var contact = Contact { ContactId = 1 };
//    //    //_context.Organisations.Add(organisation);
//    //    //_context.Contacts.Add(contact);
//    //    //await _context.SaveChangesAsync();
//    //    var organisationId = 4;
//    //    var contactId = 1;

//    //    var createDirectorate = new CreateDirectorateDto
//    //    {
//    //        Name = "New Directorate",
//    //        ShortDescription = "Test",
//    //        LeadContact="String",
//    //        Address = new AddressDto { PostCode = "12345" },
//    //        CompanyContact = new CompanyContactDto
//    //        {
//    //            PhoneNumber = "1234567890",
//    //            Email = "email@example.com",
//    //            FullDescription = "Full Description"
//    //        },
//    //        GetAddressFrom = "none"
//    //    };

//    //    _addressServiceMock.Setup(x => x.HandleAddressAsync(It.IsAny<AddressDto>(), It.IsAny<string>())).ReturnsAsync(new Address { PostCode = "12345" });
//    //    _companyContactServiceMock.Setup(x => x.HandleCompanyContactAsync(It.IsAny<CompanyContactDto>(), It.IsAny<string>())).ReturnsAsync(new CompanyContact { Email = "email@example.com" });

//    //    // Act
//    //    var result = await _service.AddDirectorateAsync(1, 1, createDirectorate, "admin");

//    //    // Assert
//    //    Assert.True(result);
//    //    var directorate = await _context.Directorates.FirstOrDefaultAsync(d => d.Name == "New Directorate");
//    //    Assert.NotNull(directorate);
//    //    Assert.Equal("New Directorate", directorate.Name);
//    //}

//    //[Fact]
//    //public async Task UpdateDirectorateAsync_UpdatesDirectorate()
//    //{
//    //    // Arrange
//    //    var organisation = new Organisation { OrganisationId = 1, AddressId = 1 };
//    //    var contact = new Contact { ContactId = 1 };
//    //    var addressDto = new AddressDto {PostCode = "12345" };
//    //    var companyContactDto = new CompanyContactDto { Email = "email@example.com" };
//    //    var address = _mapper.Map<Address>(addressDto);
//    //    var companyContact = _mapper.Map<CompanyContact>(companyContactDto);

//    //    var directorate = new Directorate
//    //    {
//    //        DirectorateId = 1,
//    //        Name = "Old Directorate",
//    //        OrganisationId = 1,
//    //        ContactId = 1,
//    //        Address = address,
//    //        CompanyContact = companyContact,
//    //        IsActive = false
//    //    };

//    //    _context.Organisations.Add(organisation);
//    //    _context.Contacts.Add(contact);
//    //    _context.Addresses.Add(address);
//    //    _context.CompanyContacts.Add(companyContact);
//    //    _context.Directorates.Add(directorate);
//    //    await _context.SaveChangesAsync();

//    //    var updateDirectorate = new UpdateDirectorateDto
//    //    {
//    //        DirectorateId = 1,
//    //        Name = "Updated Directorate",
//    //        Address = new AddressDto { PostCode = "54321" },
//    //        CompanyContact = new CompanyContactDto
//    //        {
//    //            PhoneNumber = "0987654321",
//    //            Email = "newemail@example.com",
//    //            FullDescription = "Updated Description"
//    //        },
//    //        GetAddressFrom = "none",
//    //        IsActive = true
//    //    };

//    //    _addressServiceMock.Setup(x => x.HandleAddressAsync(It.IsAny<AddressDto>(), It.IsAny<string>())).ReturnsAsync(new Address { PostCode = "54321" });
//    //    _companyContactServiceMock.Setup(x => x.HandleCompanyContactAsync(It.IsAny<CompanyContactDto>(), It.IsAny<string>())).ReturnsAsync(new CompanyContact { Email = "newemail@example.com" });

//    //    // Act
//    //    var result = await _service.UpdateDirectorateAsync(1, 1, updateDirectorate, "admin");

//    //    // Assert
//    //    Assert.True(result);
//    //    var updatedDirectorate = await _context.Directorates.FirstOrDefaultAsync(d => d.Name == "Updated Directorate");
//    //    Assert.NotNull(updatedDirectorate);
//    //    Assert.Equal("Updated Directorate", updatedDirectorate.Name);
//    //    Assert.Equal("54321", updatedDirectorate.Address.PostCode);
//    //    Assert.Equal("newemail@example.com", updatedDirectorate.CompanyContact.Email);
//    //}

//    //[Fact]
//    //public async Task AddDirectorateAsync_ValidData_ReturnsTrue()
//    //{
//    //    // Arrange
//    //    var organisation = new Organisation { OrganisationId = 1 };
//    //    var contact = new Contact { ContactId = 1 };
//    //    _context.Organisations.Add(organisation);
//    //    _context.Contacts.Add(contact);
//    //    await _context.SaveChangesAsync();

//    //    var createDirectorate = new CreateDirectorateDto
//    //    {
//    //        Name = "Liver",
//    //        ShortDescription = "string",
//    //        LeadContact = "string",
//    //        Address = new AddressDto
//    //        {
//    //            Address1 = "",
//    //            Address2 = "",
//    //            Address3 = "",
//    //            PostCode = "",
//    //            City = "",
//    //            TownId = 0
//    //        },
//    //        CompanyContact = new CompanyContactDto
//    //        {
//    //            PhoneNumber = "456",
//    //            Fax = "555",
//    //            Email = "Liver@gmail.com",
//    //            WebAddress = "",
//    //            CharityNumber = "666",
//    //            CompanyNumber = "999",
//    //            TypeOfBusiness = "",
//    //            SICCode = "",
//    //            FullDescription = "In England"
//    //        },
//    //        GetAddressFrom = "Organisation"
//    //    };

//    //    _addressServiceMock.Setup(x => x.GetAddressByOrganisationId(It.IsAny<int>())).ReturnsAsync(new Address { PostCode = "" });
//    //    _companyContactServiceMock.Setup(x => x.GetCompanyContactByOrganisationId(It.IsAny<int>())).ReturnsAsync(new CompanyContact { Email = "Liver@gmail.com" });

//    //    // Act
//    //    var result = await _service.AddDirectorateAsync(1, 1, createDirectorate, "admin");

//    //    // Assert
//    //    Assert.True(result);
//    //    var directorate = await _context.Directorates.FirstOrDefaultAsync(d => d.Name == "Liver");
//    //    Assert.NotNull(directorate);
//    //    Assert.Equal("Liver", directorate.Name);
//    //    Assert.Equal("Liver@gmail.com", directorate.CompanyContact.Email);
//    //}

//    //[Fact]
//    //public async Task UpdateDirectorateAsync_ValidData_ReturnsTrue()
//    //{
//    //    // Arrange
//    //    var organisation = new Organisation { OrganisationId = 1 };
//    //    var contact = new Contact { ContactId = 1 };
//    //    var addressDto = new AddressDto { PostCode = "88" };
//    //    var companyContactDto = new CompanyContactDto { PhoneNumber = "333" };
//    //    var address = _mapper.Map<Address>(addressDto);
//    //    var companyContact = _mapper.Map<CompanyContact>(companyContactDto);

//    //    var directorate = new Directorate
//    //    {
//    //        DirectorateId = 9,
//    //        Name = "Old Directorate",
//    //        OrganisationId = 1,
//    //        ContactId = 1,
//    //        Address = address,
//    //        CompanyContact = companyContact,
//    //        IsActive = false
//    //    };

//    //    _context.Organisations.Add(organisation);
//    //    _context.Contacts.Add(contact);
//    //    _context.Addresses.Add(address);
//    //    _context.CompanyContacts.Add(companyContact);
//    //    _context.Directorates.Add(directorate);
//    //    await _context.SaveChangesAsync();

//    //    var updateDirectorate = new UpdateDirectorateDto
//    //    {
//    //        DirectorateId = 9,
//    //        Name = "Inter Milan",
//    //        ShortDescription = "string",
//    //        LeadContact = "string",
//    //        Address = new AddressDto
//    //        {
//    //            Address1 = "LaiXa",
//    //            Address2 = "Troi",
//    //            Address3 = "DucThuong",
//    //            PostCode = "88",
//    //            City = "Ha Noi",
//    //            TownId = 1
//    //        },
//    //        CompanyContact = new CompanyContactDto
//    //        {
//    //            PhoneNumber = "333",
//    //            Fax = "string",
//    //            Email = "string",
//    //            WebAddress = "string",
//    //            CharityNumber = "string",
//    //            CompanyNumber = "string",
//    //            TypeOfBusiness = "string",
//    //            SICCode = "string",
//    //            FullDescription = "string"
//    //        },
//    //        GetAddressFrom = "string",
//    //        IsActive = true
//    //    };

//    //    _addressServiceMock.Setup(x => x.HandleAddressAsync(It.IsAny<AddressDto>(), It.IsAny<string>())).ReturnsAsync(new Address { PostCode = "88" });
//    //    _companyContactServiceMock.Setup(x => x.HandleCompanyContactAsync(It.IsAny<CompanyContactDto>(), It.IsAny<string>())).ReturnsAsync(new CompanyContact { Email = "string" });

//    //    // Act
//    //    var result = await _service.UpdateDirectorateAsync(1, 1, updateDirectorate, "admin");

//    //    // Assert
//    //    Assert.True(result);
//    //    var updatedDirectorate = await _context.Directorates.FirstOrDefaultAsync(d => d.Name == "Inter Milan");
//    //    Assert.NotNull(updatedDirectorate);
//    //    Assert.Equal("Inter Milan", updatedDirectorate.Name);
//    //    Assert.Equal("88", updatedDirectorate.Address.PostCode);
//    //    Assert.Equal("string", updatedDirectorate.CompanyContact.Email);
//    //}

//}