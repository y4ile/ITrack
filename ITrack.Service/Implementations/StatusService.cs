/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Enum;
using ITrack.Data.Interfaces;
using ITrack.Data.Responce;
using ITrack.Service.Interfaces;

namespace ITrack.Service.Implementations
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repository;

        public StatusService(IStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<IEnumerable<Status>>> GetAll()
        {
            var baseResponse = new BaseResponse<IEnumerable<Status>>();
            try
            {
                var statuses = await _repository.GetAll();
                if (!statuses.Any())
                {
                    baseResponse.Data = new List<Status>();
                    baseResponse.Description = "0 elements found";
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }

                baseResponse.Data = statuses;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Status>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Status>> Get(int id)
        {
            var baseResponse = new BaseResponse<Status>();
            try
            {
                var status = await _repository.GetById(id);
                if (status == null)
                {
                    baseResponse.Description = "Status not found";
                    baseResponse.StatusCode = StatusCode.StatusNotFound;

                    return baseResponse;
                }

                baseResponse.Data = status;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Status>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Status>> Create(Status model)
        {
            var baseResponse = new BaseResponse<Status>();
            try
            {
                await _repository.Create(model);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Status>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            baseResponse.StatusCode = StatusCode.OK;
            return baseResponse;
        }

        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            var baseResponse = new BaseResponse<bool>()
            {
                Data = true
            };
            try
            {
                var status = await _repository.GetById(id);
                if (status == null)
                {
                    baseResponse.Description = "Status not found";
                    baseResponse.StatusCode = StatusCode.StatusNotFound;
                    baseResponse.Data = false;

                    return baseResponse;
                }

                await _repository.Delete(status);
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[Delete] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Status>> Edit(int id, Status updated)
        {
            var baseResponse = new BaseResponse<Status>();
            try
            {
                var status = await _repository.GetById(id);
                if (status == null)
                {
                    baseResponse.Description = "Status not found";
                    baseResponse.StatusCode = StatusCode.StatusNotFound;

                    return baseResponse;
                }

                status.StatusName = updated.StatusName;

                await _repository.Update(status);
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = status;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Status>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
