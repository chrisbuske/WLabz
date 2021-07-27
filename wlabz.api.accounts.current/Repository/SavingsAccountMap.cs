using WLabz.api.accounts.current.Model;
using FluentNHibernate.Mapping;

namespace WLabz.api.accounts.current.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Account
    /// </summary>
    public class CurrentAccountMap : ClassMap<CurrentAccount>
    {
        public CurrentAccountMap()
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
                .CustomType<GenericEnumMapper<CurrentAccount.AccountTypes>>();

        }
    }
}
