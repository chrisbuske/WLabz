using WLabz.api.accounts.current.Model;
using FluentNHibernate.Mapping;
namespace WLabz.api.accounts.current.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Statement
    /// </summary>
    public class BalanceMap : ClassMap<Balance>
    {
        public BalanceMap()
        {

            //// Database Table Name...
            Table("Statements");

            CompositeId()
                .KeyProperty(x => x.AccountNumber)
                .KeyProperty(x => x.StatementDate);

            Map(x => x.ClosingBalance)
                .Not.Nullable();

        }
    }
}