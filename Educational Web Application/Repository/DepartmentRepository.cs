using EducationalWebApplication.Data;
using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDBContext _context;

        public DepartmentRepository(AppDBContext context)
        {
            _context = context;
        }
        public void Add(Department obj)
        {
            _context.Add(obj);
        }

        public void Delete(int id)
        {
            var dept = GetById(id);
            _context.Remove(dept);
        }

        public List<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public IQueryable<Department> GetAllByName(string name)
        {
            return _context.Departments.Where(d => d.Name.StartsWith(name));
        }

        public IQueryable<Department> GetAllQuery()
        {
            return _context.Departments.AsQueryable();
        }

        public Department GetById(int id)
        {
            return _context.Departments.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Department obj)
        {
            _context.Update(obj);
        }
    }
}
