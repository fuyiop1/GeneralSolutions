using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using WebSite.MyDbContext.Enums;

namespace WebSite.Helpers
{
    public static class EnumHelper
    {

        public static Dictionary<int, string> LanguageDictionary { get; private set; }

        static EnumHelper()
        {
            LanguageDictionary = EnumHelper.ConvertEnumToDictionary(typeof(Language));
        }

        public static Dictionary<int, string> ConvertEnumToDictionary(Type enumType)
        {
            var result = new Dictionary<int, string>();
            if (enumType.IsEnum)
            {
                var enumKeys = Enum.GetNames(enumType);

                foreach (var enumKeyItem in enumKeys)
                {
                    var fieldInfo = enumType.GetField(enumKeyItem);
                    if (fieldInfo != null)
                    {
                        var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        foreach (var attributeItem in attributes)
                        {
                            var descriptionAttribue = attributeItem as DescriptionAttribute;
                            if (descriptionAttribue != null)
                            {
                                result.Add(Convert.ToInt32(Enum.Parse(enumType, enumKeyItem)), descriptionAttribue.Description);
                                break;
                            }
                        }

                    }
                }

            }
            return result;
        }

        public static string GetEnumDescription(int val, Type enumType)
        {
            var result = string.Empty;
            var dic = ConvertEnumToDictionary(enumType);
            if (dic.Keys.Contains(val))
            {
                result = dic[val];
            }
            return result;
        }

    }
}