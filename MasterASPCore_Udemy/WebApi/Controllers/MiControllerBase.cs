using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiControllerBase : ControllerBase
    {
        private IMediator _mediator;

        /*Se importo manualmente using Microsoft.Extensions.DependencyInjection;*/
        protected IMediator mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

    }
}
