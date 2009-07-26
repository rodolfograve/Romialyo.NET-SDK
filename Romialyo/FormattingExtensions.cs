using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    public static class FormattingExtensions
    {

        public static string PrettyFormat(this object obj)
        {
            Type t = obj.GetType();
            StringBuilder resultBuilder = new StringBuilder(128);
            resultBuilder.Append(t.Name + "{");
            foreach (var prop in t.GetProperties())
            {
                object value = prop.GetValue(obj, null);
                string valueStr = value == null ? "<NULL>" : value.ToString();
                resultBuilder.Append(prop.Name + "='" + valueStr + "',");
            }
            foreach (var prop in t.GetFields())
            {
                object value = prop.GetValue(obj);
                string valueStr = value == null ? "<NULL>" : value.ToString();
                resultBuilder.Append(prop.Name + "='" + valueStr + "',");
            }
            resultBuilder.Append("}");
            return resultBuilder.ToString();
        }

    }
}
