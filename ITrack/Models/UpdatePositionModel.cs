/*
 *      @Author: yaile
 */

namespace ITrack.Models;

public class UpdatePositionModel
{
    public int StageID { get; set; }
    public int NewStatusID { get; set; }
    public List<int> OrderedStageIDs { get; set; }
}