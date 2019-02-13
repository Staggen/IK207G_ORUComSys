using Datalayer.Models;

namespace Datalayer.Repositories {
    public class CategoryRepository : Repository<CategoryModels, int> {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}