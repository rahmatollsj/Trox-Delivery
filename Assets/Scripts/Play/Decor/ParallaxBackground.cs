#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private Vector2 relativeSpeed = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 offset = new Vector2(0f, 0f);

        private SpriteRenderer spriteRenderer;
        private float spriteWidth;
        private float spriteHeight;
        private float cameraWidth;
        private float cameraHeight;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            
            Sprite sprite = spriteRenderer.sprite;
            spriteWidth = sprite.texture.width / sprite.pixelsPerUnit;
            spriteHeight = sprite.texture.height / sprite.pixelsPerUnit;
            
            cameraHeight = 2 * camera.orthographicSize;
            cameraWidth = cameraHeight * camera.aspect;
            
            spriteRenderer.size = new Vector2(cameraWidth * 3 / relativeSpeed.x, spriteHeight);
        }

        private void FixedUpdate()
        {
            FixedUpdatePosition();
        }

#if UNITY_EDITOR
        // Author: David Pagotto
        private void OnValidate()
        {
            if (EditorApplication.isPlaying)
                return;
            // Unity appelle OnValidate() lors d'un Build du jeu.
            if (!gameObject.scene.isLoaded)
                return;
            // Permet de voir l'effet de l'offset directement dans l'éditeur de scène.
            Awake();
            Undo.RecordObject(this, "Update background preview");
            FixedUpdatePosition();
        }
#endif

        private void FixedUpdatePosition()
        {
            Vector2 cameraPosition = camera.transform.position + (Vector3)offset;
            Vector2 translation = new Vector2(
                cameraPosition.x * relativeSpeed.x % spriteWidth,
                cameraPosition.y * relativeSpeed.y
            );
            transform.position = cameraPosition - translation;
        }
    }
}