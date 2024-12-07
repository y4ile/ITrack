/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Interfaces;
using ITrack.Data.Responce;
using ITrack.Service.Interfaces;
using ITrack.Data.Enum;

namespace ITrack.Service.Implementations
{
    public class DirectionService : IDirectionService
    {
        private readonly IDirectionRepository _repository;

        public DirectionService(IDirectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<IEnumerable<DevelopmentDirection>>> GetAll()
        {
            var baseResponse = new BaseResponse<IEnumerable<DevelopmentDirection>>();
            try
            {
                var directions = await _repository.GetAll();
                if (!directions.Any())
                {
                    baseResponse.Data = new List<DevelopmentDirection>();
                    baseResponse.Description = "0 elements found";
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }

                baseResponse.Data = directions;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<DevelopmentDirection>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<DevelopmentDirection>> Get(int id)
        {
            var baseResponse = new BaseResponse<DevelopmentDirection>();
            try
            {
                var direction = await _repository.GetById(id);
                if (direction == null)
                {
                    baseResponse.Description = "Direction not found";
                    baseResponse.StatusCode = StatusCode.DirectionNotFound;

                    return baseResponse;
                }

                baseResponse.Data = direction;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<DevelopmentDirection>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<DevelopmentDirection>> Create(DevelopmentDirection model)
        {
            var baseResponse = new BaseResponse<DevelopmentDirection>();
            try
            {
                await _repository.Create(model);
            }
            catch (Exception ex)
            {
                return new BaseResponse<DevelopmentDirection>()
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
                var direction = await _repository.GetById(id);
                if (direction == null)
                {
                    baseResponse.Description = "Direction not found";
                    baseResponse.StatusCode = StatusCode.DirectionNotFound;
                    baseResponse.Data = false;

                    return baseResponse;
                }

                await _repository.Delete(direction);
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

        public async Task<IBaseResponse<DevelopmentDirection>> Edit(int id, DevelopmentDirection updated)
        {
            var baseResponse = new BaseResponse<DevelopmentDirection>();
            try
            {
                var direction = await _repository.GetById(id);
                if (direction == null)
                {
                    baseResponse.Description = "Direction not found";
                    baseResponse.StatusCode = StatusCode.DirectionNotFound;

                    return baseResponse;
                }

                direction.DirectionName = updated.DirectionName;

                await _repository.Update(direction);
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = direction;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<DevelopmentDirection>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
