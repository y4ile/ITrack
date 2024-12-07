/*
 *      @Author: yaile
 */

namespace ITrack.Data.Enum
{
    public enum StatusCode
    {
        UserNotFound = 0,
        IdentityResultFailed = 1,
        DirectionNotFound = 10,
        StageNotFound = 20,
        StatusNotFound = 30,
        PlanNotFound = 40,

        OK = 200,
        InternalServerError = 500
    }
}
