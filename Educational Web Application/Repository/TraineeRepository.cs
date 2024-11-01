using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.Repository
{
    public class TraineeRepository : ITraineeRepository
    {
        private readonly AppDBContext _context;

        public TraineeRepository(AppDBContext context)
        {
            _context = context;
        }
        public void Add(Trainee obj)
        {
            _context.Add(obj);
        }
        public void Update(Trainee obj)
        {
            _context.Update(obj);
        }
        public void Delete(int id)
        {
            var trainee = _context.Trainees.Include(cr => cr.CourseResults).FirstOrDefault(t => t.Id == id);
            if (trainee != null)
            {
                _context.CourseResults.RemoveRange(trainee.CourseResults);
                _context.Remove(trainee);
            }
        }
        public List<Trainee> GetAll()
        {
            return _context.Trainees.ToList();
        }
        public Trainee GetById(int id)
        {
            return _context.Trainees.Find(id);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<Trainee> GetAllWithDept()
        {
            return _context.Trainees.Include(t => t.Department);
        }

        public async Task<Trainee> GetByIdWithCrs(int id)
        {
            return await _context.Trainees
                .Include(t => t.Department).Include(cr => cr.CourseResults)
                                           .ThenInclude(c => c.Course)
                                           .ThenInclude(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public IQueryable<Trainee> GetAllByName(string name)
        {
            return _context.Trainees
                    .Include(d => d.Department)
                    .Where(i => i.Name.Contains(name));
        }
    }
}
