using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WMTest.Models
{
    public class SecondTaskRepositorySQL : Repository<SecondTaskModel, int>, ISecondTaskRepository<SecondTaskModel, int>
    {
        public SecondTaskRepositorySQL()
        {
            db = new Context();
            Repo = db.SecondTaskModels;
        }

        private string command = 
@"DECLARE @id AS INT;
EXEC @id = [dbo].[AddOrUpdate] @id = {0}, @Value = {1};
SELECT @id;";

        public int AddOrUpdate(SecondTaskModel t)
        {
            try
            {
                string cmd = string.Format(command, t.ID, t.Value);
                var result = db.Database.SqlQuery<int>(cmd).ToList().First();
                return result;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }
    }
}