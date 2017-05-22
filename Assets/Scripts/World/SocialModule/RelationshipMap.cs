using System.Collections.Generic;

namespace Assets.Scripts.World.SocialModule
{

    public class RelationshipMap
    {
        #region private properties

        private Dictionary<int, int> _relationshipDictionary;

        #endregion

        public RelationshipMap(int faction)
        {
            _relationshipDictionary = new Dictionary<int, int>();
            SetRelationship(faction,20);
        }

        #region public properties

        public int Faction { get; protected set; }

        #endregion

        #region public methods

        public int GetRelation(int faction)
        {
            return _relationshipDictionary.ContainsKey(faction) ? 
                _relationshipDictionary[faction] : 0;
        }

        public void SetRelationship(int faction, int relationship)
        {
            _relationshipDictionary[faction] = relationship;
        }

        #endregion
    }
}
