using WLabz.api.clientelle.Model;
using FluentNHibernate.Mapping;

namespace WLabz.api.clientelle.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Individual
    /// </summary>
    public class IndividualMap : ClassMap<Individual>
    {
        public IndividualMap()
        {

            // Database Table Name...
            Table("Individuals");

            Id(x => x.EntityID)
                .Unique(); ;

            Map(x => x.IdentityNumber)
                .Not.Nullable()
                .Length(16);

            Map(x => x.CountryOfIssue)
                .Not.Nullable()
                .Length(24);

            Map(x => x.FirstName)
                .Not.Nullable()
                .Length(32);

            Map(x => x.MiddleNames)
                .Not.Nullable()
                .Length(64); 

            Map(x => x.Surname)
                .Not.Nullable()
                .Length(64);

            Map(x => x.DOB)
                .Not.Nullable();

            Map(x => x.Gender)
                .Length(8)
                .Not.Nullable();

        }
    }
}
