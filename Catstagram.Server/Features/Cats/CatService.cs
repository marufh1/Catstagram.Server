using Catstagram.Server.Data;
using Catstagram.Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catstagram.Server.Features.Cats
{
    public class CatService : ICatService
    {
        private readonly CatstagramDbContext data;

        public CatService(CatstagramDbContext data) => this.data = data;
        public async Task<int> Create(string imgUrl, string description, string userId)
        {
            var cat = new Cat
            {
                Description = description,
                ImageUrl = imgUrl,
                UserId = userId
            };

            this.data.Add(cat);
            await this.data.SaveChangesAsync();

            return cat.Id;
        }
    }
}
