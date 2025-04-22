using AutoMapper;
using z_workshop_server.DTOs;
using z_workshop_server.Models;

namespace z_workshop_server.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, UserAuthDTO>().ReverseMap();
        CreateMap<User, UserFormData>().ReverseMap();

        CreateMap<Customer, CustomerDTO>().ReverseMap();
        CreateMap<Customer, CustomerFormData>().ReverseMap();

        CreateMap<Employee, EmployeeDTO>().ReverseMap();
        CreateMap<Employee, EmployeeFormData>().ReverseMap();

        CreateMap<UserFormData, UserDTO>().ReverseMap();
        CreateMap<UserFormData, UserAuthDTO>().ReverseMap();
        CreateMap<UserFormData, UserUpdateRequest>().ReverseMap();

        CreateMap<CustomerFormData, CustomerDTO>().ReverseMap();

        CreateMap<EmployeeFormData, EmployeeDTO>().ReverseMap();
    }
}
