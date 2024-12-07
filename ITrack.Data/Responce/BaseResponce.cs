/*
 *      @Author: yaile
 */

using ITrack.Data.Enum;

namespace ITrack.Data.Responce
{
    public interface IBaseResponse<T>
    {
        StatusCode StatusCode { get; }
        T Data { get; }
    }
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public string Description { get; set; }
    }
}
