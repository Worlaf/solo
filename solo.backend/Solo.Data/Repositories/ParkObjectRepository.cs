using System;
using System.Collections.Generic;
using System.Text;
using Solo.Data.Infrastructure;
using Solo.Domain;
using Solo.Domain.Entities;

namespace Solo.Data.Repositories
{
    public class ParkObjectRepository : EntityRepositoryBase<ParkObject>, IEntityRepository<ParkObject>
    {
        public ParkObjectRepository(SoloDbContext dbContext) : base(dbContext)
        {
        }
    }
}
