using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.BLL.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        void CreateCustomMap<TSource, TDestination>()
        {
            CreateMap<TSource, TDestination>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<TDestination, TSource>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, destMember) => destMember != null)
                );
        }

        CreateCustomMap<User, UserDTO>();
        CreateCustomMap<User, UserAuthDTO>();
        CreateCustomMap<User, UserFormData>();

        CreateCustomMap<Customer, CustomerDTO>();
        CreateCustomMap<Customer, CustomerFormData>();
        CreateCustomMap<Customer, CustomerUpdateFormData>();

        CreateCustomMap<Employee, EmployeeDTO>();
        CreateCustomMap<Employee, EmployeeFormData>();
        CreateCustomMap<Employee, EmployeeUpdateFormData>();

        CreateCustomMap<Product, ProductDTO>();
        CreateCustomMap<Product, ProductFormData>();
        CreateCustomMap<Product, ProductUpdateFormData>();

        CreateCustomMap<Publisher, PublisherDTO>();
        CreateCustomMap<Publisher, PublisherFormData>();
        CreateCustomMap<Publisher, PublisherUpdateFormData>();

        CreateCustomMap<UserFormData, UserDTO>();
        CreateCustomMap<UserFormData, UserAuthDTO>();

        CreateCustomMap<CustomerFormData, CustomerDTO>();
        CreateCustomMap<CustomerUpdateFormData, CustomerDTO>();

        CreateCustomMap<EmployeeFormData, EmployeeDTO>();
        CreateCustomMap<EmployeeUpdateFormData, EmployeeDTO>();

        CreateCustomMap<ProductFormData, ProductDTO>();
        CreateCustomMap<ProductUpdateFormData, ProductDTO>();
    }
}
