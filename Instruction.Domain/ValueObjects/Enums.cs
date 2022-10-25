using System.ComponentModel;

namespace Instruction.Domain.ValueObjects
{
    public enum LoggingType
    {
        Request,
        Response
    }

    public enum NotificationType
    {
        [Description("Sms Servisi")]
        Sms = 1,
        [Description("Mail Servisi")]
        EMail = 2,
        [Description("Bildirim Servisi")]
        Notification = 3
    }
    public enum OrderStatusType
    {
        Created=1,
        Complated=2,
        Canceled=3
    }
}
