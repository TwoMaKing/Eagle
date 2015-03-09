using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Domain
{
    /// <summary>
    /// 值对象的抽象基类, 值对象是只包含值类型成员属性的类的实例.
    /// </summary>
    public abstract class ValueObjectBase<TValueObject> : IEquatable<TValueObject> where TValueObject : ValueObjectBase<TValueObject>
    {
        public override bool Equals(object obj)
        {
            // If both are null, or both are same instance, return true
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            var item = obj as ValueObjectBase<TValueObject>;

            if (item == null)
            {
                return false;
            }

            return this.Equals((TValueObject)item);
        }

        public virtual bool Equals(TValueObject other)
        {
            if (other == null)
                return false;

            Type t = this.GetType();

            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {
                object otherValue = property.GetValue(other, null);
                object thisValue = property.GetValue(this, null);

                //if the value is null...
                if (otherValue == null)
                {
                    if (thisValue != null)
                        return false;
                }
                //if the value is a datetime-related type...
                else if ((typeof(DateTime).IsAssignableFrom(property.PropertyType)) ||
                         ((typeof(DateTime?).IsAssignableFrom(property.PropertyType))))
                {
                    string dateString1 = ((DateTime)otherValue).ToLongDateString();
                    string dateString2 = ((DateTime)thisValue).ToLongDateString();
                    if (!dateString1.Equals(dateString2))
                    {
                        return false;
                    }
                    continue;
                }
                //if the value is any collection...
                else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    IEnumerable otherEnumerable = (IEnumerable)otherValue;
                    IEnumerable thisEnumerable = (IEnumerable)thisValue;

                    if (!otherEnumerable.Cast<object>().SequenceEqual(thisEnumerable.Cast<object>()))
                        return false;
                }
                //if we get this far, just compare the two values...
                else if (!otherValue.Equals(thisValue))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 31;
            var changeMultiplier = false;
            const int index = 1;

            // Compare all public properties
            var publicProperties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (publicProperties != null && publicProperties.Any())
            {
                IEnumerable<object> values = publicProperties.Select(item => item.GetValue(this, null));

                foreach (var value in values)
                {
                    if (value != null)
                    {
                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114)
                                   + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                    {
                        hashCode = hashCode ^ (index * 13); // Support order
                    }
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ValueObjectBase<TValueObject> left, ValueObjectBase<TValueObject> right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ValueObjectBase<TValueObject> left, ValueObjectBase<TValueObject> right)
        {
            return !(left == right);
        }

    }


}
