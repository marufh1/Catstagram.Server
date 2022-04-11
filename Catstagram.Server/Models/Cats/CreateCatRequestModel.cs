using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catstagram.Server.Models.Cats
{
    public class CreateCatRequestModel
    {
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
