using WLabz.api.clientelle.Model;
using FluentNHibernate.Mapping;

namespace WLabz.api.clientelle.Repository
{
    /// <summary>
    /// Fluent mapping to WLabz.api.clientelle.Model.Entity
    /// </summary>
    public class EntityMap : ClassMap<Entity>
    {
        public EntityMap()
        {

            //// Database Table Name...
            Table("Entities");

            Id(x => x.EntityID)
                .Unique()
                .GeneratedBy.GuidComb();

            Map(x => x.EntityType)
                .Length(32)
                .Not.Nullable()
                .CustomType<GenericEnumMapper<Entity.EntityTypes>>();

            HasMany(x => x.Accounts).Fetch.Select().Not.LazyLoad();
            HasOne(x => x.Individual).Fetch.Select().Not.LazyLoad();
            HasOne(x => x.Organisation).Fetch.Select().Not.LazyLoad();

            //References(x => x.Individual)
            //    .LazyLoad(Laziness.False)
            //    .Nullable()
            //    .Cascade.Persist();

            //References(x => x.Organisation)
            //    .LazyLoad(Laziness.False)
            //    .Nullable()
            //    .Cascade.Persist();

        }
    }
}