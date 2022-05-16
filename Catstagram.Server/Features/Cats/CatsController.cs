namespace Catstagram.Server.Features.Cats
{
    using Catstagram.Server.Data.Model;
    using Catstagram.Server.Features.Cats.Models;
    using Catstagram.Server.Infrastructure;
    using Catstagram.Server.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using static Infrastructure.WebConstants;

    [Authorize]
    public class CatsController : ApiController
    {

        private readonly ICatService _catService;

        public CatsController(ICatService catService) => _catService = catService;


       
        [HttpGet]
        public async Task<IEnumerable<CatListingServiceModel>> Mine()
        {
            var userId = this.User.GetId();
            return  await this._catService.ByUser(userId);
        }

        
        [HttpGet]
        [Route(Id)]
        public async Task<ActionResult<CatDetailsServiceModel>> Details(int id)
        {
            return await this._catService.Details(id);
        }
       
        [HttpPost]
        public async Task<ActionResult> Create(CreateCatRequestModel model)
        {
            var userId = this.User.GetId();

            var id = await this._catService.Create(
                model.ImageUrl, 
                model.Description, 
                userId);
           
            return Created(nameof(this.Create), id);
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateCatRequestModel model)
        {
            var userId = this.User.GetId();
            var updated = await this._catService.Update(model.Id, model.Description, userId);
            if (!updated)
            {
                return BadRequest();
            }
            return this.Ok();
        }

        [HttpDelete]
        [Route(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = this.User.GetId();
            var deleted = await this._catService.Delete(id, userId);
            if (!deleted)
            {
                return BadRequest();
            }
            return this.Ok();
        }

    }
}
