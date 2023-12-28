using Microsoft.AspNetCore.Mvc;
using Vb.Data.Entity;
using Vb.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Vb.Api.Response;
using Vb.Api.Validations;
using FluentValidation.Results;

namespace Vb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EftTransactionController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public EftTransactionController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResponse<List<EftTransaction>>> Get()
        {
            var eftTransactions = await dbContext.Set<EftTransaction>()
                .Include(x => x.Account)
                .Where(x => x.IsActive == true)
                .ToListAsync();

            if (eftTransactions.Count == 0)
                return ApiResponse<List<EftTransaction>>.NotFound("EftTransaction", null);

            return ApiResponse<List<EftTransaction>>.Success(eftTransactions);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<EftTransaction>> Get(int id)
        {
            var eftTransaction = await dbContext.Set<EftTransaction>()
                .Include(x => x.Account)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (eftTransaction is null)
                return ApiResponse<EftTransaction>.NotFound("EftTransaction", id);

            return ApiResponse<EftTransaction>.Success(eftTransaction);
        }

        [HttpPost]
        public async Task<ApiResponse<EftTransaction>> Post([FromBody] EftTransaction eftTransaction)
        {
            EftTransactionValidator validator = new EftTransactionValidator();
            ValidationResult result = validator.Validate(eftTransaction);

            if (!result.IsValid)
                return ApiResponse<EftTransaction>.Failure(result.Errors);

            await dbContext.Set<EftTransaction>().AddAsync(eftTransaction);
            await dbContext.SaveChangesAsync();

            return ApiResponse<EftTransaction>.Success(eftTransaction);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<EftTransaction>> Put(int id, [FromBody] EftTransaction eftTransaction)
        {
            EftTransactionValidator validator = new EftTransactionValidator();
            ValidationResult result = validator.Validate(eftTransaction);

            if (!result.IsValid)
                return ApiResponse<EftTransaction>.Failure(result.Errors);

            var fromdb = await dbContext.Set<EftTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<EftTransaction>.NotFound("EftTransaction", id);

            await dbContext.SaveChangesAsync();

            return ApiResponse<EftTransaction>.Success(eftTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<EftTransaction>> Delete(int id)
        {
            var fromdb = await dbContext.Set<EftTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<EftTransaction>.NotFound("EftTransaction", id);

            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();

            return ApiResponse<EftTransaction>.Success();
        }
    }
}
