using Harmony;
using UnityEngine;

namespace Game
{
    public class VehicleMover : MonoBehaviour
    {
        // Author: Benoit Simon-Turgeon
        [SerializeField] private float torque = 750f;
        [SerializeField] private float vehicleRotationSpeed = 750f;
        [Tooltip("Multiplier of the usual forward speed used for the brake force. Default is 2f")] 
        [SerializeField] private float brakeForce = 2f;

        // Author: David Pagotto
        [Tooltip("Vehicle will be 'grounded' if both wheels touch an object with any of these tags")] 
        [SerializeField] private LayerMask groundTags;
        [SerializeField] private LayerMask bridgeTags;
        //Author: Seyed-Rahmatoll Javadi
        [SerializeField] private LayerMask icedGroundTags;
        
        //Author: Seyed-Rahmatoll Javadi
        [SerializeField] private int maximumSpeed = 150;
          
        // Author: Benoit Simon-Turgeon
        [SerializeField] private int wheelsSpeedDivider = 10;
        [SerializeField] private float maxSpeedToTiltVehicleWhenAccelerating = 2f;
        [SerializeField] private float tiltDifferenceDivider = 2f;
        
        //Author: Seyed-Rahmatoll Javadi
        private const float MPerSecondToKMPerSecond = 3.6f;
        private const int DefaultFriction = 4;
        private const int IceFriction = 0;

        // Author: Benoit Simon-Turgeon
        private Rigidbody2D rbVehicleBody;
        private PolygonCollider2D vehicleCollider;
        private Rigidbody2D rbFrontWheel;
        private Rigidbody2D rbRearWheel;

        private BonusNitroChangedEventChannel bonusNitroChangedEventChannel;

        //Author: David Pagotto
        private Vector2 velocityBeforeFreeze;
        
        //Author: Benoit Simon-Turgeon
        private float speedMultiplier = 1f;
        
        //Author: Seyed-Rahmatoll Javadi
        private bool isOnIce;
        public int MaximumSpeed => maximumSpeed;

        // Author: David Pagotto
        public bool IsGrounded { get; private set; }

        // Author: Félix Bernier
        public bool IsAirTiming { get; private set; }
        public bool IsFlipped { get; private set; }

        //Author: Seyed-Rahmatoll Javadi
        public float Speed { get; private set; }

        // Author: David Pagotto
        public bool Freeze
        {
            get => rbVehicleBody.constraints == RigidbodyConstraints2D.FreezeAll;
            set {
                if (value)
                {
                    velocityBeforeFreeze = rbVehicleBody.velocity;
                    rbVehicleBody.constraints = RigidbodyConstraints2D.FreezeAll;
                } else
                {
                    rbVehicleBody.velocity = velocityBeforeFreeze;
                    rbVehicleBody.constraints = RigidbodyConstraints2D.None;
                }
            }
        }

        // Author: Benoit Simon-Turgeon
        private bool isGoingForward = true;

        private void Awake()
        {
            // Author: Benoit Simon-Turgeon
            var vehicle = GetComponent<Vehicle>();
            rbVehicleBody = vehicle.VehicleBody.GetComponent<Rigidbody2D>();
            rbRearWheel = vehicle.RearWheel.GetComponent<Rigidbody2D>();
            rbFrontWheel = vehicle.FrontWheel.GetComponent<Rigidbody2D>();
            vehicleCollider = rbVehicleBody.GetComponentInParent<PolygonCollider2D>();
            
            bonusNitroChangedEventChannel = Finder.BonusNitroChangedEventChannel;
            bonusNitroChangedEventChannel.OnNitroChanged += OnNitroChanged;
            //Author: Seyed-Rahmatoll Javadi
            Speed = 0;
            isOnIce = false;
        }

