using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public class SqlUserLike: IUserLike
    {
        private ShopifyDbContext  _userlikes;
        public SqlUserLike(ShopifyDbContext context)
        {
            _userlikes = context;
        }
        public bool AddUserLike(UserLike userlike)
        {
            UserLike isDuplicate = _userlikes.UserLikes.FirstOrDefault(each => each.PId == userlike.PId && each.UId == userlike.UId);
            if (isDuplicate == null)
            {
                _userlikes.Add(userlike);
                _userlikes.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<UserLike> GetLikesByUserId(string Uid)
        {
            return _userlikes.UserLikes.Where(s => s.UId == Uid).ToList();
        }

        public bool RemoveUserLike(UserLike liked)
        {
            UserLike isDuplicate = _userlikes.UserLikes.FirstOrDefault(each => each.Id == liked.Id);
            if (isDuplicate != null)
            {
                _userlikes.Remove(liked);
                _userlikes.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserLike GetUserLikeById(int Lid)
        {
            return _userlikes.UserLikes.SingleOrDefault(each => each.Id == Lid);
        }
    }
}
