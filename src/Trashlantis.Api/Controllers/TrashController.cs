namespace Trashlantis.Api.Controllers
{
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;


    [ApiController]
    [Route("[controller]")]
    public class TrashController :
        ControllerBase
    {
        readonly ILogger<TrashController> _logger;
        readonly IPublishEndpoint _publishEndpoint;

        public TrashController(ILogger<TrashController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Get(string binNumber)
        {
            _logger.LogInformation("Time to take out the trash: {BinNumber}", binNumber);

            await _publishEndpoint.Publish<TakeOutTheTrash>(new {BinNumber = binNumber});

            return Accepted();
        }
    }
}