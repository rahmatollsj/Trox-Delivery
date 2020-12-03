using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Game
{
    // Author: David Pagotto
    public class GroundUpdater : MonoBehaviour { }

    [CustomEditor(typeof(GroundUpdater))]
    public class GroundUpdaterEditor : Editor
    {
        public override void OnInspectorGUI() 
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Scale SpriteShape points"))
            {
                GroundUpdater updater = (GroundUpdater)target;
                var controller = updater.GetComponent<SpriteShapeController>();
                var spline = controller.spline;
                Undo.RecordObject(controller, "Update spline");

                for (int i = 0; i < controller.spline.GetPointCount(); i++)
                {
                    var pos = controller.spline.GetPosition(i);
                    pos.Scale(updater.transform.localScale);
                    controller.spline.SetPosition(i, pos);
                }
                Undo.RecordObject(controller, "Update scale");
                updater.transform.localScale = new Vector3(1f, 1f, 1f);
                controller.RefreshSpriteShape();
                controller.BakeCollider();
            }
            EditorGUILayout.HelpBox("If executed in runtime, colldier update may take a few seconds. Beware of objects falling through the map.", MessageType.Warning);
        }
    }
}
