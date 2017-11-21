using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMTest.Models
{
    public class ThirdTaskRepositorySQL : Repository<ThirdTaskModel, int>, IWallet<ThirdTaskModel, int>, IThirdTaskRepository<ThirdTaskModel, int>
    {

        public ThirdTaskRepositorySQL()
        {
            db = new Context();
            Repo = db.ThirdTaskModels;
        }


        private string command = @"
DECLARE @i AS INT
EXEC @i = [dbo].[TransferMoney] 
	@Source = {0},
	@Destination = {1},
	@Ammount = '{2}'
SELECT @i";

        public MoneyTransferResult TrasferMoney(int ID1, int ID2, decimal amount)
        {
            try
            {
                string cmd = string.Format(command, ID1, ID2, amount.ToString());
                var result = db.Database.SqlQuery<int>(cmd).ToList().First();
                return (MoneyTransferResult)result;
            }
            catch (Exception Ex)
            {
                return MoneyTransferResult.InternalError;
            }
        }
    }
}