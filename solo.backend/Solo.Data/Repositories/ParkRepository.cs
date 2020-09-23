using System;
using System.Collections.Generic;
using System.Text;
using Solo.Data.Infrastructure;
using Solo.Domain;
using Solo.Domain.Entities;

namespace Solo.Data.Repositories
{
    public class ParkRepository : EntityRepositoryBase<Park>, IEntityRepository<Park>
    {
        public ParkRepository(SoloDbContext dbContext) : base(dbContext)
        {
        }
    }
}
