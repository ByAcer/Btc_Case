namespace Instruction.Domain.ValueObjects
{
    public enum LoggingType
    {
        Request,
        Response
    }

    public enum NotificationType
    {
        Sms=1,
        EMail=2,
        Notification=3
    }
    public enum OrderStatusType
    {
        Created=1,
        Complated=2,
        Canceled=3
    }
}
