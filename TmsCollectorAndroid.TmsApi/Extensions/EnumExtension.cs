using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace TmsCollectorAndroid.TmsApi.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<TEnum>(this TEnum e) where TEnum : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }

        public static TEnum GetEnumByDescription<TEnum>(this string enumDescription) where TEnum : Enum
        {
            var resultEnum = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().FirstOrDefault(en => en.GetDescription() == enumDescription);

            return ((resultEnum.GetDescription() == enumDescription) ? resultEnum : (TEnum)(object)-1);
        }
    }
}