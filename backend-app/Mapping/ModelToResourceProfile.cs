using AutoMapper;
using backend_app.Application.Resources;
using backend_app.Domain.Models;

namespace backend_app.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Account, ContactResource>();
        }
    }
}