using System;
using System.Collections.Generic;

namespace World.SocialModule
{
    public class RelationshipMap
    {
        public enum RelationshipType
        {
            Neutral,
            Hostile,
            Friendly
        }

        private readonly Dictionary<Tuple<byte, byte>, RelationshipType> _mapping;

        public RelationshipMap()
        {
            _mapping = new Dictionary<Tuple<byte, byte>, RelationshipType>();
        }

        public RelationshipType GetRelationshipType(byte factionIdA, byte factionIdB)
        {
            var tuple = Tuple.Create(factionIdA, factionIdB);

            return _mapping.ContainsKey(tuple)
                ? _mapping[tuple]
                : RelationshipType.Neutral;
        }

        public void SetRelationship(byte factionIdA, byte factionIdB, RelationshipType relationshipType)
        {
            var tuple = Tuple.Create(factionIdA, factionIdB);

            _mapping[tuple] = relationshipType;
        }

        public IEnumerable<byte> GetFactionsWithRelationship(byte factionId, RelationshipType relationshipType)
        {
            return new byte[0];
        }
    }
}