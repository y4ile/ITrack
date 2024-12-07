/*
 *      @Author: yaile
 */

namespace ITrack.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<bool> Create(T entity);

        Task<List<T>> GetAll();
        
        Task<T> Update(T entity);

        Task<bool> Delete(T entity);

        Task<T> GetById(int id);
    }
}
