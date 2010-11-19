// <copyright file="ValueSetter.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

using System;
using System.ComponentModel;
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Utility class that sets values on objects.
    /// </summary>
    public abstract class ValueSetter
    {
        #region Static helper methods

        /// <summary>
        /// Gets the value setter.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="ValueSetter" /> that can set the named value.</returns>
        public static ValueSetter GetValueSetter(object target, string name)
        {
            string normalizedName = NameComparer.NormalizeName(name);

            return PropertyDescriptorValueSetter.GetPropertyDescriptorValueSetter(target, normalizedName)
                ?? MethodInfoValueSetter.GetMethodInfoValueSetter(target, normalizedName)
                ?? PropertyInfoValueSetter.GetPropertyInfoValueSetter(target, normalizedName)
                ?? FieldInfoValueSetter.GetFieldInfoValueSetter(target, normalizedName)
                ?? (ValueSetter)new NullValueSetter();
        }

        /// <summary>
        /// Determines whether this instance [can set value] the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     <c>true</c> if this instance can set the value on the specified target; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanSetValue(object target, string name)
        {
            return GetValueSetter(target, name).CanSetValue();
        }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="name">The name.</param>
        /// <returns>The <c>Type</c> of the value that can be set.</returns>
        public static Type GetValueType(object target, string name)
        {
            return GetValueSetter(target, name).GetValueType();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(object target, string name, object value)
        {
            GetValueSetter(target, name).SetValue(value);
        }

        /// <summary>
        /// Sets the formatted value.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void SetFormattedValue(object target, string name, string value)
        {
            GetValueSetter(target, name).SetFormattedValue(value);
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Determines whether this instance can set the value.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance can set the value; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool CanSetValue();

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <returns>The <c>Type</c> of the value that can be set.</returns>
        public abstract Type GetValueType();

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract void SetValue(object value);

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the formatted value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetFormattedValue(string value)
        {
            SetValue(ValueParser.ParseValue(value, GetValueType()));
        }

        #endregion
    }

    internal class PropertyDescriptorValueSetter : ValueSetter
    {
        internal static PropertyDescriptorValueSetter GetPropertyDescriptorValueSetter(object target, string normalizedName)
        {
            if (target is ICustomTypeDescriptor)
            {
                ICustomTypeDescriptor typeDescriptor = (ICustomTypeDescriptor)target;
                PropertyDescriptorCollection properties = typeDescriptor.GetProperties();

                foreach (PropertyDescriptor property in properties)
                {
                    string normalizedPropertyName = NameComparer.NormalizeName(property.Name);

                    if (NameComparer.NormalizedNamesAreEqual(normalizedPropertyName, normalizedName) && !property.IsReadOnly)
                    {
                        return new PropertyDescriptorValueSetter(target, property);
                    }
                }
            }

            return null;
        }

        private object _target;
        private PropertyDescriptor _propertyDescriptor;

        public PropertyDescriptorValueSetter(object target, PropertyDescriptor propertyDescriptor)
        {
            _target = target;
            _propertyDescriptor = propertyDescriptor;
        }

        public override bool CanSetValue()
        {
            return true;
        }

        public override Type GetValueType()
        {
            return _propertyDescriptor.PropertyType;
        }

        public override void SetValue(object value)
        {
            _propertyDescriptor.SetValue(_target, value);
        }
    }

    internal class MethodInfoValueSetter : ValueSetter
    {
        internal static MethodInfoValueSetter GetMethodInfoValueSetter(object target, string normalizedName)
        {
            MemberInfo[] methods = target.GetType().GetMember(normalizedName, MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            foreach (MethodInfo method in methods)
            {
                if (MethodCanSetValue(method))
                {
                    return new MethodInfoValueSetter(target, method);
                }
            }

            return null;
        }

        private static bool MethodCanSetValue(MethodInfo method)
        {
            return method.ReturnType == typeof(void) &&
                   method.GetParameters().Length == 1;
        }

        private object _target;
        private MethodInfo _methodInfo;

        public MethodInfoValueSetter(object target, MethodInfo methodInfo)
        {
            _target = target;
            _methodInfo = methodInfo;
        }

        public override bool CanSetValue()
        {
            return true;
        }

        public override Type GetValueType()
        {
            return _methodInfo.GetParameters()[0].ParameterType;
        }

        public override void SetValue(object value)
        {
            _methodInfo.Invoke(_target, new object[] { value });
        }
    }

    internal class PropertyInfoValueSetter : ValueSetter
    {
        internal static PropertyInfoValueSetter GetPropertyInfoValueSetter(object target, string normalizedName)
        {
            PropertyInfo property = ValueGetter.GetPropertyInfo(target.GetType(), normalizedName);

            if (property != null && PropertyCanSetValue(property))
            {
                return new PropertyInfoValueSetter(target, property);
            }

            return null;
        }

        private static bool PropertyCanSetValue(PropertyInfo property)
        {
            return property.CanWrite;
        }

        private object _target;
        private PropertyInfo _propertyInfo;

        public PropertyInfoValueSetter(object target, PropertyInfo propertyInfo)
        {
            _target = target;
            _propertyInfo = propertyInfo;
        }

        public override bool CanSetValue()
        {
            return true;
        }

        public override Type GetValueType()
        {
            return _propertyInfo.PropertyType;
        }

        public override void SetValue(object value)
        {
            _propertyInfo.SetValue(_target, value, null);
        }
    }

    internal class FieldInfoValueSetter : ValueSetter
    {
        internal static FieldInfoValueSetter GetFieldInfoValueSetter(object target, string normalizedName)
        {
            FieldInfo field = target.GetType().GetField(normalizedName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (field != null)
            {
                if (FieldCanSetValue(field))
                {
                    return new FieldInfoValueSetter(target, field);
                }
            }

            return null;
        }

        private static bool FieldCanSetValue(FieldInfo field)
        {
            return !field.IsInitOnly;
        }

        private object _target;
        private FieldInfo _fieldInfo;

        public FieldInfoValueSetter(object target, FieldInfo fieldInfo)
        {
            _target = target;
            _fieldInfo = fieldInfo;
        }

        public override bool CanSetValue()
        {
            return true;
        }

        public override Type GetValueType()
        {
            return _fieldInfo.FieldType;
        }

        public override void SetValue(object value)
        {
            _fieldInfo.SetValue(_target, value);
        }
    }

    internal class NullValueSetter : ValueSetter
    {
        public override bool CanSetValue()
        {
            return false;
        }

        public override Type GetValueType()
        {
            throw new InvalidOperationException();
        }

        public override void SetValue(object value)
        {
            throw new InvalidOperationException();
        }
    }
}
