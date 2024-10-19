using ClubMembership.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubMembership.Repositories
{
    public class ClubMemberRepository : IClubMemberRepository
    {
        private readonly iBlueAnts_MembersContext _context;

        public ClubMemberRepository(iBlueAnts_MembersContext context)
        {
            _context = context;
        }

        public IEnumerable<ClubMember> GetAllClubMembers()
        {
            return _context.ClubMembers.Include(c => c.Society).Include(c => c.ClubMemberHobbies).ToList();
        }

        public ClubMember GetClubMemberById(Guid id)
        {
            return _context.ClubMembers
                .Include(c => c.Society)
                .Include(c => c.ClubMemberHobbies)
                .ThenInclude(ch => ch.Hobby)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
