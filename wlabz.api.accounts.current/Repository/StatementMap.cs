using FluentNHibernate.Mapping;

namespace WLabz.api.accounts.current.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Statement
    /// </summary>
    public class StatementMap : ClassMap<Model.Statement>
    {
        public StatementMap()
        {

            //// Database Table Name...
            Table("Statements");

            CompositeId()
                .KeyProperty(x => x.AccountNumber)
                .KeyProperty(x => x.StatementDate);

            Map(x => x.ClosingBalance)
                .Not.Nullable();

            Map(x => x.TotalDebits)
                .Not.Nullable();

            Map(x => x.TotalCredits)
                .Not.Nullable();

        }
    }
}