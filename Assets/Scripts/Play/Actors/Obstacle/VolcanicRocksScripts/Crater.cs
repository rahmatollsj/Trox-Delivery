using UnityEngine;

namespace Game
{
    //Author : François-Xavier Bernier
    public class Crater : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherGameObject = other.gameObject;
            var volcanoRock = otherGameObject.GetComponentInParent<VolcanicRock>();
            if (volcanoRock != null)
            {
                volcanoRock.Explode();
                gameObject.SetActive(false);
            }
        }
    }
}