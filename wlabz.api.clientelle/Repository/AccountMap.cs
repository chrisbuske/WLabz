using WLabz.api.clientelle.Model;
using FluentNHibernate.Mapping;

namespace wlabz.api.clientelle.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Account
    /// </summary>
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            
            //// Database Table Name...
            Table("Accounts");

            Id(x => x.AccountNumber)
            .Unique()
            .GeneratedBy.Assigned();

            Map(x => x.EntityID)
                 .Not.Nullable();

            Map(x => x.AccountType)
                .Length(16)
                .Not.Nullable()
                .CustomType<GenericEnumMapper<Account.AccountTypes>>();

            HasMany(x => x.Statements)
                .Cascade.None()
                .Fetch.Select().Not.LazyLoad();


        }
    }
}
