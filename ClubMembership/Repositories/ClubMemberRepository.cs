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

        public IEnumerable<ClubMember> GetAllClubMembers(string memberName = null, string societyName = null, int? gender = null, int? membershipCategory = null, bool? isActive = null)
        {
            var query = _context.ClubMembers
                .Include(cm => cm.Society)
                .Include(cm => cm.ClubMemberHobbies)
                .ThenInclude(cmh => cmh.Hobby)
                .AsQueryable();

            if (!string.IsNullOrEmpty(memberName))
            {
                query = query.Where(cm => cm.MemberName.Contains(memberName));
            }

            if (!string.IsNullOrEmpty(societyName))
            {
                query = query.Where(cm => cm.Society.SocietyName.Contains(societyName));
            }

            if (gender.HasValue)
            {
                query = query.Where(cm => cm.Gender == gender.Value);
            }

            if (membershipCategory.HasValue)
            {
                query = query.Where(cm => cm.MembershipCategory == membershipCategory.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(cm => cm.IsActive == isActive.Value);
            }

            return query.ToList();
        }

        public void AddClubMember(ClubMember member)
        {
            _context.ClubMembers.Add(member);
            _context.SaveChanges(); // Ensure you call SaveChanges here to persist the new member
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
