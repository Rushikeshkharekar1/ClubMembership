using ClubMembership.Models;

namespace ClubMembership.Repositories
{
    public interface IClubMemberRepository
    {
        IEnumerable<ClubMember> GetAllClubMembers(string memberName = null, string societyName = null, int? gender = null, int? membershipCategory = null, bool? isActive = null);
        ClubMember GetClubMemberById(Guid id);
        void AddClubMember(ClubMember member);

    }

    public interface IHobbyRepository
    {
        IEnumerable<Hobby> GetAllHobbies();
        Hobby GetHobbyById(Guid id);
        void AddClubMemberHobby(ClubMemberHobby hobby);

    }

    public interface ISocietyRepository
    {
        IEnumerable<Society> GetAllSocieties();
        Society GetSocietyById(Guid id);
    }
}
