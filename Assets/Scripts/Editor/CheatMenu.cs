using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    // Author: David Pagotto

    public class CheatMenuInjector
    {
        private static bool initialized = false;
        private static CheatMenu cheatMenu;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoaded()
        {
            if (initialized)
                return;
            initialized = true;
            cheatMenu = new CheatMenu();

        }
    }

    public class CheatMenu
    {
        private bool menuEnabled = true;
        private bool unlimitedFuel = false;
        private bool temperatureHack = false;
        private bool flyMode = false;
        private LevelManager levelManager;

        private Vehicle vehicle;
        private Box[] boxes;
        private Transform[] boxPrevParents;
        private GameObject[] vehicleChilds;


        public CheatMenu()
        {
            if (!Finder.Main)
                return;
            Finder.Main.MainControllerOnGUI += OnSceneGUI;
            levelManager = Finder.LevelManager;
            Finder.LevelLoadedEventChannel.OnLevelLoaded += OnLevelLoaded;
        }


        private void OnLevelLoaded()
        {
            boxPrevParents = null;
            vehicle = Finder.Vehicle;
            boxes = GameObject.FindObjectsOfType<Box>();
            vehicleChilds = vehicle.Children();
        }

        private void AttachBoxes()
        {
            boxPrevParents = new Transform[boxes.Length];

            for (int i = 0; i < boxes.Length; i++)
            {
                var box = boxes[i];
                if (box.IsBoxOut)
                    continue;
                var bt = box.transform;
                boxPrevParents[i] = bt.parent;
                box.transform.parent = vehicle.VehicleBody.transform;
                box.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                box.GetComponent<Collider2D>().isTrigger = true;

            }
        }

        private void DetachBoxes()
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                var box = boxes[i];
                var bt = box.transform;
                box.transform.parent = boxPrevParents[i];
                box.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                box.GetComponent<Collider2D>().isTrigger = false;
            }
            boxPrevParents = null;
        }

        private void ProcessFlyMode(KeyCode keyCode)
        {
            Vector3 newOffset = new Vector3();
            switch (keyCode)
            {
                case KeyCode.Keypad8:
                    newOffset.y += 2;
                    break;
                case KeyCode.Keypad2:
                    newOffset.y -= 2;
                    break;
                case KeyCode.Keypad4:
                    newOffset.x -= 2;
                    break;
                case KeyCode.Keypad6:
                    newOffset.x += 2;
                    break;
            }

            foreach (var child in vehicleChilds)
                if(!child.GetComponent<Box>())
                    child.transform.position += newOffset;
        }

        private void TeleportBoxes()
        {
            var truckBed = GameObject.FindObjectOfType<TruckBed>();
            foreach (var box in boxes)
            {
                box.gameObject.SetActive(true);
                box.GetComponent<SpriteRenderer>().color = Color.white;
                box.transform.position = truckBed.transform.position;
            }
        }

        public void OnSceneGUI()
        {
            var e = Event.current;
            if (e.type == EventType.KeyDown)
                ProcessFlyMode(e.keyCode);
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Insert)
                menuEnabled = !menuEnabled;
            if (!menuEnabled)
                return;
            if (!levelManager.CurrentLevel.isLoaded)
                return;

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(0, 100, 200, 300));
            GUI.color = new Color(0, 0, 0, 0.9f);
            GUI.Box(new Rect(0, 0, 300, 200), new GUIContent());
            GUI.color = Color.white;
            if (boxPrevParents == null)
            {
                if (GUILayout.Button("Attach boxes"))
                    AttachBoxes();
            } else
            {
                if (GUILayout.Button("Detach boxes"))
                    DetachBoxes();
            }

            if (GUILayout.Button("Teleport to endpoint"))
            {
                var endpoint = GameObject.FindObjectOfType<EndPointTrigger>();
                AttachBoxes();
                foreach (var child in vehicle.Children())
                    child.transform.position = endpoint.transform.position;
                DetachBoxes();
            }

            if (GUILayout.Button("Teleport boxes to truckbed"))
                TeleportBoxes();

            if (GUILayout.Button("Give boost"))
            {
                Finder.BonusInventory.AddBonus(new NitroBonusData());
                var viewer = GameObject.FindObjectOfType<BonusInventoryViewer>();
                var method = typeof(BonusInventoryViewer).GetMethod("OnBonusInventoryUpdate", BindingFlags.NonPublic | BindingFlags.Instance);
                if(method == null)
                {
                    Debug.LogError("Couldn't find method OnBonusInventoryUpdate");
                } else
                {
                    method.Invoke(viewer, new object[] { null });
                }
            }

            unlimitedFuel = GUILayout.Toggle(unlimitedFuel, "Unlimited fuel");
            temperatureHack = GUILayout.Toggle(temperatureHack, "Temperature hack");

            var prevFlyMode = flyMode;
            flyMode = GUILayout.Toggle(flyMode, "Fly Mode(Numpad)");
            GUILayout.Label("Press [Insert] to hide");
            GUILayout.EndArea();

            if(prevFlyMode != flyMode)
            {
                if (flyMode)
                {
                    foreach (var child in vehicleChilds)
                            child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                } else
                {
                    foreach (var child in vehicleChilds)
                            child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                }
            }
            Handles.EndGUI();

            if (unlimitedFuel)
                vehicle.CurrentFuel = vehicle.MaxFuel;
            if (temperatureHack)
                vehicle.Temperature = Finder.LevelSpecs.InitialTemperature;
        }
    }
}
