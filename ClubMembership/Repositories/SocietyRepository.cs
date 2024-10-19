using ClubMembership.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubMembership.Repositories
{
    public class SocietyRepository : ISocietyRepository
    {
        private readonly iBlueAnts_MembersContext _context;

        public SocietyRepository(iBlueAnts_MembersContext context)
        {
            _context = context;
        }

        public IEnumerable<Society> GetAllSocieties()
        {
            return _context.Societies.ToList();
        }

        public Society GetSocietyById(Guid id)
        {
            return _context.Societies.FirstOrDefault(s => s.Id == id);
        }
    }
}
