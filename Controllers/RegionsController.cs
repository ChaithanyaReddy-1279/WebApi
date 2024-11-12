using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.CustomActionFilters;
using MyWebApi.Models.Domain;
using MyWebApi.Models.DTO;
using MyWebApi.Repositories;

namespace MyWebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAllRegions Actions Method was Invoked");
            //Get data from database to Domain Model
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map Domain Model to DTO
            // var regionsDto = new List<RegionDto>();
            // foreach (var regionDomain in regionsDomain) 
            // {
            //     regionsDto.Add(new RegionDto()
            //     {
            //         Id = regionDomain.Id,
            //         Code = regionDomain.Code,
            //         Name = regionDomain.Name,
            //         RegionImageUrl = regionDomain.RegionImageUrl
            //     });
            // }

            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            logger.LogInformation($"Finshed GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");
            return Ok(regionsDto);
        }

        //Get the region by Id
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Find method takes only primary key. we cannot use this for any other properties.
            //var region = dbContext.Regions.Find(id);
            //using Linq method FirstOrDefault
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            
            //return DTO to Client(Swagger)
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            //Check if region is exist
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();  
            }   

            //Map Domain Model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}