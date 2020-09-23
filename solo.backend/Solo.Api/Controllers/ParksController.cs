using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solo.Api.Models;
using Solo.Common.Extensions;
using Solo.Data.Repositories;
using Solo.Domain.Map;

namespace Solo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParksController : ControllerBase
    {
        private readonly ParkRepository _parkRepository;

        public ParksController(ParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }

        [HttpGet]
        public async Task<ListModel<object>> ListAsync()
        {
            return new ListModel<object>
            {
                Items = (await _parkRepository.GetAllAsync(p => new
                {
                    p.Id,
                    p.Name,
                    p.RegionJson
                })).Select(p => new
                {
                    p.Id, 
                    p.Name, 
                    Region = p.RegionJson.FromJson<Region>()
                }).ToArray()
            };
        }

        [HttpGet("{id:int}")]
        public async Task<object> GetAsync(int id)
        {
            var park = await _parkRepository.GetByIdAsync(id, p => new {p.Id, p.Name, p.RegionJson});

            return new
            {
                park.Id,
                park.Name,
                Region = park.RegionJson.FromJson<Region>()
            };
        }
    }
}