using AutoMapper;
using Microsoft.EntityFrameworkCore;
using z_workshop_server.Data;
using z_workshop_server.DTOs;
using z_workshop_server.Models;

namespace z_workshop_server.Services;

public class EmployeeService
{
    public EmployeeService(IMapper mapper, AppDbContext db) { }
}
