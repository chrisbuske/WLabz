using WLabz.api.accounts.current.Model;
using FluentNHibernate.Mapping;

namespace WLabz.api.accounts.current.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Overdraft
    /// </summary>
    public class OverdraftMap : ClassMap<Overdraft>
    {
        public OverdraftMap()
        {
            //// Database Table Name...
            Table("Overdrafts");

            Id(x => x.AccountNumber)
            .Unique()
            .GeneratedBy.Assigned();

            Map(x => x.EntityID)
                 .Not.Nullable();

            Map(x => x.OverdraftAmount)
                    .Not.Nullable();

            Map(x => x.OverdraftStatus)
                .Length(16)
                .Not.Nullable()
                .CustomType<GenericEnumMapper<Overdraft.OverdraftStatuses>>();

        }
    }
}
