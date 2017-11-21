using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMTest.Models
{
    #region TaskOne
    public interface IGetOrAdd<T, I>
    {
        I GetOrAdd(T t);
    }
    public interface IFirstTaskRepository<T, I>
        : IRepository<T, I>, IGetOrAdd<T, I> where T : class
    {
        
    }
    #endregion
    #region TaskTwo
    public interface IAddOrUpdate<T, I>
    {
        I AddOrUpdate(T t);
    }
    public interface ISecondTaskRepository<T, I>
        : IRepository<T, I>, IAddOrUpdate<T, I> where T : class
    {

    }
    #endregion
    #region TaskThree
    public interface IThirdTaskRepository<T, I> 
        : IRepository<T, I>, IWallet<T, I> where T : class
    {

    }

    public enum MoneyTransferResult
    {
        InvalidSource = 0, InvalidDestination = 1, NotEnoughMoney = 3, InternalError = 4, OK = 5
    }
    public interface IWallet<T, I>
    {
         MoneyTransferResult TrasferMoney(I ID1, I ID2, decimal amount);        
    }
    #endregion
    
    public interface IRepository<T, I>
        where T : class
    {
        void Add(T t);
        void Update(T t);
        void Delete(T t);
        IEnumerable<T> Get();
        IEnumerable<T> Get(Func<T, bool> predicate);
        T FirstOrDefault(Func<T, bool> predicate);
        T Find(I ID);
    }
}
