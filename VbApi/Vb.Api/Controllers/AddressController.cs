using Microsoft.AspNetCore.Mvc;
using Vb.Data.Entity;
using Vb.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Vb.Api.Response;
using Vb.Api.Validations;
using FluentValidation.Results;
using System.Net;

namespace Vb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AddressController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResponse<List<Address>>> Get()
        {
            var addresses = await dbContext.Set<Address>()
                .Include(x => x.Customer)
                .Where(x => x.IsActive == true)
                .ToListAsync();

            if (addresses.Count == 0)
                return ApiResponse<List<Address>>.NotFound("Address", null);

            return ApiResponse<List<Address>>.Success(addresses);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<Address>> Get(int id)
        {
            var address = await dbContext.Set<Address>()
                .Include(x => x.Customer)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (address is null)
                return ApiResponse<Address>.NotFound("Address", id);

            return ApiResponse<Address>.Success(address);
        }

        [HttpPost]
        public async Task<ApiResponse<Address>> Post([FromBody] Address address)
        {
            AddressValidator validator = new AddressValidator();
            ValidationResult result = validator.Validate(address);

            if (!result.IsValid)
                return ApiResponse<Address>.Failure(result.Errors);

            await dbContext.Set<Address>().AddAsync(address);
            await dbContext.SaveChangesAsync();

            return ApiResponse<Address>.Success(address);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<Address>> Put(int id, [FromBody] Address address)
        {
            AddressValidator validator = new AddressValidator();
            ValidationResult result = validator.Validate(address);

            if (!result.IsValid)
                return ApiResponse<Address>.Failure(result.Errors);

            var fromdb = await dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<Address>.NotFound("Address", id);

            mapper.Map(address, fromdb);

            await dbContext.SaveChangesAsync();

            return ApiResponse<Address>.Success(address);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<Address>> Delete(int id)
        {
            var fromdb = await dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<Address>.NotFound("Address", id);

            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();

            return ApiResponse<Address>.Success();
        }
    }
}
