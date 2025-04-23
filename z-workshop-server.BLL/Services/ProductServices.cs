using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IProductService : IZBaseService<Product, ProductDTO> { }

public class ProductService(IProductRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Product, ProductDTO>(repository, mapper, worker, "Product"),
        IProductService { }
