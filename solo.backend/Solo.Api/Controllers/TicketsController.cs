using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solo.Api.Models;
using Solo.Api.Models.Ticket;
using Solo.Data.Infrastructure;
using Solo.Data.Repositories;
using Solo.Domain.Entities;

namespace Solo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketRepository _ticketRepository;

        public TicketsController(IUnitOfWork unitOfWork, TicketRepository ticketRepository)
        {
            _unitOfWork = unitOfWork;
            _ticketRepository = ticketRepository;
        }

        [HttpGet("/my")]
        public async Task<ListModel<TicketViewModel>> GetMyTicketsAsync(int? parkObjectId = null, bool? includeClosed = null)
        {
            var userId = GetUserId();
            var result = new ListModel<TicketViewModel>
            {
                Items = await _ticketRepository.GetManyAsync(
                    t => t.CustomerId == userId && (!parkObjectId.HasValue || t.ParkObjectId == parkObjectId.Value) && (!includeClosed.HasValue || includeClosed.Value || !t.Closed), t =>
                        new TicketViewModel
                        {
                            Id = t.Id
                            Type = t.Type,
                            Closed = t.Closed,
                            CustomerId = t.CustomerId,
                            ParkObjectId = t.ParkObjectId
                        })
            };

            PopulateQueueNumber(result);

            return result;
        }

        [HttpPost("/buy")]
        public async Task<ListModel<TicketViewModel>> BuyAsync(BuyTicketsModel model)
        {
            // а тут пригодился бы слой сервисов, но времени мало
            var userId = GetUserId();
            var tickets = CreateTickets(TicketType.Adult, model.AdultCount).Concat(CreateTickets(TicketType.Child, model.ChildCount))
                .Concat(CreateTickets(TicketType.Privileged, model.PrivilegedCount)).ToArray();

            foreach (var ticket in tickets)
            {
                _ticketRepository.Save(ticket);
            }

            await _unitOfWork.CommitAsync();

            var result = new ListModel<TicketViewModel>
            {
                Items = tickets.Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    Type = t.Type,
                    Closed = t.Closed,
                    ParkObjectId = t.ParkObjectId,
                    CustomerId = t.CustomerId
                }).ToArray()
            };

            PopulateQueueNumber(result);

            return result;

            IEnumerable<Ticket> CreateTickets(TicketType type, int count) => Enumerable.Range(0, count).Select(_ => new Ticket
            {
                Type = type, ParkObjectId = model.ParkObjectId, CustomerId = userId
            });
        }

        [HttpPost("/close/{id:int}")]
        public async Task<IActionResult> CloseAsync(int id)
        {
            if (await _ticketRepository.GetQueueNumberAsync(id) == 0)
            {
                var ticket = await _ticketRepository.GetByIdAsync(id);
                ticket.Closed = true;

                _ticketRepository.Save(ticket);
                await _unitOfWork.CommitAsync();

                return Ok();
            }

            return BadRequest("Нельзя закрыть билет вне очереди.");
        }

        private void PopulateQueueNumber(ListModel<TicketViewModel> model)
        {
            var parkObjectIds = model.Items.Select(i => i.ParkObjectId).Distinct().ToArray();
            var maxId = model.Items.Max(i => i.Id);
            var tickets = _ticketRepository.GetManyAsync(t => t.Id < maxId && parkObjectIds.Contains(t.ParkObjectId) && !t.Closed, t => new {t.Id, t.ParkObjectId}).Result;

            foreach (var ticket in model.Items.Where(i => !i.Closed))
            {
                ticket.QueueNumber = tickets.Count(t => t.Id < ticket.Id && t.ParkObjectId == ticket.ParkObjectId);
            }
        }

        private int GetUserId() => int.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value); // надеемся на лучшее
    }
}