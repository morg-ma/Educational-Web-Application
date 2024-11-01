using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.Repository
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly AppDBContext _context;

        public InstructorRepository(AppDBContext context)
        {
            _context = context;
        }
        // CRUD
        public void Add(Instructor ins)
        {
            _context.Add(ins);
        }
        public void Update(Instructor ins)
        {
            _context.Update(ins);
        }
        public void Delete (int id)
        {
            var ins = GetById(id);
            _context.Remove(ins);
        }
        public List<Instructor> GetAll()
        {
            return _context.Instructors.ToList();
        }
        public Instructor GetById(int id)
        {
            return _context.Instructors.Find(id);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<Instructor> GetInstructorsWithDepartAndCrs()
        {
            return _context.Instructors.Include(d => d.Department).Include(c => c.Course).AsQueryable();
        }

        public Instructor? GetByIdWithDepartAndCrs(int? id)
        {
            return _context.Instructors
                    .Include(d => d.Department)
                    .Include(c => c.Course)
                    .FirstOrDefault(i => i.Id == id);
        }

        public IQueryable<Instructor> GetAllByName(string name)
        {
            return _context.Instructors
                    .Include(d => d.Department)
                    .Include(c => c.Course)
                    .Where(i => i.Name.Contains(name));
        }
    }
}
