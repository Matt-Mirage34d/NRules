using NRules.Fluent.Dsl;
using NRules.Samples.ClaimsExpert.Domain;

namespace NRules.Samples.ClaimsExpert.Rules.ValidationRules
{
    [Name("Patient sex validation")]
    public class PatientSexValidationRule : Rule
    {
        public override void Define()
        {
            Claim claim = null;

            When()
                .Claim(() => claim)
                .Patient(p => p == claim.Patient,
                    p => p.Sex == Sex.Unspecified);

            Then()
                .Do(ctx => ctx.Error(claim, "Patient sex not specified"));
        }
    }
}