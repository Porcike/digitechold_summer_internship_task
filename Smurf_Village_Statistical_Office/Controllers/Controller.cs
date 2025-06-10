using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Utils;
using System.Security.AccessControl;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class Controller : ControllerBase
    {
        //private readonly SmurfVillageContext _context;

        //public Controller(SmurfVillageContext context)
        //{
        //    _context = context;
        //}

        //[HttpGet("Smurfs")]
        //public async Task<IActionResult> GetSmurfs()
        //{
            
        //    var smurfs = await _context.Smurfs
        //        .AsNoTracking()
        //        .ToListAsync();

        //    return Ok(smurfs);
        //}

        //[HttpGet("MushroomHouses")]
        //public async Task<IActionResult> GetMushroomHouses()
        //{
        //    var houses = await _context.MushroomHouses
        //        .Select(h => new
        //        {
        //            h.Id,
        //            h.Capacity,
        //            h.Color,
        //            h.Motto,
        //            ResidentIds = h.Residents.Select(r => r.Id).ToList()
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    return Ok(houses);
        //}

        //[HttpGet("WorkingPlaces")]
        //public async Task<IActionResult> GetWorkingPlaces()
        //{
        //    var workplaces = await _context.WorkingPlaces
        //        .Select(w => new
        //        {
        //            w.Id,
        //            w.Name,
        //            AcceptedJobs = w.AcceptedJobs,         
        //            EmployeeIds = w.Employees.Select(e => e.Id).ToList()
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    return Ok(workplaces);
        //}

       
        //[HttpGet("LeisureVenues")]
        //public async Task<IActionResult> GetLeisureVenues()
        //{
        //    var venues = await _context.LeisureVenues
                
        //        .Select(v => new
        //        {
        //            v.Id,
        //            v.Name,
        //            v.Capacity,
        //            v.AcceptedBrand,
        //            MemberIds = v.Members.Select(m => m.Id).ToList()
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    return Ok(venues);
        //}

        //[HttpGet]
        //[Route("Jobs")]
        //public async Task<IActionResult> GetJobs()
        //{
        //    var items = Enum.GetValues<Job>()
        //                    .Cast<Job>()
        //                    .Select(j => new
        //                    {
        //                        Id = (int)j,
        //                        Name = j.ToString()
        //                    });
        //    return Ok(items);
         
        //}

        //[HttpGet]
        //[Route("Foods")]
        //public async Task<IActionResult> GetFoods()
        //{
        //    var items = Enum.GetValues<Food>()
        //         .Cast<Food>()
        //         .Select(f => new
        //         {
        //             Id = (int)f,
        //             Name = f.ToString()
        //         });
        //    return Ok(items);
        //}

        //[HttpGet]
        //[Route("Brands")]
        //public async Task<IActionResult> GetBrands()
        //{
        //    var items = Enum.GetValues<Brand>()
        //         .Cast<Brand>()
        //         .Select(b => new
        //         {
        //             Id = (int)b,
        //             Name = b.ToString()
        //         });
        //    return Ok(items);
        //}
    }
}
