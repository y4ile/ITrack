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
    public class StudyPlanService : IStudyPlanService
    {
        private readonly IStudyPlanRepository _repository;

        public StudyPlanService(IStudyPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<IEnumerable<StudyPlan>>> GetAll()
        {
            var baseResponse = new BaseResponse<IEnumerable<StudyPlan>>();
            try
            {
                var studyPlans = await _repository.GetAll();
                if (!studyPlans.Any())
                {
                    baseResponse.Data = new List<StudyPlan>();
                    baseResponse.Description = "0 elements found";
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }

                baseResponse.Data = studyPlans;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<StudyPlan>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<StudyPlan>> Get(int id)
        {
            var baseResponse = new BaseResponse<StudyPlan>();
            try
            {
                var studyPlan = await _repository.GetById(id);
                if (studyPlan == null)
                {
                    baseResponse.Description = "Plan not found";
                    baseResponse.StatusCode = StatusCode.PlanNotFound;

                    return baseResponse;
                }

                baseResponse.Data = studyPlan;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudyPlan>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<StudyPlan>> Create(StudyPlan model)
        {
            var baseResponse = new BaseResponse<StudyPlan>();
            try
            {
                await _repository.Create(model);
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudyPlan>()
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
                var studyPlan = await _repository.GetById(id);
                if (studyPlan == null)
                {
                    baseResponse.Description = "Plan not found";
                    baseResponse.StatusCode = StatusCode.PlanNotFound;
                    baseResponse.Data = false;

                    return baseResponse;
                }

                await _repository.Delete(studyPlan);
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

        public async Task<IBaseResponse<StudyPlan>> Edit(int id, StudyPlan updated)
        {
            var baseResponse = new BaseResponse<StudyPlan>();
            try
            {
                var studyPlan = await _repository.GetById(id);
                if (studyPlan == null)
                {
                    baseResponse.Description = "Plan not found";
                    baseResponse.StatusCode = StatusCode.PlanNotFound;

                    return baseResponse;
                }
                
                studyPlan.PlanName = updated.PlanName;
                studyPlan.DirectionId = updated.DirectionId;
                studyPlan.UserId = updated.UserId;

                await _repository.Update(studyPlan);
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = studyPlan;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudyPlan>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
