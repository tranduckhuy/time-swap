namespace TimeSwap.Shared.Constants
{
    /// <summary>
    /// This class contains all the status codes for the application 1xxx
    /// </summary>
    #region SuccessCodes
    public enum SuccessCode
    {
        Register = 1000,
        Login = 1001,
        Update = 1002
    }
    #endregion

    /// <summary>
    /// This class contains all the error codes for the application 2xxx
    /// </summary>
    #region ErrorCodes
    public enum ErrorCode
    {
        RegisterFailed = 2000,
        LoginFailed = 2001,
        ValidationFailed = 2002
    }
    #endregion
}
