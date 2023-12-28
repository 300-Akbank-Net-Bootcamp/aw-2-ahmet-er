using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Api.Response;
using Vb.Api.Validations;
using Vb.Data;
using Vb.Data.Entity;

namespace Vb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public CustomersController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ApiResponse<List<Customer>>> Get()
        {
            var customers = await dbContext.Set<Customer>()
                .Where(x => x.IsActive == true)
                .ToListAsync();

            if (customers.Count == 0)
                return ApiResponse<List<Customer>>.NotFound("Customer", null);

            return ApiResponse<List<Customer>>.Success(customers);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<Customer>> Get(int id)
        {
            var customer = await dbContext.Set<Customer>()
                .Include(x => x.Accounts)
                .Include(x => x.Addresses)
                .Include(x => x.Contacts)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer is null)
                return ApiResponse<Customer>.NotFound("Customer", id);

            return ApiResponse<Customer>.Success(customer);
        }

        [HttpPost]
        public async Task<ApiResponse<Customer>> Post([FromBody] Customer customer)
        {
            CustomerValidator validator = new CustomerValidator();
            ValidationResult result = validator.Validate(customer);

            if (!result.IsValid)
                return ApiResponse<Customer>.Failure(result.Errors);

            await dbContext.Set<Customer>().AddAsync(customer);
            await dbContext.SaveChangesAsync();

            return  ApiResponse<Customer>.Success(customer);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<Customer>> Put(int id, [FromBody] Customer customer)
        {
            CustomerValidator validator = new CustomerValidator();
            ValidationResult result = validator.Validate(customer);

            if (!result.IsValid)
                return ApiResponse<Customer>.Failure(result.Errors);

            var fromdb = await dbContext.Set<Customer>().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (fromdb is null)
                return ApiResponse<Customer>.NotFound("Customer", id);

            mapper.Map(customer, fromdb);

            await dbContext.SaveChangesAsync();

            return ApiResponse<Customer>.Success(customer);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<Customer>> Delete(int id)
        {
            var fromdb = await dbContext.Set<Customer>().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (fromdb is null)
                return ApiResponse<Customer>.NotFound("Customer", id);

            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();

            return ApiResponse<Customer>.Success();
        }
    }
}
