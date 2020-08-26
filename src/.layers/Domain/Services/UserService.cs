using System.Collections.Generic;
using System.Data;
using System.Linq;

using Pharmacies.DataStore;
using Pharmacies.Models;

namespace Pharmacies.Domain.Services
{
    public class UserService
    {
        public IRepository<User> Users { private get; set; }

        public IScopeFactory Scope { private get; set; }

        public IEnumerable<User> GetAll => this.Users.All();

        public IQueryable<User> Query => this.Users.Query;

        public User Get(int id) => this.Users.Get(id);

        public int Insert(User user)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Users.Insert(user);

                unit.Commit();
            }

            return user.Id;
        }

        public void Update(User user)
        {
            if (string.IsNullOrEmpty(user.Password))
            {
                user.IgnoreOnUpdate.Add(nameof(user.Password));
            }

            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {

                this.Users.Update(user);

                unit.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Users.Delete(new User { Id = id });

                unit.Commit();
            }
        }
    }
}
