using Microsoft.AspNetCore.Mvc;
using ProzorroDataMining.Application.Interfaces.Services;

namespace ProzorroDataMining.Api.Controllers
{
    /// <summary>
    /// Controller for triggering ETL (Extract, Transform, Load) process
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ETLController : ControllerBase
    {
        private readonly ITenderIngestionService _etlService;

        #region Ctor

        public ETLController(ITenderIngestionService etlService)
        {
            _etlService = etlService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the ETL process to fetch recent tenders from Prozorro API
        /// and store them in the local database.
        /// </summary>
        /// <remarks>
        /// This endpoint triggers the ingestion service which:
        /// - Fetches recent tenders from Prozorro public API
        /// - Validates and transforms the data
        /// - Stores tenders, contracts, and suppliers in the database
        /// </remarks>
        /// <returns>
        /// Returns a success message when the ETL process completes.
        /// </returns>
        /// <response code="200">ETL process completed successfully</response>
        /// <response code="500">Internal server error during ETL process</response>
        [HttpPost("import")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportTenders()
        {
            await _etlService.FetchAndStoreRecentTendersAsync();
            return Ok(new { Message = "ETL process completed successfully" });
        }

        #endregion
    }
}
