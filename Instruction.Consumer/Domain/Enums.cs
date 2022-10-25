using System.ComponentModel;

namespace Instruction.Consumer.Domain;

public class Enums
{
    public enum NotificationType
    {
        [Description("Sms Servisi")]
        Sms =1,
        [Description("Mail Servisi")]
        EMail =2,
        [Description("Bildirim Servisi")]
        Notification =3
    }
}
