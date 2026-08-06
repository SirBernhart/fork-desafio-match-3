using System.Collections;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class MatchBonusView : MonoBehaviour
    {
        [SerializeField] private float _vfxDuration = 1;

        private void Awake()
        {
            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(_vfxDuration);
            Destroy(gameObject);
        }
    }
}
