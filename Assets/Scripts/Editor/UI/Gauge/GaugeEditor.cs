using Harmony;
using UnityEditor;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    // https://www.youtube.com/watch?v=bPO7_JNWNmI
    // https://www.youtube.com/watch?v=n_RHttAaRCk
    // https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5fc
    [CustomEditor(typeof(Gauge))]
    public class GaugeEditor : Editor
    {
        private readonly Color polygonColor = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.2f);
        private const int numPreviewPolygonPoints = 20;
        private Vector3[] previewPolygonPoints = new Vector3[numPreviewPolygonPoints];
        private Gauge gauge;

        private Vector2 GetAngleToDirection(float angle) => new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));


        private void UpdatePreviewPolygon()
        {
            float beginAngle = gauge.NeedleLeftAnglePoint.AngleTo(gauge.NeedlePivotPosition);
            float endAngle = gauge.NeedleRightAnglePoint.AngleTo(gauge.NeedlePivotPosition);
            float offsetBetweenPoints = (endAngle - beginAngle) / (numPreviewPolygonPoints - 2);
            float needleLength = gauge.NeedleLeftAnglePoint.DistanceTo(gauge.NeedlePivotPosition);

            float currentOffset = 0f;
            for (int i = 0; i < numPreviewPolygonPoints; i++, currentOffset += offsetBetweenPoints)
            {
                var offsetFromCenter = GetAngleToDirection(beginAngle + currentOffset) * needleLength;
                previewPolygonPoints[i] = gauge.NeedlePivotPosition + offsetFromCenter;
            }
            previewPolygonPoints[numPreviewPolygonPoints - 1] = gauge.NeedlePivotPosition;
        }

        private Vector2? CalculateNeedleAnglePointPosition(Vector2 currentPosition, Vector2 newPosition, bool isAngleLeft, bool isShiftHeld)
        {
            if (isShiftHeld)
            {
                // À gauche et à droite, la longeur d'aiguille reste la même.
                float diff = newPosition.x - currentPosition.x;
                if (!isAngleLeft)
                    diff *= -1;

                Vector2 diffVector = new Vector2(diff, 0);

                gauge.NeedleLeftAnglePointOffset += diffVector;
                gauge.NeedleRightAnglePointOffset -= diffVector;
            }
            else
            {
                // On veut que le point soit toujours à la même distance du point central.
                float newAngle = newPosition.AngleTo(gauge.NeedlePivotPosition);
                Vector2 dir = GetAngleToDirection(newAngle);
                float needleLength = gauge.NeedleLeftAnglePoint.DistanceTo(gauge.NeedlePivotPosition);
                return dir * needleLength;
            }
            return null;
        }

        private void ResetGauge()
        {
            if (!gauge.IsInitializedByEditor && gauge.needleImage != null)
            {
                Undo.RecordObject(gauge, "Initialize Gauge");
                var needleRect = gauge.needleImage.rectTransform.rect;
                gauge.NeedleLeftAnglePointOffset = new Vector2(-needleRect.height, 0);
                gauge.NeedleRightAnglePointOffset = new Vector2(needleRect.height, 0);
                gauge.IsInitializedByEditor = true;
                UpdatePreviewPolygon();
            }
        }

        private void Awake()
        {
            gauge = (Gauge)target;
            ResetGauge();
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            gauge = (Gauge)target;
            if (GUILayout.Button("Reset points"))
            {
                gauge.IsInitializedByEditor = false;
                ResetGauge();
                SceneView.RepaintAll();
            }

            EditorGUILayout.HelpBox("Hold shift to change distance between the needle's pivot point and left & right angle points.", MessageType.Info);
        }

        private void OnSceneGUI()
        {
            Event guiEvent = Event.current;
            // Permet de détecter si la fenêtre de l'éditeur a changer de taille.
            if (guiEvent.type == EventType.Layout) 
                UpdatePreviewPolygon();

            
            Handles.color = Color.blue;

            // Point qui permet de choisir l'angle où l'aiguille commence à gauche
            EditorGUI.BeginChangeCheck();
            Vector2 newLeftAnglePoint = Handles.FreeMoveHandle(gauge.NeedleLeftAnglePoint, Quaternion.identity, 1f, Vector2.zero, Handles.CylinderHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(gauge, "Move needle angle left");
                Vector2? result = CalculateNeedleAnglePointPosition(gauge.NeedleLeftAnglePoint, newLeftAnglePoint, true, guiEvent.shift);
                if (result.HasValue)
                    gauge.NeedleLeftAnglePointOffset = result.Value;

                UpdatePreviewPolygon();
                gauge.UpdateNeedleAvailableRange();
            }

            // Point qui permet de choisir l'angle où l'aiguille commence à droite
            EditorGUI.BeginChangeCheck();
            Vector2 newRightAnglePoint = Handles.FreeMoveHandle(gauge.NeedleRightAnglePoint, Quaternion.identity, 1f, Vector2.zero, Handles.CylinderHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(gauge, "Move needle angle right");
                Vector2? result = CalculateNeedleAnglePointPosition(gauge.NeedleRightAnglePoint, newRightAnglePoint, false, guiEvent.shift);
                if (result.HasValue)
                    gauge.NeedleRightAnglePointOffset = result.Value;

                UpdatePreviewPolygon();
                gauge.UpdateNeedleAvailableRange();
            }

            Handles.color = polygonColor;
            Handles.DrawAAConvexPolygon(previewPolygonPoints);

        }
    }
}