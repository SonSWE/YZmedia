namespace CommonLib
{
    public class Logger
    {
        public static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}