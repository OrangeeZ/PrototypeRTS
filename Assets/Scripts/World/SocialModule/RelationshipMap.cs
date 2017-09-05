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

        private readonly RelationshipType[,] _relationshipMapping;

        public RelationshipMap()
        {
            _relationshipMapping = new RelationshipType[3, 3];
        }

        public RelationshipType GetRelationshipType(byte factionIdA, byte factionIdB)
        {
            return _relationshipMapping[factionIdA, factionIdB];
        }

        public void SetRelationship(byte factionIdA, byte factionIdB, RelationshipType relationshipType)
        {
            _relationshipMapping[factionIdA, factionIdB] = relationshipType;
            _relationshipMapping[factionIdB, factionIdA] = relationshipType;
        }

        public IEnumerable<byte> GetFactionsWithRelationship(byte factionId, RelationshipType relationshipType)
        {
            for (var i = 0; i < 3; ++i)
            {
                if (_relationshipMapping[factionId, i] == relationshipType)
                {
                    yield return (byte) i;
                }
            }
        }
    }
}