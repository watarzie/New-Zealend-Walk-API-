using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }
        //GET Walks
        //GET: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var walksDomainModel = await _walkRepository.GetAllAsync();
            return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
        }
        //Create Walk
        //Post:/api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map DTO to Domain Model
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto); // addWalkRequestDto to Walk mappings with AutoMapper

            await _walkRepository.CreateAsync(walkDomainModel);

            //Map Domain Model to Dto
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
