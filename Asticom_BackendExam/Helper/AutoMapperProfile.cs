using Asticom_BackendExam.Models.DbModel;
using Asticom_BackendExam.Models.Request;
using Asticom_BackendExam.Models.Response;
using AutoMapper;

namespace Asticom_BackendExam.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddUserRequest, UserInfo>();
            CreateMap<UserInfo, UserResponse>();
            CreateMap<UserInfo, UserModel>();
        }
    }
}
