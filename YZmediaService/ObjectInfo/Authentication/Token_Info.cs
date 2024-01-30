namespace ObjectInfo
{
    public sealed class TokenRequestParams
    {
        public string Grant_type { get; set; } = string.Empty;
        public string Refresh_Token { get; set; } = string.Empty;
        public string User_Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class BaseInfo
    {
        public decimal Code { get; set; }
        public string Message { get; set; }
    }

    public class IdentityRefreshToken
    {
        public string Identity { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime IssueTimeUtc { get; set; }
        public DateTime ExpiryTimeUtc { get; set; }
        public bool IsExpired { get { return DateTime.UtcNow >= ExpiryTimeUtc; } }
    }

    public class CustomClaimTypes
    {
        public const string ClientId = "client_id";
        public const string UserPid = "user_pid";
        public const string User_Id = "User_Id";
        public const string Privileges = "privileges";
        public const string IsAdministrator = "is_administrator";
        public const string UnitId = "unit_id";
        public const string TaiKhoanID = "taikhoan_id";
        public const string UnitCode = "unit_code";
        public const string AnhDaiDien = "anh_dai_dien";


        public const string expires = "exp";
    }
}
