using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RA.Utils
{
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration,TValue>
        where TValue:IComparable
    {
        readonly string _displayName;
        readonly TValue _value;
        private static Lazy<TEnumeration[]> _enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        protected Enumeration(TValue value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }
        public TValue Value
        {
            get { return _value; }
        }
        public string DisplayName
        {
            get { return _displayName; }
        }
        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other.Value);
        }

        public override sealed string ToString()
        {
            return DisplayName;
        }
        public static TEnumeration[] GetAll()
        {
            return _enumerations.Value;
        }

        private static TEnumeration[] GetEnumerations()
        {
            Type enumerationType = typeof(TEnumeration);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }
        public bool Equals(TEnumeration other)
        {
            return other != null && Value.Equals(other.Value);
        }
    }
}
