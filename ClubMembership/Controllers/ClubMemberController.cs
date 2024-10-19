using ClubMembership.Models;
using ClubMembership.Repositories;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;

public class ClubMemberController : Controller
{
    private readonly IClubMemberRepository _clubMemberRepository;
    private readonly ISocietyRepository _societyRepository;
    private readonly IHobbyRepository _hobbyRepository;

    public ClubMemberController(IClubMemberRepository clubMemberRepository, ISocietyRepository societyRepository, IHobbyRepository hobbyRepository)
    {
        _clubMemberRepository = clubMemberRepository;
        _societyRepository = societyRepository;
        _hobbyRepository = hobbyRepository; 
    }

    public IActionResult ShowMembers(string memberName, string societyName, int? gender, int? membershipCategory, bool? isActive)
    {
        var members = _clubMemberRepository.GetAllClubMembers(memberName, societyName, gender, membershipCategory, isActive);
        return View(members);
    }

    public IActionResult AddMember()
    {
        var viewModel = new AddMemberViewModel
        {
            Societies = _societyRepository.GetAllSocieties().Where(s => (bool)s.IsActive).ToList(),
            Hobbies = _hobbyRepository.GetAllHobbies().Where(h => (bool)h.IsActive).ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult AddMember(AddMemberViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create a new member instance
            var newMember = new ClubMember
            {
                Id = Guid.NewGuid(),
                MemberName = model.MemberName,
                SocietyId = model.SocietyId,
                Gender = model.Gender,
                MembershipCategory = model.MembershipCategory,
                Remark = model.Remark,
                IsActive = model.IsActive
            };

            // Add the new member using the repository
            _clubMemberRepository.AddClubMember(newMember); // Ensure this method is in your repository

            // Handle hobbies if selected
            if (model.SelectedHobbies != null && model.SelectedHobbies.Count > 0)
            {
                foreach (var hobbyId in model.SelectedHobbies)
                {
                    var clubMemberHobby = new ClubMemberHobby
                    {
                        Id = Guid.NewGuid(),
                        ClubMemberId = newMember.Id,
                        HobbyId = Guid.Parse(hobbyId) // Ensure hobbyId is a valid Guid
                    };
                    _hobbyRepository.AddClubMemberHobby(clubMemberHobby); // Ensure this method is in your hobby repository
                }
            }

            // Redirect to the member list or another appropriate page
            return RedirectToAction(nameof(ShowMembers));
        }

        // If model state is invalid, repopulate societies and hobbies for the view
        model.Societies = _societyRepository.GetAllSocieties().Where(s => (bool)s.IsActive).ToList();
        model.Hobbies = _hobbyRepository.GetAllHobbies().Where(h => (bool)h.IsActive).ToList();

        return View(model);
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

    public IActionResult DownloadExcel()
    {
        // Set the EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // Fetch members data along with their societies and hobbies
        var members = _clubMemberRepository.GetAllClubMembers(null, null, null, null, null)
            .Select(member => new
            {
                member.MemberName,
                SocietyName = member.Society.SocietyName,
                Hobbies = member.ClubMemberHobbies.Select(h => h.Hobby.HobbyName).ToList(),
                Gender = member.Gender == 0 ? "Male" : member.Gender == 1 ? "Female" : "Other",
                member.Remark,
                IsActive = member.IsActive.HasValue && member.IsActive.Value ? "Yes" : "No"
            })
            .ToList();

        using (var package = new ExcelPackage())
        {
            // Create a worksheet
            var worksheet = package.Workbook.Worksheets.Add("Club Members");

            // Define headers
            worksheet.Cells[1, 1].Value = "Member Name";
            worksheet.Cells[1, 2].Value = "Society";
            worksheet.Cells[1, 3].Value = "Hobbies";
            worksheet.Cells[1, 4].Value = "Gender";
            worksheet.Cells[1, 5].Value = "Remarks";
            worksheet.Cells[1, 6].Value = "Is Active";

            // Populate the worksheet with member data
            for (int i = 0; i < members.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = members[i].MemberName;
                worksheet.Cells[i + 2, 2].Value = members[i].SocietyName;
                worksheet.Cells[i + 2, 3].Value = string.Join(", ", members[i].Hobbies);
                worksheet.Cells[i + 2, 4].Value = members[i].Gender;
                worksheet.Cells[i + 2, 5].Value = members[i].Remark;
                worksheet.Cells[i + 2, 6].Value = members[i].IsActive;
            }

            // Save the Excel package to a MemoryStream
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0; // Reset the stream position to the beginning

            // Create a file name with a timestamp
            string excelName = $"ClubMembers-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";

            // Return the stream without disposing it
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }

}
