using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[BsonKnownTypes(typeof(AConfigComponent))]
	[BsonKnownTypes(typeof(UnitGateComponent))]
	[BsonKnownTypes(typeof(NumericComponent))]
	[BsonKnownTypes(typeof(Entity))]
	[BsonKnownTypes(typeof(EntityDB))]
	public partial class Component
	{
	}
}