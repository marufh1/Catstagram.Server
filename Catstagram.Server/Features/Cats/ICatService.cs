using Catstagram.Server.Features.Cats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catstagram.Server.Features.Cats
{
    public interface ICatService
    {
        public Task<int> Create(string imgUrl, string description, string userId);
        public Task<bool> Update(int id, string description, string userId);
        public Task<bool> Delete(int id, string userId);
        public Task<IEnumerable<CatListingServiceModel>> ByUser(string userId);
        public Task<CatDetailsServiceModel> Details(int id);
    }
}
