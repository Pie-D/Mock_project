using AutoMapper;
using BusinessLayer.Interfaces;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ContactService : IContactService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public ContactService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<string>> GetAllLeadContacts()
        {
            var listLeadContact = await _context.Contacts.Where(c=>c.ManagerId==null).Select(c=>c.KnownAs).ToListAsync();
            return listLeadContact;
        }
    }
}
