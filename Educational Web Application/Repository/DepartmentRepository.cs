using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using Microsoft.EntityFrameworkCore;

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
            // First remove the instructors, courses, and the trainees whoes in the department
            var dept = _context.Departments.Include(i => i.Instructors).Include(c => c.Courses).Include(t => t.Trainees).FirstOrDefault(d => d.Id == id);
            if (dept != null)
            {
                _context.Courses.RemoveRange(dept.Courses);
                _context.Instructors.RemoveRange(dept.Instructors);
                _context.Trainees.RemoveRange(dept.Trainees);
                _context.Remove(dept);
            }
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

        public Department GetDeptWithCourses(int id)
        {
            return _context.Departments.Include(c => c.Courses).FirstOrDefault(d => d.Id == id);
        }

        public Department GetDeptWithInstructors(int id)
        {
            return _context.Departments.Include(c => c.Instructors).ThenInclude(c => c.Course).FirstOrDefault(d => d.Id == id);
        }

        public Department GetDeptWithTrainees(int id)
        {
            return _context.Departments.Include(c => c.Trainees).FirstOrDefault(d => d.Id == id);
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
