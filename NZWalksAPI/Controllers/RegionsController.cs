using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    // https://localhost:1234/api/regions the url will looks similarly.
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper= mapper;
        }
        // GET ALL REGIONS
        // GET: https://localhost:portnumber:api/regions this is the restfull url
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database --> domain models
            var regionsDomain = await _regionRepository.GetAllAsync(); // That return us to Region table datas.When we do it this lines async it will work asychenronus.İf process stop or late it's going on the next lines
            // return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //Get single region (Get region by id)
        //Get: https//localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")] // this id pair to method parameter (Guid id) if we dont do that we will get error message.We do type safe route id:Guid
        public async Task<IActionResult> GetById(Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id); // we implented repository pattern to controller getbyıd method

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert Region Domain Model to Region DTO
            // return RegionDto
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }
        //post to create new region
        //post:https//localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto); // addRegionRequestDto to Region mappings with Automapper

            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            var regionDto = _mapper.Map<RegionDto>(regionDomainModel); // regionDomainModel to RegionDto mappings with AutoMapper

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto); // return 201 http response that means created succesfully

        }
        // Update Region
        // put: https//localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

            // check if region exists
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<UpdateRegionRequestDto>(regionDomainModel));
        }
        //Delete region
        //delete:https//localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel =  await _regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // return the deleted region back

            return Ok(_mapper.Map<RegionDto>(regionDomainModel)); // regionDomainModel to RegionDto mappings with AutoMapper..!
        }

    }
}
