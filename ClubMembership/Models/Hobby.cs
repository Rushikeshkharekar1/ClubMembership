using System;
using System.Collections.Generic;

namespace ClubMembership.Models
{
    public partial class Hobby
    {
        public Hobby()
        {
            ClubMemberHobbies = new HashSet<ClubMemberHobby>();
        }

        public Guid Id { get; set; }
        public string HobbyName { get; set; } = null!;
        public bool? IsActive { get; set; }

        public virtual ICollection<ClubMemberHobby> ClubMemberHobbies { get; set; }
    }
}
