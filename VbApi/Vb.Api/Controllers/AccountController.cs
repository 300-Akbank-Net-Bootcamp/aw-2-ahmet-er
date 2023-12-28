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
    public class AccountController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AccountController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResponse<List<Account>>> Get()
        {
            var accounts = await dbContext.Set<Account>()
                .Include(x => x.Customer)
                .Include(x => x.AccountTransactions)
                .Include(x => x.EftTransactions)
                .Where(x => x.IsActive == true)
                .ToListAsync();

            if (accounts.Count == 0)
                return ApiResponse<List<Account>>.NotFound("Account", null);

            return ApiResponse<List<Account>>.Success(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<Account>> Get(int id)
        {
            var account = await dbContext.Set<Account>()
                .Include(x => x.Customer)
                .Include(x => x.AccountTransactions)
                .Include(x => x.EftTransactions)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (account is null)
                return ApiResponse<Account>.NotFound("Account", id);

            return ApiResponse<Account>.Success(account);
        }

        [HttpPost]
        public async Task<ApiResponse<Account>> Post([FromBody] Account account)
        {
            AccountValidator validator = new AccountValidator();
            ValidationResult result = validator.Validate(account);

            if (!result.IsValid)
                return ApiResponse<Account>.Failure(result.Errors);

            await dbContext.Set<Account>().AddAsync(account);
            await dbContext.SaveChangesAsync();

            return ApiResponse<Account>.Success(account);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<Account>> Put(int id, [FromBody] Account account)
        {
            AccountValidator validator = new AccountValidator();
            ValidationResult result = validator.Validate(account);

            if (!result.IsValid)
                return ApiResponse<Account>.Failure(result.Errors);

            var fromdb = await dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<Account>.NotFound("Account", id);

            mapper.Map(account, fromdb);
            await dbContext.SaveChangesAsync();

            return ApiResponse<Account>.Success(account);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<Account>> Delete(int id)
        {
            var fromdb = await dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<Account>.NotFound("Account", id);

            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();

            return ApiResponse<Account>.Success();
        }
    }
}
