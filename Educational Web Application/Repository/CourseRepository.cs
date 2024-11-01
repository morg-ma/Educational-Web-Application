using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDBContext _context;

        public CourseRepository(AppDBContext context)
        {
            _context = context;
        }
        public void Add(Course obj)
        {
            _context.Add(obj);
        }

        public void Delete(int id)
        {
            var crs = _context.Courses.Include(cr => cr.CourseResults).Include(i => i.Instructors).FirstOrDefault(c => c.Id == id);
            if (crs != null)
            {
                _context.CourseResults.RemoveRange(crs.CourseResults);
                _context.Instructors.RemoveRange(crs.Instructors);
                _context.Remove(crs);
            }
            
        }

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public IQueryable<Course> GetAllByName(string name)
        {
            return _context.Courses.Include(d => d.Department).Where(c => c.Name.StartsWith(name));
        }

        public IQueryable<Course> GetAllWithDepart()
        {
            return _context.Courses.Include(d => d.Department).AsQueryable();
        }

        public Course GetById(int id)
        {
            return _context.Courses.Find(id);
        }

        public Course GetByIdWithDept(int id)
        {
            return _context.Courses.Include(d => d.Department).FirstOrDefault(c => c.Id == id);
        }

        public List<Course> GetCoursesInDepartment(int departmentId)
        {
            return _context.Courses.Where(c => c.DepartmentID == departmentId).Include(d => d.Department).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Course obj)
        {
            _context.Update(obj);
        }
    }
}
