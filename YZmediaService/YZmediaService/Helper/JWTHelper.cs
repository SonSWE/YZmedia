using CommonLib;
using ObjectInfo;
using System.IdentityModel.Tokens.Jwt;

namespace YZmediaService
{
    public static class JWTHelper
    {
        public static YZ_UserInfo? GetUserInfo(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var audienceConfig = Config_Info.Jwt_Issuer;// ConfigData.Audience;
            var tokenHandler = new JwtSecurityTokenHandler();
            //var signingKey = GetSecretInfo();

            if (tokenHandler.CanReadToken(token))
            {
                var tokenRead = tokenHandler.ReadJwtToken(token);

                //var userPid = tokenRead.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.UserPid)?.Value;
                var userPid = tokenRead.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.User_Id)?.Value;

                long.TryParse(userPid ?? string.Empty, out var userPidValue);

                return LoginMem.GetUser(userPidValue);
            }

            return null;
        }

    }
}
