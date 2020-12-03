using UnityEngine;

namespace Game
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Calcule l'angle entre deux vecteurs
        /// </summary>
        /// <param name="_this">Vector2 qui appelle l'extension</param>
        /// <param name="targetPosition">La position dont ont souhaite calculer l'angle entre.</param>
        /// <returns>L'angle (en radians)</returns>
        public static float AngleTo(this Vector2 _this, Vector2 targetPosition)
        {
            var direction = _this - targetPosition;
            return Mathf.Atan2(direction.y, direction.x);
        }
    }
}
