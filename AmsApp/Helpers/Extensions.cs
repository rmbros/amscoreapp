using Microsoft.AspNetCore.Mvc;

namespace AmsApp.Helpers
{
    public static class Extensions
    {
        public static string NullIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? null : s;
        }

        public static int GetUserId(this ControllerBase controllerBase)
        {
            return Convert.ToInt32(controllerBase.User.Claims.First(c => c.Type.Equals(Helpers.Constants.Strings.AMSClaimIdentifiers.UserId)).Value);
        }

        public static int GetEmployeeId(this ControllerBase controllerBase)
        {
            return Convert.ToInt32(controllerBase.User.Claims.First(c => c.Type.Equals(Helpers.Constants.Strings.AMSClaimIdentifiers.EmployeeId)).Value);
        }

        public static string GetClaim(this ControllerBase controllerBase, string claimName)
        {   
            return controllerBase.User.Claims.First(c => c.Type.Equals(claimName)).Value;
        }
    }
}
