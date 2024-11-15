using EDC.Client.Models;
using EDC.Server.Data;
using EDC.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;


namespace Survey.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SurveyController(DefaultDbContext dbContext) : ControllerBase
{
    [HttpGet("Districts")]
    public async Task<ActionResult<DistrictModel[]>> GetDistrictsAsync()
    {
        var items = await dbContext.Districts.AsNoTracking().ToArrayAsync().ConfigureAwait(false);
        if (items.Length != 0)
        {
            return items.Select(i => new DistrictModel()
            {
                Id = i.Id,
                Name = i.Name,
                TamilName = i.TamilName,
                IsActive = i.IsActive
            }).ToArray();
        }
        return Array.Empty<DistrictModel>();
    }

    [HttpGet("ParliamentConstituencies")]
    public async Task<ActionResult<ParliamentConstituencyModel[]>> GetParliamentConstituenciesAsync()
    {
        var items = await dbContext.ParliamentConstituencies.AsNoTracking().ToArrayAsync().ConfigureAwait(false);
        if (items.Length != 0)
        {
            return items.Select(i => new ParliamentConstituencyModel()
            {
                Id = i.Id,
                Name = i.Name,
                TamilName = i.TamilName,
                IsActive = i.IsActive,
                DistrictId = i.DistrictId
            }).ToArray();
        }
        return Array.Empty<ParliamentConstituencyModel>();
    }

    [HttpGet("Constituencies")]
    public async Task<ActionResult<ConstituencyModel[]>> GetConstituenciesAsync()
    {
        var items = await dbContext.Constituencies.AsNoTracking().ToArrayAsync().ConfigureAwait(false);
        if (items.Length != 0)
        {
            return items.Select(i => new ConstituencyModel()
            {
                Id = i.Id,
                Name = i.Name,
                TamilName = i.TamilName,
                IsActive = i.IsActive,
                ParliamentConstituencyNo = i.ParliamentConstituencyNo
            }).ToArray();
        }
        return Array.Empty<ConstituencyModel>();
    }

    [HttpGet("LoadAreaTypes")]
    public async Task<ActionResult<AreaTypeModel[]>> GetAreaTypesAsync()
    {
        var items = await dbContext.AreaTypes.AsNoTracking().ToArrayAsync().ConfigureAwait(false);
        if (items.Length != 0)
        {
            var item =  items.Select(i => new AreaTypeModel()
            {
                Id = i.Id,
                Name = i.Name,
                TamilName = i.TamilName,
                IsActive = i.IsActive,
            }).ToArray();
            return item;
        }
        return Array.Empty<AreaTypeModel>();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Save(MeetingEntryModel report)
    {
        try
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            if (report.Content != null && report.Image != null)
            {
                var uniqueFileName1 = DateTime.Now.ToString("yyyyMMddHHmm") + "_" + report.Image;
                var filePath1 = Path.Combine(uploadsFolder, uniqueFileName1);
                report.Image = uniqueFileName1;
                FileStream file1 = System.IO.File.Create(filePath1);
                file1.Write(report.Content, 0, report.Content.Length);
                file1.Close();
            }
            var newsurvey = new Meeting15nov2024()
            {
                DistrictId = report.DistrictId,
                ConstituencyId = report.ConstituencyId,
                ParliamentConstituencyId = report.ParliamentConstituencyId,
                AreaTypeId = report.AreaTypeId,
                AreaName = report.AreaName,
                BoothNo = report.BoothNo,
                PhoneNo = report.PhoneNo,
                Name = report.Name,
                Address = report.Address,
                Gender = report.Gender,
                Image = report.Image,
                CreatedOn = DateTime.Now,
            };
            dbContext.Meeting15nov2024s.Add(newsurvey);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return report.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

}