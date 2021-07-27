using FluentNHibernate.Mapping;
using WLabz.api.clientelle.Model;

namespace WLabz.api.clientelle.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Organisation
    /// </summary>
    public class OrganisationalMap : ClassMap<Organisation>
    {
        public OrganisationalMap()
        {

            // Database Table Name...
            Table("Organisations");

            Id(x => x.EntityID)
                .Unique();

            Map(x => x.RegistrationNamber)
                .Not.Nullable()
                .Length(32);                

            Map(x => x.CountryOfIssue)
                .Not.Nullable()
                .Length(24);

            Map(x => x.OrganisationName)
                .Not.Nullable()
                .Length(64);

        }
    }
}