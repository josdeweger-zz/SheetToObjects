using System;
using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

namespace SheetToObjects.ConsoleApp.Models
{

    [SheetToObjectConfig(true)]
    public class Profile
    {
        [MappingByHeader("emailaddress")]
        [IsRequired]
        [Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", true)]
        public string Email { get; set; }

        [IsRequired]
        public Gender Gender { get; set; }
        
        
        [IsRequired]
        public string FirstName { get; set; }
        

        public string MiddleName { get; set; }

        [IsRequired]
        public string LastName { get; set; }

        [IsRequired]
        [Format("d-M-yyyy")]
        public DateTime DateOfBirth { get; set; }

        [ShouldHaveUniqueValue]
        [IsRequired]
        public string RelationNumber { get; set; }
        
        [MappingByIndex(6)]
        [IsRequired]
        public string LanguageCode { get; set; }

        [IsRequired]
        public YesNo Terms { get; set; }

        [IsRequired]
        public ProfileType ProfileType { get; set; }
    }

    public enum Gender
    {
        M,
        F
    }

    public enum ProfileType
    {
        Profile = 2
    }

    public enum YesNo
    {
        Yes,
        No
    }
}