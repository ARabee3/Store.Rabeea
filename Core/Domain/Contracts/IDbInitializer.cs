using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    // will do data seeding , apply unapplied migrations
    public interface IDbInitializer
    {
        Task InitializeAsync();
        Task InitializeIdentityAsync();
    }
}
