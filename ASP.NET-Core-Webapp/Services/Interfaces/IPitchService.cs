using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp.Services
{
    public interface IPitchService
    {
        IList<string> CreateEmailListFromPostedPitch(SeedData.Pitch pitch);
        IList<string> CreateListWithBadgeHolders(SeedData.Pitch pitch);
    }
}
