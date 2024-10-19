using ClubMembership.Models;

namespace ClubMembership.Repositories
{
    public interface IClubMemberRepository
    {
        IEnumerable<ClubMember> GetAllClubMembers();
        ClubMember GetClubMemberById(Guid id);
    }

    public interface IHobbyRepository
    {
        IEnumerable<Hobby> GetAllHobbies();
        Hobby GetHobbyById(Guid id);
    }

    public interface ISocietyRepository
    {
        IEnumerable<Society> GetAllSocieties();
        Society GetSocietyById(Guid id);
    }
}
