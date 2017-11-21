using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WMTest.Models
{
    public class FirstTaskRepositorySQL
     : Repository<FirstTaskModel, int>, IFirstTaskRepository<FirstTaskModel, int>
    {

        private string command = @"
DECLARE @id AS INT
EXEC @id = [dbo].[GetOrAdd] @Name = '{0}';
SELECT @id";

        public FirstTaskRepositorySQL()
        {
            db = new Context();
            Repo = db.FirstTaskModels;
        }

        public int GetOrAdd(FirstTaskModel t)
        {
            try
            {
                return db.Database.SqlQuery<int>(string.Format(command, t.Name)).ToList().First();
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }
    }
}