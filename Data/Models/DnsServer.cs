using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace dug.Data.Models
{
    public class DnsServer
    {
        public Guid DnsServerId {get; set;}

        [Required]
        public IPAddress IPAddress {get; set;}

        public string CountryCode {get; set;}

        public string City {get; set;}

        public bool? DNSSEC {get; set;} //TODO: Will no value in the server info here actually set this to null?
        
        public double Reliability {get; set;}

        public ContinentCodes ContinentCode { get { return DataMaps.CountryContinentMap[CountryCode]; } }

        public string CountryName { 
            get {
                return DataMaps.CountryNameMap.ContainsKey(CountryCode) ? DataMaps.CountryNameMap[CountryCode] : "";
            }
        }

        public string CountryFlag { get { return DataMaps.CountryFlagMap[CountryCode]; } }

        public string CityCountryName {
            get {
                string result = string.IsNullOrWhiteSpace(City) ? "" : $"{City}, ";
                result += string.IsNullOrEmpty(CountryName) ? "UNKNOWN COUNTRY 🤷" : CountryName;
                return result;
            }
        }

        public string CityCountryContinentName {
            get {
                string result = string.IsNullOrWhiteSpace(City) ? "" : $"{City}, ";
                result += string.IsNullOrEmpty(CountryName) ? "UNKNOWN COUNTRY 🤷" : $"{CountryName}, ";
                result += ContinentCode == null ? "" : ContinentCode.Name;
                return result;
            }
        }

        public string ToCsvString(){
            return $"{IPAddress.ToString()},{CountryCode},{City},{DNSSEC},{Reliability}";
        }
    }

    public class DnsServerComparer : IEqualityComparer<DnsServer>
    {

        public bool Equals(DnsServer x, DnsServer y)
        {
            return x.IPAddress == y.IPAddress;
        }

        public int GetHashCode([DisallowNull] DnsServer obj)
        {
            return obj.ToCsvString().GetHashCode();
        }
    }
}