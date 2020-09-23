using System;
using System.Collections.Generic;
using System.Text;
using Solo.Data.Infrastructure;
using Solo.Domain;
using Solo.Domain.Entities;

namespace Solo.Data.Repositories
{
    public class UserRepository : EntityRepositoryBase<User>, IEntityRepository<User>
    {
        public UserRepository(SoloDbContext dbContext) : base(dbContext)
        {
        }
    }
}
