using Microsoft.AspNetCore.Mvc;
using Vb.Data.Entity;
using Vb.Data;
using Microsoft.EntityFrameworkCore;
using Vb.Api.Response;
using Vb.Api.Validations;
using FluentValidation.Results;
using AutoMapper;

namespace Vb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AccountTransactionController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResponse<List<AccountTransaction>>> Get()
        {
            var accountTransactions = await dbContext.Set<AccountTransaction>()
                .Include(x => x.Account)
                .Where(x => x.IsActive == true)
                .ToListAsync();

            if (accountTransactions.Count == 0)
                return ApiResponse<List<AccountTransaction>>.NotFound("AccountTransaction", null);

            return ApiResponse<List<AccountTransaction>>.Success(accountTransactions);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<AccountTransaction>> Get(int id)
        {
            var accountTransaction = await dbContext.Set<AccountTransaction>()
                .Include(x => x.Account)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (accountTransaction is null)
                return ApiResponse<AccountTransaction>.NotFound("AccountTransaction", id);

            return ApiResponse<AccountTransaction>.Success(accountTransaction);
        }

        [HttpPost]
        public async Task<ApiResponse<AccountTransaction>> Post([FromBody] AccountTransaction accountTransaction)
        {
            AccountTransactionValidator validator = new AccountTransactionValidator();
            ValidationResult result = validator.Validate(accountTransaction);

            if (!result.IsValid)
                return ApiResponse<AccountTransaction>.Failure(result.Errors);

            await dbContext.Set<AccountTransaction>().AddAsync(accountTransaction);
            await dbContext.SaveChangesAsync();

            return ApiResponse<AccountTransaction>.Success(accountTransaction);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<AccountTransaction>> Put(int id, [FromBody] AccountTransaction accountTransaction)
        {
            AccountTransactionValidator validator = new AccountTransactionValidator();
            ValidationResult result = validator.Validate(accountTransaction);

            if (!result.IsValid)
                return ApiResponse<AccountTransaction>.Failure(result.Errors);

            var fromdb = await dbContext.Set<AccountTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<AccountTransaction>.NotFound("AccountTransaction", id);

            mapper.Map(accountTransaction, fromdb);

            await dbContext.SaveChangesAsync();

            return ApiResponse<AccountTransaction>.Success(accountTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<AccountTransaction>> Delete(int id)
        {
            var fromdb = await dbContext.Set<AccountTransaction>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<AccountTransaction>.NotFound("AccountTransaction", id);

            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();

            return ApiResponse<AccountTransaction>.Success();
        }
    }
}