        // Author: Benoit Simon-Turgeon
        private void OnNitroChanged(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        // Author: Benoit Simon-Turgeon
        private void OnDestroy()
        {
            bonusNitroChangedEventChannel.OnNitroChanged -= OnNitroChanged;
        }

        private void FixedUpdate()
        {
            //Author: Seyed-Rahmatoll Javadi
            isOnIce = rbFrontWheel.IsTouchingLayers(icedGroundTags) && rbRearWheel.IsTouchingLayers(icedGroundTags);
            if (isOnIce)
            {
                rbFrontWheel.sharedMaterial.friction = IceFriction;   
                rbRearWheel.sharedMaterial.friction = IceFriction;
            }
            else
            {
                rbFrontWheel.sharedMaterial.friction = DefaultFriction;   
                rbRearWheel.sharedMaterial.friction = DefaultFriction;
            }

            // Author: David Pagotto
            IsGrounded = (rbFrontWheel.IsTouchingLayers(groundTags) && rbRearWheel.IsTouchingLayers(groundTags)) || 
                         (rbFrontWheel.IsTouchingLayers(bridgeTags) && rbRearWheel.IsTouchingLayers(bridgeTags)) || 
                         isOnIce;
            // Author: Félix Bernier
            IsAirTiming = !(rbFrontWheel.IsTouchingLayers(groundTags)) && !(rbRearWheel.IsTouchingLayers(groundTags)) && !rbVehicleBody.IsTouchingLayers(groundTags) &&
                          !(rbFrontWheel.IsTouchingLayers(bridgeTags)) && !(rbRearWheel.IsTouchingLayers(bridgeTags)) && !rbVehicleBody.IsTouchingLayers(bridgeTags) &&
                          !(rbFrontWheel.IsTouchingLayers(icedGroundTags)) && !(rbRearWheel.IsTouchingLayers(icedGroundTags)) && !rbVehicleBody.IsTouchingLayers(icedGroundTags);
            // Author: Félix Bernier & Benoit Simon-Turgeon
            IsFlipped = vehicleCollider.IsTouchingLayers(groundTags) || vehicleCollider.IsTouchingLayers(bridgeTags);
            //Author: Seyed-Rahmatoll Javadi
            float currentSpeed = Mathf.Round(rbVehicleBody.velocity.magnitude * MPerSecondToKMPerSecond);
            if(currentSpeed <= MaximumSpeed)
                Speed = currentSpeed;
        }

        // Author: Benoit Simon-Turgeon
        private void UpdateWheelsSpeed()
        {
            var temp = isGoingForward ? -1 : 1;
            rbFrontWheel.AddTorque(torque/wheelsSpeedDivider * speedMultiplier * temp * Time.fixedDeltaTime);
            rbRearWheel.AddTorque(torque/wheelsSpeedDivider * speedMultiplier * temp * Time.fixedDeltaTime);
        }

        // Author: Benoit Simon-Turgeon
        public void Forward()
        {
            UpdateWheelsSpeed();
            rbVehicleBody.AddForce(torque * Time.fixedDeltaTime * speedMultiplier * transform.right);

            // http://answers.unity.com/answers/364946/view.html
            var carBodyVelocity = transform.InverseTransformDirection(rbVehicleBody.velocity).x;
            if (carBodyVelocity < maxSpeedToTiltVehicleWhenAccelerating)
                rbVehicleBody.AddTorque(vehicleRotationSpeed / tiltDifferenceDivider * Time.fixedDeltaTime);

            isGoingForward = true;
        }

        // Author: Benoit Simon-Turgeon
        public void Backward()
        {
            UpdateWheelsSpeed();
            rbVehicleBody.AddForce(torque * Time.fixedDeltaTime * speedMultiplier * -transform.right);

            var carBodyVelocity = transform.InverseTransformDirection(rbVehicleBody.velocity).x;
            if (carBodyVelocity > -maxSpeedToTiltVehicleWhenAccelerating)
                rbVehicleBody.AddTorque(vehicleRotationSpeed * Time.fixedDeltaTime * -1);

            isGoingForward = false;
        }

        // Author: Benoit Simon-Turgeon
        public void Brake()
        {
            var carBodyVelocity = transform.InverseTransformDirection(rbVehicleBody.velocity).x;
            if (carBodyVelocity == 0) return;

            // To prevent speed multiplier from nitro being overwritten
            var currentMultiplier = speedMultiplier;
            speedMultiplier = (speedMultiplier > brakeForce) ? speedMultiplier : brakeForce;

            if (carBodyVelocity > 0)
                Backward();
            else
                Forward();
            
            speedMultiplier = currentMultiplier;
        }

        // Author: David Pagotto
        public void RotateCw()
        {
            // Author: Benoit Simon-Turgeon
            isGoingForward = true;
            // Author: David Pagotto
            rbVehicleBody.AddTorque(vehicleRotationSpeed * Time.fixedDeltaTime * -1);
        }

        // Author: David Pagotto
        public void RotateCcw()
        {
            // Author: Benoit Simon-Turgeon
            isGoingForward = false;
            // Author: David Pagotto
            rbVehicleBody.AddTorque(vehicleRotationSpeed * Time.fixedDeltaTime);
        }
        
        // Author: Seyed-Rahmatoll Javadi
        public void ImpulseUp(float force)
        {
            rbVehicleBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }
}