using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WMTest.Models
{
    public class FirstTaskRepository : Repository<FirstTaskModel, int>, IFirstTaskRepository<FirstTaskModel, int>
    {

        public FirstTaskRepository()
        {
            db = new Context();
            Repo = db.FirstTaskModels;
        }
        public int GetOrAdd(FirstTaskModel t)
        {
            FirstTaskModel M = db.FirstTaskModels.FirstOrDefault(F => F.Name == t.Name);
            if (M != null) return M.ID;
            using (TransactionScope scope = new TransactionScope())
            {
                db.FirstTaskModels.Add(t);
                ObjectContext objectContext = ((IObjectContextAdapter)db).ObjectContext;
                objectContext.SaveChanges(SaveOptions.None);
                scope.Complete();
                objectContext.AcceptAllChanges();
            }
            return t.ID;
        }
    }
}