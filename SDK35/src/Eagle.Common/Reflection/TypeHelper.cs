using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Eagle.Common.Reflection
{
    public static class TypeHelper
    {
        public static bool IsNullable(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("The type cannot be null.");
            }

            return type.IsGenericType && 
                   type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }

    /// <summary>
    /// Common Types
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Object 
        /// </summary>
        public static readonly Type Object = typeof(Object);

        /// <summary>
        /// Type 
        /// </summary>
        public static readonly Type Type = typeof(Type);

        /// <summary>
        /// Stirng 
        /// </summary>
        public static readonly Type String = typeof(String);

        /// <summary>
        /// Char
        /// </summary>
        public static readonly Type Char = typeof(Char);

        /// <summary>
        /// Boolean 
        /// </summary>
        public static readonly Type Boolean = typeof(Boolean);

        /// <summary>
        /// Byte 
        /// </summary>
        public static readonly Type Byte = typeof(Byte);


        /// <summary>
        /// Byte Array
        /// </summary>
        public static readonly Type ByteArray = typeof(Byte[]);

        /// <summary>
        /// SByte
        /// </summary>
        public static readonly Type SByte = typeof(SByte);

        /// <summary>
        /// Int16
        /// </summary>
        public static readonly Type Int16 = typeof(Int16);

        /// <summary>
        /// UInt16
        /// </summary>
        public static readonly Type UInt16 = typeof(UInt16);

        /// <summary>
        /// Int32
        /// </summary>
        public static readonly Type Int32 = typeof(Int32);

        /// <summary>
        /// UInt32
        /// </summary>
        public static readonly Type UInt32 = typeof(UInt32);

        /// <summary>
        /// Int64 
        /// </summary>
        public static readonly Type Int64 = typeof(Int64);

        /// <summary>
        /// UInt64 
        /// </summary>
        public static readonly Type UInt64 = typeof(UInt64);

        /// <summary>
        /// Double
        /// </summary>
        public static readonly Type Double = typeof(Double);

        /// <summary>
        /// Single 
        /// </summary>
        public static readonly Type Single = typeof(Single);

        /// <summary>
        /// Decimal 
        /// </summary>
        public static readonly Type Decimal = typeof(Decimal);

        /// <summary>
        /// Guid 
        /// </summary>
        public static readonly Type Guid = typeof(Guid);

        /// <summary>
        /// DateTime 
        /// </summary>
        public static readonly Type DateTime = typeof(DateTime);

        /// <summary>
        /// TimeSpan 
        /// </summary>
        public static readonly Type TimeSpan = typeof(TimeSpan);

        /// <summary>
        /// Nullable 
        /// </summary>
        public static readonly Type Nullable = typeof(Nullable<>);

        /// <summary>
        /// ValueType 
        /// </summary>
        public static readonly Type ValueType = typeof(ValueType);

        /// <summary>
        /// void 
        /// </summary>
        public static readonly Type Void = typeof(void);

        /// <summary>
        /// DBNull 
        /// </summary>
        public static readonly Type DBNull = typeof(DBNull);

        /// <summary>
        /// Delegate 
        /// </summary>
        public static readonly Type Delegate = typeof(Delegate);

        /// <summary>
        /// ByteEnumerable 
        /// </summary>
        public static readonly Type ByteEnumerable = typeof(IEnumerable<Byte>);

        /// <summary>
        /// IEnumerable 
        /// </summary>
        public static readonly Type IEnumerableofT = typeof(System.Collections.Generic.IEnumerable<>);

        /// <summary>
        /// IEnumerable 
        /// </summary>
        public static readonly Type IEnumerable = typeof(System.Collections.IEnumerable);

        /// <summary>
        /// IListSource 
        /// </summary>
        public static readonly Type IListSource = typeof(System.ComponentModel.IListSource);

        /// <summary>
        /// IDictionary 
        /// </summary>
        public static readonly Type IDictionary = typeof(System.Collections.IDictionary);

        /// <summary>
        /// IDictionary 
        /// </summary>
        public static readonly Type IDictionaryOfT = typeof(IDictionary<,>);
        /// <summary>
        /// Dictionary 
        /// </summary>
        public static readonly Type DictionaryOfT = typeof(Dictionary<,>);

        /// <summary>
        /// StringDictionary 
        /// </summary>
        public static readonly Type StringDictionary = typeof(StringDictionary);

        /// <summary>
        /// NameValueCollection 
        /// </summary>
        public static readonly Type NameValueCollection = typeof(NameValueCollection);

        /// <summary>
        /// IDataReader 
        /// </summary>
        public static readonly Type IDataReader = typeof(System.Data.IDataReader);

        /// <summary>
        /// DataTable 
        /// </summary>
        public static readonly Type DataTable = typeof(System.Data.DataTable);

        /// <summary>
        /// DataRow 
        /// </summary>
        public static readonly Type DataRow = typeof(System.Data.DataRow);

        /// <summary>
        /// IDictionary 
        /// </summary>
        public static readonly Type IDictionaryOfStringAndObject = typeof(IDictionary<string, object>);

        /// <summary>
        /// IDictionary 
        /// </summary>
        public static readonly Type IDictionaryOfStringAndString = typeof(IDictionary<string, string>);

    }

}
