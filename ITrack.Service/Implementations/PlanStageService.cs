/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Enum;
using ITrack.Data.Interfaces;
using ITrack.Data.Responce;
using ITrack.Service.Interfaces;

namespace ITrack.Service.Implementations;

public class PlanStageService : IPlanStageService
{
    private readonly IPlanStageRepository _repository;
    
    public PlanStageService(IPlanStageRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IBaseResponse<IEnumerable<PlanStage>>> GetAll()
    {
        var baseResponse = new BaseResponse<IEnumerable<PlanStage>>();
        try
        {
            var planStages = await _repository.GetAll();
            if (!planStages.Any())
            {
                baseResponse.Data = new List<PlanStage>();
                baseResponse.Description = "0 elements found";
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }

            baseResponse.Data = planStages;
            baseResponse.StatusCode = StatusCode.OK;
            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<PlanStage>>()
            {
                Description = $"[GetAll] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    
    public async Task<IBaseResponse<PlanStage>> Get(int id)
    {
        var baseResponse = new BaseResponse<PlanStage>();
        try
        {
            var planStage = await _repository.GetById(id);
            if (planStage == null)
            {
                baseResponse.Description = "Plan stage not found";
                baseResponse.StatusCode = StatusCode.PlanNotFound;

                return baseResponse;
            }

            baseResponse.Data = planStage;
            baseResponse.StatusCode = StatusCode.OK;
            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<PlanStage>()
            {
                Description = $"[Get] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    
    public async Task<IBaseResponse<PlanStage>> Create(PlanStage model)
    {
        var baseResponse = new BaseResponse<PlanStage>();
        try
        {
            await _repository.Create(model);
            baseResponse.Data = model;
            baseResponse.StatusCode = StatusCode.OK;
            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<PlanStage>()
            {
                Description = $"[Create] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    
    public async Task<IBaseResponse<bool>> Delete(int id)
    {
        var baseResponse = new BaseResponse<bool>()
        {
            Data = true
        };
        try
        {
            var planStage = await _repository.GetById(id);
            if (planStage == null)
            {
                baseResponse.Description = "Plan stage not found";
                baseResponse.StatusCode = StatusCode.PlanNotFound;
                baseResponse.Data = false;

                return baseResponse;
            }

            await _repository.Delete(planStage);
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
    
    public async Task<IBaseResponse<PlanStage>> Edit(int id, PlanStage updated)
    {
        var baseResponse = new BaseResponse<PlanStage>();
        try
        {
            var planStage = await _repository.GetById(id);
            if (planStage == null)
            {
                baseResponse.Description = "Plan stage not found";
                baseResponse.StatusCode = StatusCode.PlanNotFound;

                return baseResponse;
            }

            planStage.StageName = updated.StageName;
            planStage.StatusId = updated.StatusId;
            planStage.Priority = updated.Priority;
            planStage.OpenDate = updated.OpenDate;
            planStage.CloseDate = updated.CloseDate;

            await _repository.Update(planStage);
            baseResponse.StatusCode = StatusCode.OK;
            baseResponse.Data = planStage;

            return baseResponse;
        }
        catch (Exception ex)
        {
            return new BaseResponse<PlanStage>()
            {
                Description = $"[Edit] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}