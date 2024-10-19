﻿using ClubMembership.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubMembership.Repositories
{
    public class HobbyRepository : IHobbyRepository
    {
        private readonly iBlueAnts_MembersContext _context;

        public HobbyRepository(iBlueAnts_MembersContext context)
        {
            _context = context;
        }

        public void AddClubMemberHobby(ClubMemberHobby hobby)
        {
            _context.ClubMemberHobbies.Add(hobby);
            _context.SaveChanges(); // Ensure you call SaveChanges here to persist the new hobby
        }


        public IEnumerable<Hobby> GetAllHobbies()
        {
            return _context.Hobbies.ToList();
        }

        public Hobby GetHobbyById(Guid id)
        {
            return _context.Hobbies.FirstOrDefault(h => h.Id == id);
        }
    }
}
