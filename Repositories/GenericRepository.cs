
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EmployeeManagement.Api.Data;

namespace EmployeeManagement.Api.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
        public async Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T> GetByIdAsync(object id) => await _dbSet.FindAsync(id);
        public void Remove(T entity) => _dbSet.Remove(entity);
        public void Update(T entity) => _dbSet.Update(entity);
    }

    public interface IUnitOfWork
    {
        IGenericRepository<EmployeeManagement.Api.Entities.User> Users { get; }
        IGenericRepository<EmployeeManagement.Api.Entities.Employee> Employees { get; }
        IGenericRepository<EmployeeManagement.Api.Entities.Department> Departments { get; }
        IGenericRepository<EmployeeManagement.Api.Entities.Attendance> Attendances { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IGenericRepository<EmployeeManagement.Api.Entities.User> _users;
        private IGenericRepository<EmployeeManagement.Api.Entities.Employee> _employees;
        private IGenericRepository<EmployeeManagement.Api.Entities.Department> _departments;
        private IGenericRepository<EmployeeManagement.Api.Entities.Attendance> _attendances;

        public UnitOfWork(AppDbContext context) { _context = context; }

        public IGenericRepository<EmployeeManagement.Api.Entities.User> Users => _users ??= new GenericRepository<EmployeeManagement.Api.Entities.User>(_context);
        public IGenericRepository<EmployeeManagement.Api.Entities.Employee> Employees => _employees ??= new GenericRepository<EmployeeManagement.Api.Entities.Employee>(_context);
        public IGenericRepository<EmployeeManagement.Api.Entities.Department> Departments => _departments ??= new GenericRepository<EmployeeManagement.Api.Entities.Department>(_context);
        public IGenericRepository<EmployeeManagement.Api.Entities.Attendance> Attendances => _attendances ??= new GenericRepository<EmployeeManagement.Api.Entities.Attendance>(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
