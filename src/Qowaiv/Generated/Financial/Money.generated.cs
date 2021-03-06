﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NotField
#define NotIsEmpty
#define NotIsUnknown
#define NotIsEmptyOrUnknown
#define NotEqualsSvo
#define NotGetHashCodeClass
namespace Qowaiv.Financial
{
    using System;

    public partial struct Money
    {
#if !NotField
        private Money(decimal value) => m_Value = value;
        /// <summary>The inner value of the money.</summary>
        private decimal m_Value;
#endif
#if !NotIsEmpty
        /// <summary>Returns true if the  money is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
        /// <summary>Returns true if the  money is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
        /// <summary>Returns true if the  money is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;

    public partial struct Money : IEquatable<Money>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Money other && Equals(other);
#if !NotEqualsSvo
        /// <summary>Returns true if this instance and the other money are equal, otherwise false.</summary>
        /// <param name = "other">The <see cref = "Money"/> to compare with.</param>
        public bool Equals(Money other) => m_Value == other.m_Value;
#if !NotGetHashCodeStruct
        /// <inheritdoc/>
        public override int GetHashCode() => m_Value.GetHashCode();
#endif
#if !NotGetHashCodeClass
        /// <inheritdoc/>
        public override int GetHashCode() => m_Value is null ? 0 : m_Value.GetHashCode();
#endif
#endif
        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator !=(Money left, Money right) => !(left == right);
        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand</param>
        public static bool operator ==(Money left, Money right) => left.Equals(right);
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Collections.Generic;

    public partial struct Money : IComparable, IComparable<Money>
    {
        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Money other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

#if !NotEqualsSvo
        /// <inheritdoc/>
        public int CompareTo(Money other) => Comparer<decimal>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Money l, Money r) => l.CompareTo(r) < 0;
        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator>(Money l, Money r) => l.CompareTo(r) > 0;
        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Money l, Money r) => l.CompareTo(r) <= 0;
        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Money l, Money r) => l.CompareTo(r) >= 0;
#endif
    }
}

namespace Qowaiv.Financial
{
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial struct Money : IXmlSerializable
    {
        /// <summary>Gets the <see href = "XmlSchema"/> to XML (de)serialize the money.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;
        /// <summary>Reads the money from an <see href = "XmlReader"/>.</summary>
        /// <param name = "reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var xml = reader.ReadElementString();
#if !NotCultureDependent
            var val = Parse(xml, CultureInfo.InvariantCulture);
#else
            var val = Parse(xml);
#endif
            m_Value = val.m_Value;
            OnReadXml(val);
        }

        partial void OnReadXml(Money other);
        /// <summary>Writes the money to an <see href = "XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses <see cref = "ToXmlString()"/>.
        /// </remarks>
        /// <param name = "writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToXmlString());
        }
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Globalization;
    using Qowaiv.Json;

    public partial struct Money
    {
        /// <summary>Creates the money from a JSON string.</summary>
        /// <param name = "json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized money.
        /// </returns>
        
#if !NotCultureDependent
        public static Money FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
        public static Money FromJson(string json) => Parse(json);
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Globalization;

    public partial struct Money : IFormattable
    {
        /// <summary>Returns a <see cref = "string "/> that represents the money.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the money.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        /// <summary>Returns a formatted <see cref = "string "/> that represents the money.</summary>
        /// <param name = "formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString(string.Empty, formatProvider);
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Globalization;

    public partial struct Money
    {
#if !NotCultureDependent
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Money"/>.</summary>
        /// <param name = "s">
        /// A string containing the money to convert.
        /// </param>
        /// <returns>
        /// The parsed money.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Money Parse(string s) => Parse(s, CultureInfo.CurrentCulture);
        /// <summary>Converts the <see cref = "string "/> to <see cref = "Money"/>.</summary>
        /// <param name = "s">
        /// A string containing the money to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed money.
        /// </returns>
        /// <exception cref = "FormatException">
        /// <paramref name = "s"/> is not in the correct format.
        /// </exception>
        public static Money Parse(string s, IFormatProvider formatProvider)
        {
            return TryParse(s, formatProvider, out Money val) ? val : throw new FormatException(QowaivMessages.FormatExceptionMoney);
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Money"/>.</summary>
        /// <param name = "s">
        /// A string containing the money to convert.
        /// </param>
        /// <returns>
        /// The money if the string was converted successfully, otherwise default.
        /// </returns>
        public static Money TryParse(string s)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out Money val) ? val : default;
        }

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Money"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the money to convert.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Money result) => TryParse(s, CultureInfo.CurrentCulture, out result);
#else
        /// <summary>Converts the <see cref="string"/> to <see cref="Money"/>.</summary>
        /// <param name="s">
        /// A string containing the money to convert.
        /// </param>
        /// <returns>
        /// The parsed money.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is not in the correct format.
        /// </exception>
        public static Money Parse(string s)
        {
            return TryParse(s, out Money val)
                ? val
                : throw new FormatException(QowaivMessages.FormatExceptionMoney);
        }

        /// <summary>Converts the <see cref="string"/> to <see cref="Money"/>.</summary>
        /// <param name="s">
        /// A string containing the money to convert.
        /// </param>
        /// <returns>
        /// The money if the string was converted successfully, otherwise default.
        /// </returns>
        public static Money TryParse(string s)
        {
            return TryParse(s, out Money val) ? val : default;
        }
#endif
    }
}

namespace Qowaiv.Financial
{
    using System;
    using System.Globalization;

    public partial struct Money
    {
#if !NotCultureDependent
        /// <summary>Returns true if the value represents a valid money.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);
        /// <summary>Returns true if the value represents a valid money.</summary>
        /// <param name = "val">
        /// The <see cref = "string "/> to validate.
        /// </param>
        /// <param name = "formatProvider">
        /// The <see cref = "IFormatProvider"/> to interpret the <see cref = "string "/> value with.
        /// </param>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return !string.IsNullOrWhiteSpace(val) && TryParse(val, formatProvider, out _);
        }
#else
        /// <summary>Returns true if the value represents a valid money.</summary>
        /// <param name="val">
        /// The <see cref="string"/> to validate.
        /// </param>
        public static bool IsValid(string val)
        {
            return !string.IsNullOrWhiteSpace(val) && TryParse(val, out _);
        }
#endif
    }
}