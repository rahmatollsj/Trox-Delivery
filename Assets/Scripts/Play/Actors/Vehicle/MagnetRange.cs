using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace Game
{
    // Benoit Simon-Turgeon
    public class MagnetRange : MonoBehaviour
    {
        [SerializeField] private float magnetForce = 10f;
        
        private List<Box> boxList;
        private TruckBed truckBed;
        
        public bool IsMagnetActive { get; set; } = false;

        private void Awake()
        {
            truckBed = Finder.Vehicle.VehicleBody.transform.Find(GameObjects.TruckBed).GetComponent<TruckBed>();
            boxList = new List<Box>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.Box)) return;
            
            var box = other.GetComponent<Box>();
            if (!boxList.Contains(box))
                boxList.Add(box);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.Box)) return;
            
            var box = other.GetComponent<Box>();
            if (boxList.Contains(box))
                boxList.Remove(box);
        }
        
        private void FixedUpdate()
        {
            if (!IsMagnetActive) return;

            foreach (var box in boxList)
            {
                if (!box.IsBoxOut) continue;
                
                var boxRigidBody = box.GetComponent<Rigidbody2D>();
                boxRigidBody.AddForce((truckBed.transform.position - boxRigidBody.transform.position) * magnetForce);
            }
        }
    }
}