using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

namespace SheetToObjects.Specs.TestModels
{
    [SheetToObjectAttributeConfig()]
    public class AutoMapTestModel
    {
        [IsRequired]
        [Regex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", true)]
        public string Emailaddress { get; set; }

        [IsRequired]
        public Gender Gender { get; set; }

        [IsRequired]
        public string Firstname { get; set; }
        public string Middlename { get; set; }

        [IsRequired]
        public string Lastname { get; set; }

        [IsRequired]
        [Regex(@"^[A-Z0-9]{9}$", true)]
        public string RelationNumber { get; set; }

        [IsRequired]
        public string Language { get; set; }

        [IsRequired]
        public Label Label { get; set; }

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
        Guest = 0,
        BronzeProfile = 1,
        SilverProfile = 2,
        GoldProfile = 3
    }

    public enum YesNo
    {
        Yes,
        No
    }

    public enum Label
    {
        Label1,
        Label2,
        Label3,
        Label4,
        Label5,
        Label6,
        Label7
    }
}