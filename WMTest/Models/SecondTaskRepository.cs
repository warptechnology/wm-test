using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WMTest.Models
{
    public class SecondTaskRepository : Repository<SecondTaskModel, int>, ISecondTaskRepository<SecondTaskModel, int>
    { 
        public SecondTaskRepository()
        {
            db = new Context();
            Repo = db.SecondTaskModels;
        }
        public int AddOrUpdate(SecondTaskModel t)
        {
            SecondTaskModel M = db.SecondTaskModels.Find(t.ID);
            int ID = t.ID;
            using (TransactionScope scope = new TransactionScope())
            {
                if (M == null)
                {
                    SecondTaskModel stm = new SecondTaskModel() { Value = t.Value };
                    db.SecondTaskModels.Add(stm);
                    ID = stm.ID;
                }
                else
                {
                    M.Value = t.Value;
                    db.Entry(M).State = System.Data.Entity.EntityState.Modified;
                }
                ObjectContext objectContext = ((IObjectContextAdapter)db).ObjectContext;
                objectContext.SaveChanges(SaveOptions.None);
                scope.Complete();
                objectContext.AcceptAllChanges();
            }
            return ID;
        }
    }
}