/*
 *      @Author: yaile
 */

namespace ITrack.Data.Roles
{
    public static class RoleNames
    {
        public const string Admin = "Administrator";
        public const string Mentor = "Mentor";
        public const string User = "User";

        public static IEnumerable<string> AllRoles
        {
            get
            {
                yield return Admin;
                yield return Mentor;
                yield return User;
            }
        }
    }
}
