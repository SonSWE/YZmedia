using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace YZmediaService
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FunctionAuthorizeFilterAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public bool CheckRight { get; set; } = true;

        /// <summary>
        /// FuncCode trong bảng SaFunction.
        /// </summary>
        public string FuncCode { get; set; } = string.Empty;
        /// <summary>
        /// Danh sách các FuncCode trong bảng SaFunction.
        /// </summary>
        public string[] FuncCodes { get; set; } = Array.Empty<string>();

        //
        public FunctionAuthorizeFilterAttribute(params string[] funcCodes)
        {
            FuncCodes = funcCodes;
        }

        //
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Request.GetUserInfo();
            if (user == null || user.User_Id <= 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // 
            if (CheckRight && !CheckRightByFuncCode((long)user.User_Id, context))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        private bool CheckRightByFuncCode(long userPid, AuthorizationFilterContext context)
        {
            var functionsHasRight = LoginMem.GetFunctionsByUserPid(userPid);

            // 1. Ưu tiên check theo setup ở FuncCode trước.
            // 2. Nếu không có mới check ở FuncCodes.
            // 3. Cả 2 option check theo FuncCode không set thì check theo request path.
            if (!string.IsNullOrEmpty(FuncCode))
            {
                return functionsHasRight?.Any(x => x.Function_Id?.Equals(FuncCode) == true) ?? false;
            }
            else if (FuncCodes?.Length > 0)
            {
                return functionsHasRight?.Any(x => !string.IsNullOrEmpty(x.Function_Id) && FuncCodes.Contains(x.Function_Id)) ?? false;
            }
            else
            {
                //string reqPath = context.HttpContext.Request.Path.Value ?? string.Empty;

                //if (!string.IsNullOrEmpty(reqPath))
                //{
                //    return functionsHasRight?.Any(x => x.FuncUrl?.Equals(reqPath, StringComparison.OrdinalIgnoreCase) == true) ?? false;
                //}
            }

            return false;
        }
    }
}
