using AutoMapper;
using z_workshop_server.DTOs;
using z_workshop_server.Models;
using z_workshop_server.Repositories;

namespace z_workshop_server.Services;

public interface IProductService : IZBaseService<Product, ProductDTO> { }

public class ProductService(IProductRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Product, ProductDTO>(repository, mapper, worker, "Product"),
        IProductService { }
