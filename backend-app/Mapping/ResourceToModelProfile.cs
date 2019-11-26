using AutoMapper;
using backend_app.Application.Resources;
using backend_app.Domain.Models;

namespace backend_app.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SignUpResource, Account>();
            CreateMap<LogInResource, Account>();
        }
    }
}