using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Datalayer.Repositories {
    public abstract class Repository<TValue, TKey> where TValue : class, IIdentifiable<TKey> {
        private ApplicationDbContext context = new ApplicationDbContext();

        public Repository(ApplicationDbContext context) {
            this.context = context;
        }

        public DbSet<TValue> items => context.Set<TValue>();

        public TValue Get(TKey Id) {
            return items.Find(Id);
        }

        public List<TValue> GetAll() {
            return items.ToList();
        }

        public void Add(TValue item) {
            items.Add(item);
        }

        public void Remove(TKey Id) {
            var item = Get(Id);
            items.Remove(item);
        }

        public void Edit(TValue item) {
            context.Set<TValue>().AddOrUpdate(item);
        }

        public void Save() {
            context.SaveChanges();
        }
    }
}