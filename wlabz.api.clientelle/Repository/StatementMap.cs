using WLabz.api.clientelle.Model;
using FluentNHibernate.Mapping;
namespace wlabz.api.clientelle.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Statement
    /// </summary>
    public class StatementMap : ClassMap<Statement>
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