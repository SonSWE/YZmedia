using ObjectInfo;

namespace YZmediaService
{
    public class LoginMem
    {
        private static readonly object c_lock = new();
        /// <summary>
        /// Key: UserPid; Value: List of Functions
        /// </summary>
        private static readonly Dictionary<long, List<UserFunction>> c_dicFunctionsByUser = new();
        /// <summary>
        /// Key: UserPid; Value: User
        /// </summary>
        private static readonly Dictionary<long, UserInfo> c_dicUser = new();
        /// <summary>
        /// Key: RefeshToken; Value: Info of RefeshToken
        /// </summary>
        private static readonly Dictionary<string, IdentityRefreshToken> c_dicRefreshToken = new();

        #region Hanle Refresh Token

        public static void SetRefreshToken(IdentityRefreshToken refreshToken)
        {
            lock (c_lock)
            {
                var key = refreshToken.RefreshToken;
                if (c_dicRefreshToken.ContainsKey(key))
                {
                    c_dicRefreshToken.Remove(key);
                }
                //
                c_dicRefreshToken.Add(key, refreshToken);
            }
        }

        public static void RemoveRefreshToken(string refreshToken)
        {
            lock (c_lock)
            {
                if (c_dicRefreshToken.ContainsKey(refreshToken))
                {
                    c_dicRefreshToken.Remove(refreshToken);
                }
            }
        }

        public static IdentityRefreshToken? GetRefreshToken(string refreshToken)
        {
            lock (c_lock)
            {
                if (c_dicRefreshToken.ContainsKey(refreshToken))
                {
                    return c_dicRefreshToken[refreshToken];
                }
            }

            return null;
        }

        #endregion

        #region Hanle User

        public static void SetUser(UserInfo user)
        {
            lock (c_lock)
            {
                var key = (long)user.User_Id;
                if (c_dicUser.ContainsKey(key))
                {
                    c_dicUser.Remove(key);
                }
                //
                c_dicUser.Add(key, user);
            }
        }

        public static void RemoveUser(long userPid)
        {
            lock (c_lock)
            {
                if (c_dicUser.ContainsKey(userPid))
                {
                    c_dicUser.Remove(userPid);
                }
            }
        }

        public static UserInfo? GetUser(long userPid)
        {
            lock (c_lock)
            {
                if (c_dicUser.ContainsKey(userPid))
                {
                    return c_dicUser[userPid];
                }
            }

            return null;
        }

        public static UserInfo? GetUserByUserId(string userId)
        {
            lock (c_lock)
            {
                if (c_dicUser.Count > 0)
                {
                    foreach (var item in c_dicUser.Values)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        if (string.Equals(item.User_Id, userId))
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Hanle Functions Of User

        public static void SetFunctions(long userPid, List<UserFunction> userFunctions)
        {
            lock (c_lock)
            {
                if (c_dicFunctionsByUser.ContainsKey(userPid))
                {
                    c_dicFunctionsByUser.Remove(userPid);
                }
                //
                c_dicFunctionsByUser.Add(userPid, userFunctions);
            }
        }

        public static void RemoveFunctions(long userPid)
        {
            lock (c_lock)
            {
                if (c_dicFunctionsByUser.ContainsKey(userPid))
                {
                    c_dicFunctionsByUser.Remove(userPid);
                }
            }
        }

        public static List<UserFunction>? GetFunctionsByUserPid(long userPid)
        {
            lock (c_lock)
            {
                if (c_dicFunctionsByUser.ContainsKey(userPid))
                {
                    return c_dicFunctionsByUser[userPid];
                }
            }
            return null;
        }

        #endregion
    }
}
