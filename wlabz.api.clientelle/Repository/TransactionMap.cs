using WLabz.api.clientelle.Model;
using FluentNHibernate.Mapping;

namespace WLabz.api.clientelle.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Transaction
    /// </summary>
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

            Map(x => x.Amount)
                .Not.Nullable();

            Map(x => x.TransactionType)
                .Not.Nullable();
        

        }
    }
}
