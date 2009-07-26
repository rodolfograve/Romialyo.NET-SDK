using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;

namespace Romialyo.Data
{
    public static class MappingExtensions
    {
        public static void MapFromDataRecord<T>(this T target, IDataRecord dataRecord)
        {
            MapFromDataRecordWithCustomMap(target, dataRecord, propName => propName);
        }

        public static void MapFromDataRecordWithCustomMap<T>(this T target, IDataRecord dataRecord, Func<string, string> Mapper)
        {
            Type t = target.GetType();
            object value = null;
            foreach (var property in t.GetProperties().Where(x => x.CanWrite))
            {
                try
                {
                    string columnName = Mapper(property.Name);
                    value = GetValueFromDataRecord(dataRecord, columnName);
                    if (value != null)
                    {
                        value = ExtendedConvert.ChangeType(value, property.PropertyType);
                    }
                    property.SetValue(target, value, null);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error assigning value {" + (value == null ? "<NULL>" : value.ToString()) + "} to property {" + property.Name + "}", ex);
                }
            }
        }

        private static object GetValueFromDataRecord(IDataRecord dataRecord, string columnName)
        {
            int columnOrdinal = dataRecord.GetOrdinal(columnName);
            object result = null;
            if (!dataRecord.IsDBNull(columnOrdinal))
            {
                result = dataRecord.GetValue(columnOrdinal);
            }
            return result;
        }

    }
}
