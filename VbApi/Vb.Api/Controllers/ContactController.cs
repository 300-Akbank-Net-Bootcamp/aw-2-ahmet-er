using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Vb.Api.Response;
using Vb.Api.Validations;
using Vb.Data;
using Vb.Data.Entity;

namespace Vb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public ContactController(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResponse<List<Contact>>> Get()
        {
            var contacts = await dbContext.Set<Contact>()
                .Where(x => x.IsActive == true)
                .ToListAsync();

            if (contacts.Count == 0)
                return ApiResponse<List<Contact>>.NotFound("Contact", null);

            return ApiResponse<List<Contact>>.Success(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<Contact>> Get(int id)
        {
            var contact = await dbContext.Set<Contact>()
                .Include(x => x.Customer)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (contact is null)
                return ApiResponse<Contact>.NotFound("Contact", id);

            return ApiResponse<Contact>.Success(contact);
        }

        [HttpPost]
        public async Task<ApiResponse<Contact>> Post([FromBody] Contact contact)
        {
            ContactValidator validator = new ContactValidator();
            ValidationResult result = validator.Validate(contact);

            if (!result.IsValid)
                return ApiResponse<Contact>.Failure(result.Errors);

            await dbContext.Set<Contact>().AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return ApiResponse<Contact>.Success(contact);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<Contact>> Put(int id, [FromBody] Contact contact)
        {
            ContactValidator validator = new ContactValidator();
            ValidationResult result = validator.Validate(contact);

            if (!result.IsValid)
                return ApiResponse<Contact>.Failure(result.Errors);

            var fromdb = await dbContext.Set<Contact>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<Contact>.NotFound("Contact", id);

            mapper.Map(contact, fromdb);

            await dbContext.SaveChangesAsync();

            return ApiResponse<Contact>.Success(contact);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<Contact>> Delete(int id)
        {
            var fromdb = await dbContext.Set<Contact>().FirstOrDefaultAsync(x => x.Id == id);

            if (fromdb is null)
                return ApiResponse<Contact>.NotFound("Contact", id);

            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();

            return ApiResponse<Contact>.Success();
        }
    }
}
