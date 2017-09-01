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

        private readonly Dictionary<Tuple<byte, byte>, RelationshipType> _relationshipDictionary;

        public RelationshipMap()
        {
            _relationshipDictionary = new Dictionary<Tuple<byte, byte>, RelationshipType>();
        }

        public RelationshipType GetRelationshipType(byte factionIdA, byte factionIdB)
        {
            var tuple = Tuple.Create(factionIdA, factionIdB);

            return _relationshipDictionary.ContainsKey(tuple)
                ? _relationshipDictionary[tuple]
                : RelationshipType.Neutral;
        }

        public void SetRelationship(byte factionIdA, byte factionIdB, RelationshipType relationshipType)
        {
            var tuple = Tuple.Create(factionIdA, factionIdB);

            _relationshipDictionary[tuple] = relationshipType;
        }
    }
}