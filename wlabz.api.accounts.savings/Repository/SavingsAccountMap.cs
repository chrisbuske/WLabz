using wlabz.api.accounts.savings.Model;
using FluentNHibernate.Mapping;

namespace wlabz.api.accounts.savings.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Account
    /// </summary>
    public class SavingsAccountMap : ClassMap<SavingsAccount>
    {
        public SavingsAccountMap()
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
                .CustomType<GenericEnumMapper<SavingsAccount.AccountTypes>>();

        }
    }
}
