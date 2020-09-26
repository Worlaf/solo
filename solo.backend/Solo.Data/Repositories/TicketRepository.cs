using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solo.Data.Infrastructure;
using Solo.Domain.Entities;

namespace Solo.Data.Repositories
{
    public class TicketRepository: EntityRepositoryBase<Ticket>, IEntityRepository<Ticket>
    {
        public TicketRepository(SoloDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetQueueNumberAsync(int ticketId)
        {
            var ticket = await GetByIdAsync(ticketId, t => new {t.Closed, t.ParkObjectId});
            
            if (ticket.Closed)
                throw new InvalidOperationException("Can't get queue number for closed ticket.");

            return await CountAsync(t => t.ParkObjectId == ticket.ParkObjectId && t.Id < ticketId);
        }
    }
}
