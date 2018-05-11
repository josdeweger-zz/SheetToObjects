using SheetToObjects.Lib.Attributes.MappingType;
using SheetToObjects.Lib.Attributes.Rules;
using SheetToObjects.Lib.Configuration;

namespace SheetToObjects.ConsoleApp.Models
{

    public class ProfileModel
    {
        [IsRequired]
        [Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", true)]
        public string Email { get; set; }

        [IsRequired]
        public Gender Gender { get; set; }
        
        [MappingByColumnName("firstname")]
        [IsRequired]
        public string FirstName { get; set; }
        

        public string MiddleName { get; set; }
        [IsRequired]
        public string LastName { get; set; }
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

    public enum RegistrationSource
    {
        Import,
        Other

    }

    public enum YesNo
    {
        Yes,
        No
    }
}