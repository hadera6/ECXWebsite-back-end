using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Persistence.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public MessageRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
