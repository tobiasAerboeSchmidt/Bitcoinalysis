using DTOLib;
using DTOLib.DTOs;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Diagnostics;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlocksController : ControllerBase
    {

        private readonly ILogger<BlocksController> _logger;
        private readonly IBlocksRepository _blocksRepository;
        private readonly Histogram _histogram;
        private readonly Stopwatch _stopWatch;

        public BlocksController(ILogger<BlocksController> logger, IBlocksRepository blocksRepository)
        {
            _logger = logger;
            _blocksRepository = blocksRepository;
            _stopWatch = new Stopwatch();
            _histogram = Metrics.CreateHistogram("post_request_duration", "the time in milliseconds for each post request to be processed", new HistogramConfiguration
            {
                Buckets = Histogram.LinearBuckets(start: 10, width: 10, count: 100)
            });
        }

        /// <summary>
        /// Post a single block from the blockchain
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BlockBulkCreateDTO dto)
        {
            _stopWatch.Start();
            await _blocksRepository.CreateBlock(dto.Blocks);
            _stopWatch.Stop();

            _histogram.Observe(_stopWatch.Elapsed.TotalMilliseconds);
            _stopWatch.Reset();

            return Ok();
        }
    }
}