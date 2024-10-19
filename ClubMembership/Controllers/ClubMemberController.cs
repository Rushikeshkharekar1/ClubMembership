using ClubMembership.Repositories;
using Microsoft.AspNetCore.Mvc;

public class ClubMemberController : Controller
{
    private readonly IClubMemberRepository _clubMemberRepository;

    public ClubMemberController(IClubMemberRepository clubMemberRepository)
    {
        _clubMemberRepository = clubMemberRepository;
    }

    public IActionResult ShowMembers()
    {
        var members = _clubMemberRepository.GetAllClubMembers();
        return View(members);
    }

    public IActionResult Details(Guid id)
    {
        var member = _clubMemberRepository.GetClubMemberById(id);
        if (member == null)
        {
            return NotFound();
        }
        return View(member);
    }
}
