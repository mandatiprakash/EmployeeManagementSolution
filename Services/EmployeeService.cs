
using AutoMapper;
using EmployeeManagement.Api.DTOs.Employee;
using EmployeeManagement.Api.Entities;
using EmployeeManagement.Api.Repositories;

namespace EmployeeManagement.Api.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateAsync(EmployeeCreateDto dto);
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, EmployeeUpdateDto dto);
        Task DeleteAsync(int id);
        Task BulkCreateAsync(IEnumerable<EmployeeCreateDto> dtos);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public EmployeeService(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

        public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto dto)
        {
            var e = _mapper.Map<Employee>(dto);
            await _uow.Employees.AddAsync(e);
            await _uow.SaveChangesAsync();
            return _mapper.Map<EmployeeDto>(e);
        }

        public async Task BulkCreateAsync(IEnumerable<EmployeeCreateDto> dtos)
        {
            var entities = dtos.Select(d => _mapper.Map<Employee>(d)).ToList();
            await _uow.Employees.AddRangeAsync(entities);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _uow.Employees.GetByIdAsync(id);
            if (e == null) throw new Exception("Not found");
            _uow.Employees.Remove(e);
            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var list = await _uow.Employees.GetAllAsync();
            return list.Select(e => _mapper.Map<EmployeeDto>(e)).ToList();
        }

        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var e = await _uow.Employees.GetByIdAsync(id);
            return e == null ? null : _mapper.Map<EmployeeDto>(e);
        }

        public async Task UpdateAsync(int id, EmployeeUpdateDto dto)
        {
            var e = await _uow.Employees.GetByIdAsync(id);
            if (e == null) throw new Exception("Not found");
            _mapper.Map(dto, e);
            _uow.Employees.Update(e);
            await _uow.SaveChangesAsync();
        }
    }
}
