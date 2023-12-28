using AutoMapper;
using Vb.Data.Entity;

namespace Vb.Api.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, Account>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
