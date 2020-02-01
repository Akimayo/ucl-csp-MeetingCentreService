using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MeetingCentreService.Models
{
    public class NameValidationRule : ValidationRule
    {
        private static readonly Regex NameFormat = new Regex(@".{2,100}");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string && NameFormat.IsMatch(value as string))
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "Name is incorrect");
        }
    }
    public class CodeValidationRule : ValidationRule
    {
        private static readonly Regex CodeFormat = new Regex(@"[a-zA-Z0-9\.\-:_]{5,50}");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string && CodeFormat.IsMatch(value as string))
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "Code is incorrect");
        }
    }
    public class DescriptionaValidationRule : ValidationRule
    {
        private static readonly Regex DescriptionFormat = new Regex(@".{10,300}");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string && DescriptionFormat.IsMatch(value as string))
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "Description is incorrect");
        }
    }
    public class CapacityValidationRule : ValidationRule
    {
        private const int Min = 1, Max = 100;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is int && (int)value >= Min && (int)value <= Max)
                return ValidationResult.ValidResult;
            else if (value is string && int.TryParse((string)value, out int v) && v >= Min && v <= Max)
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "Capacity exceeds boundaries");
        }
    }
    public class CustomerValidationRule : ValidationRule
    {
        private static readonly Regex CustomerFormat = new Regex(@".{2,100}");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string && CustomerFormat.IsMatch(value as string))
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "Customer is invalid");
        }
    }
    public class NoteValidationRule : ValidationRule
    {
        private static readonly Regex NoteFormat = new Regex(@".{0,300}");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string && NoteFormat.IsMatch(value as string))
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "Note is incorrect");
        }
    }
    public class PersonsCountValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is int && (int)value > 0)
                return ValidationResult.ValidResult;
            else if (value is string && int.TryParse(value as string, out int v) && v > 0)
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "PersonsCount is invalid");
        }
    }
    public class MinimumRecommendedStockValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is int && (int)value > 0 && (int)value <= 1000)
                return ValidationResult.ValidResult;
            else if (value is string && int.TryParse(value as string, out int v) && v > 0 && v <= 1000)
                return ValidationResult.ValidResult;
            else return new ValidationResult(false, "MinimumRecommendedStock is out of bounds");
        }
    }
}
