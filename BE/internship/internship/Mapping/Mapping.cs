using AutoMapper;
using internship.Models;
using internship.ModelView.User;
namespace WebApplication1.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //User mapping
            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
