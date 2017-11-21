using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WMTest.Models
{
    public class ThirdTaskRepository : Repository<ThirdTaskModel, int>, IWallet<ThirdTaskModel, int>, IThirdTaskRepository<ThirdTaskModel, int>
    {

        public ThirdTaskRepository()
        {
            db = new Context();
            Repo = db.ThirdTaskModels;
        }
        public MoneyTransferResult TrasferMoney(int ID1, int ID2, decimal amount)
        {
            var Result = MoneyTransferResult.InternalError;
            using (TransactionScope scope = new TransactionScope())
            {
                ThirdTaskModel Wallet1 = Repo.Find(ID1);
                if (null == Wallet1) Result = MoneyTransferResult.InvalidSource;
                ThirdTaskModel Wallet2 = Repo.Find(ID2);
                if (null == Wallet2) Result = MoneyTransferResult.InvalidDestination;
                if (Wallet1.Balance < amount) Result = MoneyTransferResult.NotEnoughMoney;
                Wallet1.Balance -= amount;
                Wallet2.Balance += amount;
                db.Entry(Wallet1).State = System.Data.Entity.EntityState.Modified;
                db.Entry(Wallet2).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                ObjectContext objectContext = ((IObjectContextAdapter)db).ObjectContext;
                objectContext.SaveChanges(SaveOptions.None);
                scope.Complete();
                objectContext.AcceptAllChanges();

                Result = MoneyTransferResult.OK;
            }

            return Result;
        }
    }
}