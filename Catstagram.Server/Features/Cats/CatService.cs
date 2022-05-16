using Catstagram.Server.Data;
using Catstagram.Server.Data.Model;
using Catstagram.Server.Features.Cats.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<CatListingServiceModel>> ByUser(string userId)
            => await this.data
                .Cats
                .Where(c => c.UserId == userId)
                .Select(c => new CatListingServiceModel
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl
                })
                .ToListAsync();

        public async Task<CatDetailsServiceModel> Details(int id)
        {
            return await this.data
                      .Cats
                      .Where(c => c.Id == id)
                      .Select(c => new CatDetailsServiceModel
                      {
                          Id = c.Id,
                          UserId = c.UserId,
                          Description = c.Description,
                          ImageUrl = c.ImageUrl,
                          UserName = c.User.UserName
                      })
                      .FirstOrDefaultAsync();
        }

        public async Task<bool> Update(int id, string description, string userId)
        {
            var cat = await this.GetByIdAndByUserId(id, userId);

            if (cat == null)
                return false;

            cat.Description = description;
            await this.data.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id, string userId)
        {
            var cat = await this.GetByIdAndByUserId(id,userId);

            if (cat == null)
                return false;

            this.data.Cats.Remove(cat);
            await this.data.SaveChangesAsync();

            return true;
        }


        private async Task<Cat> GetByIdAndByUserId(int id, string userId)
        {
            var cat = await this.data.Cats.Where(c => c.Id == id && c.UserId == userId)
              .FirstOrDefaultAsync();
            return cat;
        }

    }
}
