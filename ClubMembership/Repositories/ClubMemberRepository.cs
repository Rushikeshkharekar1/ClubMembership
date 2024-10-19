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
            return _context.ClubMembers
      .Include(c => c.Society) // Include the related Society entity
      .Include(c => c.ClubMemberHobbies) // Include the ClubMemberHobbies navigation property
      .ThenInclude(cmh => cmh.Hobby) // Include the related Hobby entity
      .ToList(); // Execute the query and return the list
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
