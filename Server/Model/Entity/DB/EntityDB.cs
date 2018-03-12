using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    [BsonKnownTypes(typeof(AccountInfo))]
    public class EntityDB : Component
    {
        public EntityDB()
        {
        }

        public EntityDB(long id)
        {
            Id = id;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}
