namespace SheetToObjects.ConsoleApp.Models
{
    public class ProfileModel
    {
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int RelationNumber { get; set; }
        public string LanguageCode { get; set; }
        public Label Label { get; set; }
        public bool Terms { get; set; }
        public ProfileType ProfileType { get; set; }
        public bool IsVerified { get; set; }
        public RegistrationSource RegistrationSource { get; set; }
    }

    public enum Gender
    {
        M,
        F
    }

    public enum Label
    {
        LGP,
        LSL,
        HSN,
        CAM,
        LBL
    }

    public enum ProfileType
    {
        Guest,
        HomeOwner,
        TourOperator
    }

    public enum RegistrationSource
    {
        Import
    }
}