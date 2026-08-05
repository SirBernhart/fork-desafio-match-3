using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "MatchBonusModel", menuName = "Scriptable Objects/MatchBonusModel")]
    public class MatchBonusModel : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private GameObject _vfxPrefab;
    }
}
