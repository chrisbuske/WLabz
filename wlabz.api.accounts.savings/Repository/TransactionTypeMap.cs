using wlabz.api.accounts.savings.Model;
using FluentNHibernate.Mapping;

namespace wlabz.api.accounts.savings.Repository
{
    public class TransactionTypeMap : ClassMap<TransactionType>
    {
        public TransactionTypeMap()
        {

            //// Database Table Name...
            Table("TransactionTypes");

            Id(x => x.TransactionTypeCode)
                .Unique();

            Map(x => x.DRCRIndicator)
                .Length(4)
                .Not.Nullable()
                .CustomType<GenericEnumMapper<TransactionType.DRCRIndicators>>();

            Map(x => x.Description)
                .Length(128)
                .Not.Nullable();

        }
    }
}