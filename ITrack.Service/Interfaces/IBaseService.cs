/*
 *      @Author: yaile
 */

using ITrack.Data.Responce;

namespace ITrack.Service.Interfaces
{
    public interface IBaseService<T>
    {
        Task<IBaseResponse<IEnumerable<T>>> GetAll();
        Task<IBaseResponse<T>> Get(int id);
        Task<IBaseResponse<T>> Create(T model);
        Task<IBaseResponse<T>> Edit(int id, T updated);
        Task<IBaseResponse<bool>> Delete(int id);
    }
}
