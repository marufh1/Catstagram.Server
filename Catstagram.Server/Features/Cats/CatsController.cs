namespace Catstagram.Server.Features.Cats
{
    using Catstagram.Server.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class CatsController : ApiController
    {
        private readonly ICatService _catService;

        public CatsController(ICatService catService) => _catService = catService;


        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<CatListingResponseModel>> Mine()
        {
            var userId = this.User.GetId();
            return  await this._catService.ByUser(userId);
        }

        [Authorize]
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
    }
}
