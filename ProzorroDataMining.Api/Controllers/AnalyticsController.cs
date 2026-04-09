using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProzorroDataMining.Application.DTOs;
using ProzorroDataMining.Application.Interfaces.Services;

namespace ProzorroDataMining.Api.Controllers
{
    /// <summary>
    /// Provides analytics endpoints for tenders, contracts, and suppliers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IMapper _mapper;

        #region Ctor

        public AnalyticsController(IAnalyticsService analyticsService, IMapper mapper)
        {
            _analyticsService = analyticsService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the total savings across all tenders.
        /// </summary>
        /// <remarks>
        /// This endpoint calculates the difference between planned budgets and actual contract amounts
        /// to determine the total savings achieved in the procurement process.
        /// </remarks>
        /// <returns>A DTO containing the total savings value.</returns>
        /// <response code="200">Returns the total savings successfully</response>
        /// <response code="500">Internal server error while calculating savings</response>
        [HttpGet("savings")]
        [ProducesResponseType(typeof(SavingsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SavingsDto>> GetTotalSavings()
        {
            var savings = await _analyticsService.GetTotalSavingsAsync();
            var savingsDto = _mapper.Map<SavingsDto>(savings);
            return Ok(savingsDto);
        }

        /// <summary>
        /// Gets the top procuring entities ranked by total contract amount.
        /// </summary>
        /// <remarks>
        /// This endpoint aggregates contract amounts by procuring entity and returns
        /// the top entities with the largest spending.
        /// </remarks>
        /// <returns>A list of procurer statistics DTOs.</returns>
        /// <response code="200">Returns the list of top procurers successfully</response>
        /// <response code="500">Internal server error while fetching procurer statistics</response>
        [HttpGet("top-procurers")]
        [ProducesResponseType(typeof(IEnumerable<ProcurerStatsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProcurerStatsDto>>> GetTopProcurers()
        {
            var procurers = await _analyticsService.GetTopProcurersAsync();
            var procurerDtos = _mapper.Map<IEnumerable<ProcurerStatsDto>>(procurers);
            return Ok(procurerDtos);
        }

        /// <summary>
        /// Gets the top suppliers ranked by total contract amount.
        /// </summary>
        /// <remarks>
        /// This endpoint aggregates contract amounts by supplier name and returns
        /// the top suppliers with the largest awarded amounts.
        /// </remarks>
        /// <returns>A list of supplier statistics DTOs.</returns>
        /// <response code="200">Returns the list of top suppliers successfully</response>
        /// <response code="500">Internal server error while fetching supplier statistics</response>
        [HttpGet("top-suppliers")]
        [ProducesResponseType(typeof(IEnumerable<SupplierStatsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SupplierStatsDto>>> GetTopSuppliers()
        {
            var suppliers = await _analyticsService.GetTopSuppliersAsync();
            var supplierDtos = _mapper.Map<IEnumerable<SupplierStatsDto>>(suppliers);
            return Ok(supplierDtos);
        }

        #endregion
    }
}
