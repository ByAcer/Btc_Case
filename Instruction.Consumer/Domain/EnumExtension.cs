using System.ComponentModel;
using System.Reflection;

namespace Instruction.Consumer.Domain
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentException("Tip bulunamadı.", nameof(value));
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])
                fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                description = attributes[0].Description;
            }

            return description;
        }
    }
}
