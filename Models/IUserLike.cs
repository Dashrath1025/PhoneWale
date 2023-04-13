using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
   public interface IUserLike
    {
        IEnumerable<UserLike> GetLikesByUserId(string Uid);
        bool AddUserLike(UserLike userlike);
        UserLike GetUserLikeById(int lid);
        bool RemoveUserLike(UserLike liked);
    }
}
