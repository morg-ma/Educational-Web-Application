using EducationalWebApplication.Data;
using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDBContext _context;

        public EnrollmentRepository(AppDBContext context)
        {
            _context = context;
        }
        public void Add(CourseResult obj)
        {
            _context.Add(obj);
        }
        public void Update(CourseResult obj)
        {
            _context.Update(obj);
        }
        public void Delete(int id)
        {
            var cr = GetById(id);
            _context.Remove(cr);
        }

        public List<CourseResult> GetAll()
        {
            return _context.CourseResults.ToList();
        }

        public CourseResult GetById(int id)
        {
            return _context.CourseResults.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
