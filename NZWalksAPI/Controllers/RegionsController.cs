using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    // https://localhost:1234/api/regions the url will looks similarly.
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        public RegionsController(NZWalksDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        // GET ALL REGIONS
        // GET: https://localhost:portnumber:api/regions this is the restfull url
        [HttpGet]
        public IActionResult GetAll()
        {
            // Get data from database --> domain models
            var regions = _dbContext.Regions.ToList(); // That return us to Region table datas.

            //map domain models to DTOs
            var regionsDto = new List<RegionDto>(); // in later lessons we will use AutoMapper
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            
            // return DTOs
            return Ok(regions);
        }

        //Get single region (Get region by id)
        //Get: https//localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")] // this id pair to method parameter (Guid id) if we dont do that we will get error message.We do type safe route id:Guid
        public IActionResult GetById(Guid id) 
        {
            var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if(regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // return RegionDto
            return Ok(regionDto);
        }
        //post to create new region
        //post:https//localhost:portnumber/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // map or convert dto to domain model
            //use domain model to create region
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            _dbContext.Regions.Add(regionDomainModel);
            _dbContext.SaveChanges();

            // map domain model back to dto

            var regionDto = new RegionDto // later lessons we will use AutoMapper
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById),new {id = regionDomainModel.Id},regionDto); // return 201 http response that means created succesfully

        }
        // Update Region
        // put: https//localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // check if region exists
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map Dto to domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            // we can set the property to domain model we dont need update method from the dbcontext so we dont call it we write just save changes.

            _dbContext.SaveChanges();

            // Convert Domain Model to Dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
        //Delete region
        //delete:https//localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomainModel == null) 
            {
                return NotFound();
            }

            // if we find delete it

            _dbContext.Regions.Remove(regionDomainModel);
            _dbContext.SaveChanges(); // if we dont do that the entity will not delete on database

            // return the deleted region back
            // map domain model to dto

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }

    }
}
