using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "MatchBonusModel", menuName = "Scriptable Objects/MatchBonusModel")]
    public class MatchBonusModel : ScriptableObject
    {
        [SerializeField] private MatchBonusType _type;
        [SerializeField] private MatchBonusView _matchBonusViewPrefab;
        
        public MatchBonusType BonusType => _type;
        public GameObject MatchBonusViewPrefab => _matchBonusViewPrefab.gameObject;
    }
}
