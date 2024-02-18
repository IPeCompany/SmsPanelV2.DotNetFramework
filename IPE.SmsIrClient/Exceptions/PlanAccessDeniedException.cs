namespace IPE.SmsIrClient.Exceptions
{
    public class PlanAccessDeniedException : SmsIrException
    {
        internal PlanAccessDeniedException(byte status, string message)
            : base(status, message)
        {
        }
    }
}