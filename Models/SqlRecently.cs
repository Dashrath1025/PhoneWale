using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public class SqlRecently:IRecently
    {
        private ShopifyDbContext _recently;
        public SqlRecently(ShopifyDbContext context)
        {
            _recently = context;
        }

        public bool AddRecent(Recently recent)
        {
            var userHistory = _recently.Recentlies.Where(h => h.UId == recent.UId).ToList();
            if (userHistory.Count >= 5)
            {

                var oldestHist = userHistory.OrderBy(h => h.Id).Last();
                _recently.Remove(oldestHist);
            }

            _recently.Add(recent);
            _recently.SaveChanges();
            return true;
        }

        public IEnumerable<Recently> GetRecentByUserId(string Uid)
        {
            return _recently.Recentlies.Where(s => s.UId == Uid).ToList();
        }

        public Recently GetRecentById(int hid)
        {
            return _recently.Recentlies.SingleOrDefault(each => each.Id == hid);
        }

        public bool RemoveRecently(Recently recent)
        {
            Recently isDuplicate = _recently.Recentlies.FirstOrDefault(each => each.Id == recent.Id);
            if (isDuplicate != null)
            {
                _recently.Remove(recent);
                _recently.SaveChanges();
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
