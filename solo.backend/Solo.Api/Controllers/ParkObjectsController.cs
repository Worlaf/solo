using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solo.Api.Models;
using Solo.Api.Models.ParkObject;
using Solo.Data.Infrastructure;
using Solo.Data.Repositories;
using Solo.Domain.Entities;
using Solo.Domain.Map;

namespace Solo.Api.Controllers
{
    [ApiController]
    [Route("parks/{parkId:int}/[controller]")]
    public class ParkObjectsController : ControllerBase
    {
        private readonly ParkObjectRepository _parkObjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ParkObjectsController(ParkObjectRepository parkObjectRepository, IUnitOfWork unitOfWork)
        {
            _parkObjectRepository = parkObjectRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ListModel<ParkObjectViewModel>> ListAsync(int parkId)
        {
            return new ListModel<ParkObjectViewModel>
            {
                Items = await _parkObjectRepository.GetManyAsync(po => po.ParkId == parkId, po => new ParkObjectViewModel
                {
                    Id = po.Id,
                    Name = po.Name,
                    WorkScheduleJson = po.WorkScheduleJson,
                    Location = new Point {Latitude = po.Location.Latitude, Longitude = po.Location.Longitude}, // todo: owned property tracking issues
                    AdministrationDescriptionMarkdown = po.AdministrationDescriptionMarkdown,
                    ParkId = po.ParkId,
                    PriceForAdults = po.PriceForAdults,
                    PriceForChildren = po.PriceForChildren,
                    PublicDescriptionMarkdown = po.PublicDescriptionMarkdown,
                    Type = po.Type,
                    ImageUrl = po.ImageUrl
                })
            };
        }

        [HttpGet("{id:int}")]
        public async Task<ParkObjectViewModel> GetAsync(int id)
        {
            return await _parkObjectRepository.GetByIdAsync(id, po => new ParkObjectViewModel
            {
                Id = po.Id,
                Name = po.Name,
                WorkScheduleJson = po.WorkScheduleJson,
                Location = new Point {Latitude = po.Location.Latitude, Longitude = po.Location.Longitude}, // todo: owned property tracking issues
                AdministrationDescriptionMarkdown = po.AdministrationDescriptionMarkdown,
                ParkId = po.ParkId,
                PriceForAdults = po.PriceForAdults,
                PriceForChildren = po.PriceForChildren,
                PublicDescriptionMarkdown = po.PublicDescriptionMarkdown,
                Type = po.Type,
                ImageUrl = po.ImageUrl
            });
        }

        [HttpPost]
        public async Task<ParkObjectViewModel> CreateAsync(ParkObjectSaveModel saveModel)
        {
            return await SaveAsync(null, saveModel);
        }

        [HttpPut("{id:int}")]
        public async Task<ParkObjectViewModel> UpdateAsync(int id, ParkObjectSaveModel saveModel)
        {
            return await SaveAsync(id, saveModel);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var obj = await _parkObjectRepository.GetByIdAsync(id);
            _parkObjectRepository.Delete(obj);

            return Ok();
        }

        private async Task<ParkObjectViewModel> SaveAsync(int? id, ParkObjectSaveModel model)
        {
            var entity = id.HasValue ? await _parkObjectRepository.GetByIdAsync(id.Value) : new ParkObject();

            entity.Name = model.Name;
            entity.AdministrationDescriptionMarkdown = model.AdministrationDescriptionMarkdown;
            entity.Location = model.Location;
            entity.ParkId = model.ParkId;
            entity.PriceForChildren = model.PriceForChildren;
            entity.PriceForAdults = model.PriceForAdults;
            entity.PublicDescriptionMarkdown = model.PublicDescriptionMarkdown;
            entity.ImageUrl = model.ImageUrl;
            entity.Type = model.Type;
            entity.WorkScheduleJson = model.WorkScheduleJson;

            _parkObjectRepository.Save(entity);
            await _unitOfWork.CommitAsync();

            var viewModel = new ParkObjectViewModel
            {
                Name = entity.Name,
                AdministrationDescriptionMarkdown = entity.AdministrationDescriptionMarkdown,
                Location = entity.Location,
                ParkId = entity.ParkId,
                PriceForChildren = entity.PriceForChildren,
                PriceForAdults = entity.PriceForAdults,
                PublicDescriptionMarkdown = entity.PublicDescriptionMarkdown,
                Id = entity.Id,
                Type = entity.Type,
                ImageUrl = entity.ImageUrl,
                WorkScheduleJson = entity.WorkScheduleJson
            };

            return viewModel;
        }
    }
}