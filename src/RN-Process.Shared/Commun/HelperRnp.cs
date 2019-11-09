using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace RN_Process.Shared.Commun
{
   public static class HelperRnp
    {
        //public static string ToDescription<TEnum>(this TEnum EnumValue) where TEnum : struct
        //{
        //    return Enumerations.GetEnumDescription((Enum)(object)((TEnum)EnumValue));
        //}

        public static string DescriptionAttr<TEnum>(this TEnum source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }
    }
}
