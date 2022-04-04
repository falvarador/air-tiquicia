using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class RequiredEnumerableAttribute : ValidationAttribute
{
    public RequiredEnumerableAttribute()
        : base("The {0} field is required.")
    { }

    public override bool IsValid(object? value)
    {
        return Validate(value);
    }

    private bool Validate(object? value)
    {
        var enumValue = value as Nullable<PersonTypeEnum>;

        if (enumValue.GetValueOrDefault() == PersonTypeEnum.Empty)
            return false;

        return true;
    }
}
