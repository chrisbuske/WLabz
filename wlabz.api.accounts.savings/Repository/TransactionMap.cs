using wlabz.api.accounts.savings.Model;
using FluentNHibernate.Mapping;

namespace wlabz.api.accounts.savings.Repository
{
    public class TransactionMap : ClassMap<Transaction>
    {

        public TransactionMap()
        {
            //// Database Table Name...
            Table("Transactions");

            CompositeId()
                .KeyProperty(x => x.AccountNumber)
                .KeyProperty(x => x.TransactionTime);             
                
            Map(x => x.Description)
                    .Not.Nullable();

            Map(x => x.TransactionType)
                .Not.Nullable();

            Map(x => x.Amount)
                .Not.Nullable();

        }
    }
}
