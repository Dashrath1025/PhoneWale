using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
   public  interface IRecently
    {
        bool AddRecent(Recently recent);

        IEnumerable<Recently> GetRecentByUserId(string Uid);
        Recently GetRecentById(int hid);
        bool RemoveRecently(Recently recent);
    }
}
