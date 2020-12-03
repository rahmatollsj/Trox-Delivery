using UnityEngine;

namespace Game
{
    //Author : François-Xavier Bernier
    public class ComplexVolcanicRock : MonoBehaviour
    {
        private VolcanicRock volcanicRock;

        private void Awake()
        {
            volcanicRock = GetComponentInChildren<VolcanicRock>();
        }
    }
}