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

        [HttpGet("my")]
        public async Task<ListModel<TicketViewModel>> GetMyTicketsAsync(int? parkObjectId = null)
        {
            var userId = GetUserId();
            var result = new ListModel<TicketViewModel>
            {
                Items = await _ticketRepository.GetManyAsync(
                    t => t.CustomerId == userId && (!parkObjectId.HasValue || t.ParkObjectId == parkObjectId.Value) && !t.Closed, t =>
                        new TicketViewModel
                        {
                            Id = t.Id,
                            Type = t.Type,
                            Closed = t.Closed,
                            CustomerId = t.CustomerId,
                            ParkObjectId = t.ParkObjectId
                        })
            };

            PopulateQueueNumber(result);

            return result;
        }

        [HttpPost("buy")]
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

        [HttpPost("close/{id:int}")]
        public async Task<IActionResult> CloseAsync(int id)
        {
            var queueNumber = await _ticketRepository.GetQueueNumberAsync(id);
            if (queueNumber == 0)
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
            if (model.Items.Count == 0)
                return;


            foreach (var ticket in model.Items.Where(i => !i.Closed))
            {
                ticket.QueueNumber = _ticketRepository.GetQueueNumberAsync(ticket.Id).Result;
            }
        }

        private int GetUserId() => int.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value); // надеемся на лучшее
    }
}