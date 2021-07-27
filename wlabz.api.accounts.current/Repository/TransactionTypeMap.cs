using WLabz.api.accounts.current.Model;
using FluentNHibernate.Mapping;

namespace WLabz.api.accounts.current.Repository
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