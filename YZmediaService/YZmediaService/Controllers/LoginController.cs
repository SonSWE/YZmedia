using CommonLib;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ObjectInfo;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace YZmediaService.Controllers
{
    public class LoginController : Controller
    {
        [Route("api/sso/get-jwt")]
        public ActionResult<UserInfo> Login(string p_user_name)
        {
            Logger.log.Debug("begin user_name " + p_user_name);

            try
            {
                IActionResult response = Unauthorized();
                string _key = CommonFunc.DecryptString_AES(p_user_name);
                if (_key.Contains("dangtq0751060770") && _key.Contains("halu0212"))
                {
                    UserInfo userInfo = new UserInfo
                    {
                        User_Name = "XXXX",
                        Reference_Id = 0,
                        User_Type = 0
                    };

                    var tokenString = GenerateJSONWebToken(userInfo);
                    return Json(new { success = "1", key = tokenString });
                }
                else
                {
                    return Json(new { success = "0", key = "" });
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { success = "0", key = "" });
            }
        }

        private string GenerateJSONWebToken(UserInfo userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config_Info.Jwt_Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,  userInfo.User_Name),
                new Claim("reference_id", userInfo.Reference_Id.ToString()),
                new Claim("reference_type", userInfo.User_Type.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(Config_Info.Jwt_Issuer,
                Config_Info.Jwt_Issuer,
                claims,
                signingCredentials: credentials);

            if (Config_Info.TimeOutLogin > 0)
            {
                token = new JwtSecurityToken(Config_Info.Jwt_Issuer,
                Config_Info.Jwt_Issuer,
                claims,
                expires: DateTime.Now.AddHours(Config_Info.TimeOutLogin),
                signingCredentials: credentials);
            }

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserInfo AuthenticateUser(string p_user_name, string p_password)
        {
            try
            {
                User_DA user_DA = new User_DA();
                UserInfo userInfo = user_DA.User_Login(p_user_name, p_password);
                if (userInfo != null && userInfo.User_Id > 0)
                {
                    DataSet ds = user_DA.User_Rights_GetByUser(userInfo.User_Id);
                    List<UserFunction> _lst = CBO<UserFunction>.FillCollection_FromDataSet(ds);
                    userInfo.FunctionSettings = _lst;
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, ex.Message);
            }
            return null;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/sso/auth")]
        public async Task<IActionResult> Auth([FromBody] TokenRequestParams parameters)
        {
            try
            {
                if (parameters.Grant_type == "password")
                {
                    return await DoCheckPassword(parameters);
                }
                else if (parameters.Grant_type == "refresh_token")
                {
                    return await DoRefreshToken(parameters);
                }
                else if (parameters.Grant_type == "invalidate_token")
                {
                    return DoInvalidateToken(parameters);
                }
                else
                {
                    return Ok(new BaseInfo() { Code = -1, Message = "Invalid grant type." });
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, ex.Message);
                return Ok(new BaseInfo() { Code = -1, Message = ex.ToString() });
            }
        }

        [Authorize]
        [HttpGet]
        [FunctionAuthorizeFilter(CheckRight = false)]
        [Route("api/sso/checkalive")]
        public IActionResult CheckAlive()
        {
            var userInfo = Request.GetUserInfo();
            if (userInfo != null && userInfo.User_Id > 0) return Ok();
            else return Unauthorized();
        }

        private async Task<IActionResult> DoCheckPassword(TokenRequestParams parameters)
        {
            // Lấy cấu hình có sử dụng ldap khi đăng nhập không
            if (parameters.User_Name == null || parameters.User_Name == "")
            {
                return Ok(new BaseInfo() { Code = -1, Message = "User không được để trống" });
            }

            if (parameters.Password == null || parameters.Password == "")
            {
                return Ok(new BaseInfo() { Code = -1, Message = "Password không được để trống" });
            }
            Logger.log.Debug("begin login user_name " + parameters.User_Name + " login password " + parameters.Password);

            var user = AuthenticateUser(parameters.User_Name, parameters.Password);

            if (user != null && user.User_Name != null)
            {
                Logger.log.Debug("done login user_name " + parameters.User_Name + " login password " + parameters.Password);

                var now = DateTime.UtcNow;
                var refresh_token = new IdentityRefreshToken
                {
                    Identity = user.User_Id.ToString(),
                    RefreshToken = Guid.NewGuid().ToString("N"),
                    IssueTimeUtc = DateTime.UtcNow,
                    ExpiryTimeUtc = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1),
                };

                var returnvalue = await GetJwtAsync(refresh_token.RefreshToken, user);

                //store the refresh_token
                refresh_token.AccessToken = returnvalue.Item2;
                LoginMem.SetRefreshToken(refresh_token);

                //store the sauser
                LoginMem.SetUser(user);

                return Ok(returnvalue.Item1);
            }
            else
            {
                Logger.log.Debug("NULL login user_name " + parameters.User_Name + " login password " + parameters.Password);

                return Ok(new BaseInfo() { Code = -1, Message = "Tài khoản hoặc mật khẩu không đúng" });
            }
        }

        private async Task<IActionResult> DoRefreshToken(TokenRequestParams parameters)
        {
            //var browserInfo = Request.GetBrowserInfo();

            var authorizations = Request.Headers.Authorization;
            string accessToken = string.Empty;
            if (authorizations.Count > 0)
            {
                accessToken = (authorizations[0] ?? string.Empty).Replace("Bearer ", "");
            }

            var token = LoginMem.GetRefreshToken(parameters.Refresh_Token);

            if (token == null)
            {
                return BadRequest(new BaseInfo
                {
                    Code = -1,
                    Message = "Refresh Token not found."
                });
            }

            if (token.IsExpired)
            {
                // Remove refresh token if expired
                LoginMem.RemoveRefreshToken(token.RefreshToken);

                return BadRequest(new BaseInfo
                {
                    Code = -1,
                    Message = "Refresh Token has expired."
                });
            }

            if (!string.Equals(accessToken, token.AccessToken))
            {
                return BadRequest(new BaseInfo
                {
                    Code = -1,
                    Message = "Refresh Token and Access Token do not match."
                });
            }

            //
            var user = JWTHelper.GetUserInfo(accessToken);
            if (user == null)  //|| user.AutoId <= 0
            {
                return BadRequest(new BaseInfo
                {
                    Code = -1,
                    Message = "User not logged on."
                });
            }
            //
            LoginMem.RemoveRefreshToken(token.RefreshToken);

            var now = DateTime.UtcNow;
            var refresh_token = new IdentityRefreshToken
            {
                Identity = user.User_Id.ToString(),
                RefreshToken = Guid.NewGuid().ToString("N"),
                IssueTimeUtc = DateTime.UtcNow,
                ExpiryTimeUtc = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1),
            };

            var returnValue = await GetJwtAsync(refresh_token.RefreshToken, user);
            refresh_token.AccessToken = returnValue.Item2;

            LoginMem.SetRefreshToken(refresh_token);

            return Ok(returnValue.Item1);
        }

        private IActionResult DoInvalidateToken(TokenRequestParams parameters)
        {
            var token = LoginMem.GetRefreshToken(parameters.Refresh_Token);

            if (token == null)
            {
                return Ok(new BaseInfo() { Code = -1, Message = "Token in valid." });
            }

            if (token.IsExpired)
            {
                LoginMem.RemoveRefreshToken(token.RefreshToken);
                return Ok(new BaseInfo() { Code = -1, Message = "Token has expired." });
            }

            return Ok(new BaseInfo() { Code = 1, Message = "Token valid." });
        }


        private async Task<Tuple<string, string>> GetJwtAsync(string Refresh_Token, UserInfo user)
        {
            var now = DateTime.UtcNow;

            //Lấy quyền từ bảng trung gian
            List<UserFunction> functionSettings = user.FunctionSettings;

            LoginMem.SetFunctions((long)user.User_Id, functionSettings);
            var claims = new Claim[]
            {
                new Claim(CustomClaimTypes.User_Id, user.User_Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.User_Name, ClaimValueTypes.String),
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString(), ClaimValueTypes.String),
                new Claim("reference_id", user.Reference_Id.ToString()),
                new Claim("com_id", user.Com_Id.ToString()),
                new Claim("Lst_Com_Id", user.Lst_Com_Id.ToString()),
                new Claim("reference_type", user.User_Type.ToString()),
            };

            var expires = now.AddHours(Config_Info.TimeOutLogin);
            string _jwt_key = Config_Info.Jwt_Key;
            string _jwt_issuer = Config_Info.Jwt_Issuer;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwt_issuer,
                _jwt_issuer,
                claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials);


            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new
            {
                User_Id = user.User_Id,
                Message = "_",
                SsonData = "",
                Access_Token = accessToken,
                ExpiryTime = expires,
                Refresh_Token,
                User_Name = user.User_Name,
                Full_Name = user.Full_Name,
                User_Type = user.User_Type,
                User_Type_Text = user.User_Type_Text,
                Link_Server_file = CommonData.Address_Server_Save_file + CommonData.FileAttach + "/",
            };

            return new Tuple<string, string>(Newtonsoft.Json.JsonConvert.SerializeObject(response), accessToken);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public IActionResult Logout(string refresh_token)
        {
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    LoginMem.RemoveRefreshToken(refresh_token);
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, ex.Message);
            }
            return Ok(new BaseInfo() { Code = 1, Message = "_" });
        }
    }
}
