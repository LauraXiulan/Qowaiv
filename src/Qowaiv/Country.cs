﻿using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
	/// <summary>Represents a Country.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a Country.")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(String))]
	[TypeConverter(typeof(CountryTypeConverter))]
	public partial struct Country : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<Country>
	{
		/// <summary>Represents an empty/not set Country.</summary>
		public static readonly Country Empty = new Country() { m_Value = default(String) };

		/// <summary>Represents an unknown (but set) Country.</summary>
		public static readonly Country Unknown = new Country() { m_Value = "ZZ" };

		#region Properties

		/// <summary>The inner value of the Country.</summary>
		private String m_Value;

		/// <summary>Gets the name of the country.</summary>
		/// <remarks>
		/// For formely countries this returns the ISO 3166-3 code.
		/// For unkown a '?' is returned.
		/// For existing countries this returns the ISO 3166-1 code.
		/// </remarks>
		public string Name { get { return IsUnknown() ? "?" : m_Value ?? String.Empty; } }

		/// <summary>Gets the displayname.</summary>
		public string DisplayName { get { return GetDisplayName(CultureInfo.CurrentCulture); } }

		/// <summary>Gets the full name of the country/region in English.</summary>
		/// <returns>
		/// The full name of the country in English.
		/// </returns>
		public string EnglishName { get { return GetDisplayName(CultureInfo.InvariantCulture); } }

		///<summary>Gets the two-letter code defined in ISO 3166-1 for the country.</summary>
		/// <returns>
		/// The two-letter code defined in ISO 3166-1 for the country.
		/// </returns>
		public string IsoAlpha2Code { get { return GetResourceString("ISO2", CultureInfo.InvariantCulture); } }

		///<summary>Gets the three-letter code defined in ISO 3166-1 for the country.</summary>
		/// <returns>
		/// The three-letter code defined in ISO 3166-1 for the country.
		/// </returns>
		public string IsoAlpha3Code { get { return GetResourceString("ISO3", CultureInfo.InvariantCulture); } }

		///<summary>Gets the numeric code defined in ISO 3166-1 for the country/region.</summary>
		/// <returns>
		/// The numeric code defined in ISO 3166-1 for the country/region.
		/// </returns>
		public int IsoNumericCode { get { return m_Value == default(String) ? 0 : XmlConvert.ToInt32(GetResourceString("ISO", CultureInfo.InvariantCulture)); } }

		/// <summary>Gets the country calling code as defined by ITU-T.</summary>
		/// <remarks>
		/// Recommendations E.123 and E.164, also called IDD (International Direct Dialling) or ISD (International Subscriber Dialling) codes.
		/// </remarks>
		public string CallingCode { get { return GetResourceString("CallingCode", CultureInfo.InvariantCulture); } }

		///<summary>Gets true if the RegionInfo equvalent of this country exists, otherwise false.</summary>
		public bool RegionInfoExists { get { return !String.IsNullOrEmpty(GetResourceString("RegionInfoExists", CultureInfo.InvariantCulture)); } }

		/// <summary>Gets the start date from witch the country exists.</summary>
		public Date StartDate { get { return m_Value == default(String) ? Date.MinValue : (Date)XmlConvert.ToDateTime(GetResourceString("StartDate", CultureInfo.InvariantCulture), "yyyy-MM-dd"); } }

		/// <summary>If the country does not exist anymore, the end date is given, otherwise null.</summary>
		public Date? EndDate
		{
			get
			{
				var val = GetResourceString("EndDate", CultureInfo.InvariantCulture);
				return string.IsNullOrEmpty(val) ? null : (Date?)XmlConvert.ToDateTime(val, "yyyy-MM-dd");
			}
		}

		#endregion

		#region Methods

		/// <summary>Returns true if the Country is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(String); }

		/// <summary>Returns true if the Country is unknown, otherwise false.</summary>
		public bool IsUnknown() { return m_Value == Country.Unknown.m_Value; }

		/// <summary>Returns true if the Country is empty or unknown, otherwise false.</summary>
		public bool IsEmptyOrUnknown() { return IsEmpty() || IsUnknown(); }


		/// <summary>Gets the displayname for a specified culture.</summary>
		/// <param name="culture">
		/// The culture of the display name.
		/// </param>
		/// <returns></returns>
		public string GetDisplayName(CultureInfo culture) { return GetResourceString("DisplayName", culture); }

		/// <summary>Returns true if the country exists at the given date, otherwise false.</summary>
		/// <param name="measurement">
		/// The date of existence.
		/// </param>
		public bool ExistsOnDate(Date measurement)
		{
			return this.StartDate <= measurement && (!this.EndDate.HasValue || this.EndDate.Value >= measurement);
		}

		/// <summary>Converts the CountryInfo to a RegionInfo.</summary>
		public RegionInfo ToRegionInfo()
		{
			if (this.RegionInfoExists)
			{
				return new RegionInfo(this.IsoAlpha2Code);
			}
			throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
				QowaivMessages.NotSupportedExceptionCountryToRegionInfo, this.EnglishName, this.IsoAlpha2Code));
		}

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of Country based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Country(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			m_Value = info.GetString("Value");
		}

		/// <summary>Adds the underlying propererty of Country to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a Country.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the Country from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of Country.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the Country to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of Country.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(m_Value);
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a Country from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson() { m_Value = default(String); }

		/// <summary>Generates a Country from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the Country.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a Country from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the Country.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger)
		{
			m_Value = Parse(jsonInteger.ToString("000")).m_Value;
		}

		/// <summary>Generates a Country from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the Country.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates a Country from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the Country.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts a Country into its JSON object representation.</summary>
		object IJsonSerializable.ToJson() { return m_Value; }

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current Country for debug purposes.</summary>
		private string DebuggerDisplay
		{
			get
			{
				if (m_Value == default(String)) { return "Country: (empty)"; }
				if (m_Value == Unknown.m_Value) { return "Country: (unknown)"; }

				return string.Format(
				  CultureInfo.InvariantCulture,
				  "Country: {0} ({1}/{2})",
				  this.EnglishName,
				  this.IsoAlpha2Code,
				  this.IsoAlpha3Code
			  );
			}
		}

		/// <summary>Returns a System.String that represents the current Country.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Country.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Country.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current Country.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <remarks>
		/// The formats:
		/// 
		/// n: as Name.
		/// 0: as ISO Numeric.
		/// 2: as ISO Alpha-2.
		/// 3: as ISO Alpha-3.
		/// e: as English name.
		/// f: as formatted/display name.
		/// </remarks>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}

			// If no format specified, use the default format.
			if (String.IsNullOrEmpty(format)) { return this.Name; }

			// Apply the format.
			return StringFormatter.Apply(this, format, formatProvider, FormatTokens);
		}

		/// <summary>The format token instructions.</summary>
		private static readonly Dictionary<char, Func<Country, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<Country, IFormatProvider, string>>()
		{
			{ 'n', (svo, provider) => svo.Name },
			{ '2', (svo, provider) => svo.GetResourceString("ISO2", provider) },
			{ '3', (svo, provider) => svo.GetResourceString("ISO3", provider) },
			{ '0', (svo, provider) => svo.GetResourceString("ISO", provider) },
			{ 'e', (svo, provider) => svo.EnglishName },
			{ 'f', (svo, provider) => svo.GetResourceString("DisplayName", provider) },
		};

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compair with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this Country.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value == null ? 0 : m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Country left, Country right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Country left, Country right)
		{
			return !(left == right);
		}

		#endregion

		#region IComparable

		/// <summary>Compares this instance with a specified System.Object and indicates whether
		/// this instance precedes, follows, or appears in the same position in the sort
		/// order as the specified System.Object.
		/// </summary>
		/// <param name="obj">
		/// An object that evaluates to a Country.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a Country.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Country)
			{
				return CompareTo((Country)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a Country"), "obj");
		}

		/// <summary>Compares this instance with a specified Country and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified Country.
		/// </summary>
		/// <param name="other">
		/// The Country to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Country other) { return (m_Value ?? String.Empty).CompareTo((other.m_Value ?? String.Empty)); }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a Country to a System.String.</summary>
		public static explicit operator string(Country val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a Country.</summary>
		public static explicit operator Country(string str) { return Country.Parse(str, CultureInfo.CurrentCulture); }

		/// <summary>Casts a System.Globalization.RegionInfo to a Country.</summary>
		public static implicit operator Country(RegionInfo region) { return Country.Create(region); }

		/// <summary>Casts a Country to a System.Globalization.RegionInf.</summary>
		public static explicit operator RegionInfo(Country val) { return val.ToRegionInfo(); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a Country.</summary>
		/// <param name="s">
		/// A string containing a Country to convert.
		/// </param>
		/// <returns>
		/// A Country.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Country Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a Country.</summary>
		/// <param name="s">
		/// A string containing a Country to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <returns>
		/// A Country.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Country Parse(string s, IFormatProvider formatProvider)
		{
			Country val;
			if (Country.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionCountry);
		}

		/// <summary>Converts the string to a Country.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Country to convert.
		/// </param>
		/// <returns>
		/// The Country if the string was converted successfully, otherwise Country.Empty.
		/// </returns>
		public static Country TryParse(string s)
		{
			Country val;
			if (Country.TryParse(s, out val))
			{
				return val;
			}
			return Country.Empty;
		}

		/// <summary>Converts the string to a Country.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Country to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Country result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a Country.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Country to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, IFormatProvider formatProvider, out Country result)
		{
			result = Country.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			if (Qowaiv.Unknown.IsUnknown(s))
			{
				result = Country.Unknown;
				return true;
			}
			var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;

			AddCulture(culture);

			var str = Parsing.ToUnified(s);
			string val;

			if (Parsings[culture].TryGetValue(str, out val) || Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out val))
			{
				result = new Country() { m_Value = val };
				return true;
			}
			return false;
		}

		/// <summary>Creates a country based on a region info.</summary>
		/// <param name="region">
		/// The corresponding region info.
		/// </param>
		/// <returns>
		/// Returns a country that repesents the same region as region info.
		/// </returns>
		public static Country Create(RegionInfo region)
		{
			if (region == null) { return default(Country); }
			// In .NET, Serbia and Montenegro (CS) is still active.
			if (region.TwoLetterISORegionName == "CS") { return Country.CSXX; }

			return All.FirstOrDefault(c => c.Name == region.TwoLetterISORegionName);
		}


		/// <summary>Creates a country based on a culture info.</summary>
		/// <param name="culture">
		/// A culture info.
		/// </param>
		/// <returns>
		/// Returns a country that repesents the country specified at the culture if
		/// any, otherwise Country.Empthy.
		/// </returns>
		public static Country Create(CultureInfo culture)
		{
			if (culture == null || culture == CultureInfo.InvariantCulture || culture.IsNeutralCulture)
			{
				return default(Country);
			}

			var name = culture.Name.Substring(culture.Name.IndexOf('-') + 1);

			return All.FirstOrDefault(c => c.Name == name);
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid Country, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid Country, otherwise false.</summary>
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			if (string.IsNullOrWhiteSpace(val) || Qowaiv.Unknown.IsUnknown(val)) { return false; }

			var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
			AddCulture(culture);

			var str = Parsing.ToUnified(val);
			return Parsings[culture].ContainsKey(str) || Parsings[CultureInfo.InvariantCulture].ContainsKey(str);
		}

		#endregion

		#region Get countries

		/// <summary>Gets all existing countries.</summary>
		/// <returns>
		/// A list of existing countries.
		/// </returns>
		public static IEnumerable<Country> GetCurrent()
		{
			return GetOnDate(Date.Today);
		}

		/// <summary>Gets all countries existing on the specified measurement date.</summary>
		/// <param name="measurement">
		/// The measurement date.
		/// </param>
		/// <returns>
		/// A list of existing countries.
		/// </returns>
		public static IEnumerable<Country> GetOnDate(Date measurement)
		{
			return All.Where(country => country.ExistsOnDate(measurement));
		}

		/// <summary>Gets a collection of all country info's.</summary>
		public static readonly ReadOnlyCollection<Country> All = new ReadOnlyCollection<Country>(
			ResourceManager
				.GetString("All")
				.Split(';')
				.Select(str => new Country() { m_Value = str })
				.ToList());

		#endregion

		#region Resources

		internal static ResourceManager ResourceManager
		{
			get
			{
				if (s_ResourceManager == null)
				{
					ResourceManager temp = new ResourceManager("Qowaiv.CountryLabels", typeof(Country).Assembly);
					s_ResourceManager = temp;
				}
				return s_ResourceManager;
			}
		}
		internal static ResourceManager s_ResourceManager;

		/// <summary>Get resource string.</summary>
		/// <param name="postfix">
		/// The prefix of the resource key.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		internal string GetResourceString(string postfix, IFormatProvider formatProvider)
		{
			if (m_Value == default(String)) { return string.Empty; }
			return ResourceManager.GetString(m_Value + '_' + postfix, formatProvider as CultureInfo ?? CultureInfo.CurrentCulture) ?? String.Empty;
		}

		#endregion

		#region Lookup

		/// <summary>Initalizes the country lookup.</summary>
		[SuppressMessage("Microsoft.Usage", "CA2207:InitializeValueTypeStaticFieldsInline", Justification = "Complex intialization, this approach is better understandable.")]
		static Country()
		{
			foreach (var country in Country.All)
			{
				Parsings[CultureInfo.InvariantCulture][country.IsoAlpha2Code.ToLowerInvariant()] = country.m_Value;
				Parsings[CultureInfo.InvariantCulture][country.IsoAlpha3Code.ToLowerInvariant()] = country.m_Value;
				Parsings[CultureInfo.InvariantCulture][country.IsoNumericCode.ToString("000")] = country.m_Value;
				Parsings[CultureInfo.InvariantCulture][Parsing.ToUnified(country.GetDisplayName(CultureInfo.InvariantCulture))] = country.m_Value;
			}
		}

		/// <summary>Adds a culture to the parsings.</summary>
		/// <param name="culture">
		/// The culture to add.
		/// </param>
		private static void AddCulture(CultureInfo culture)
		{
			lock (locker)
			{
				if (Parsings.ContainsKey(culture)) { return; }

				Parsings[culture] = new Dictionary<string, string>();

				Parsings[culture][Country.Unknown.GetDisplayName(culture)] = Country.Unknown.m_Value;

				foreach (var country in Country.All)
				{
					Parsings[culture][Parsing.ToUnified(country.GetDisplayName(culture))] = country.m_Value;
				}
			}
		}

		/// <summary>Represents the parsing keys.</summary>
		private static readonly Dictionary<CultureInfo, Dictionary<string, string>> Parsings = new Dictionary<CultureInfo, Dictionary<string, string>>()
		{
			{
				CultureInfo.InvariantCulture, new Dictionary<string, string>()
				{
					{ "ZZ", "ZZ" },
					{ "ZZZ", "ZZ" },
					{ "999", "ZZ" },
					{ "unknown", "ZZ" },
				}
			},
		};

		/// <summary>The locker for adding a culture.</summary>
		private static volatile object locker = new object();

		#endregion
	}
}