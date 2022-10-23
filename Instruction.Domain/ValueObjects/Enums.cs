namespace Instruction.Domain.ValueObjects
{
    public enum LoggingType
    {
        Request,
        Response
    }

    public enum NotificationType
    {
        Sms,
        EMail,
        Notification
    }
    public enum OrderStatusType
    {
        Created=1,
        Complated=2,
        Canceled=3
    }
}
