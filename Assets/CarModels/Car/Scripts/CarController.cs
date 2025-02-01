using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public enum CarDriveType
    {
        AWD,
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    public enum SpeedType
    {
        MPH,
        KPH
    }

    public class CarController : MonoBehaviour
    {
        [SerializeField] private CarDriveType m_CarDriveType = CarDriveType.RearWheelDrive;
        [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
        [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
        [SerializeField] private Vector3 m_CentreOfMassOffset;
        [SerializeField] private float m_MaximumSteerAngle;
        [SerializeField] private bool m_TractionControlEnabled = true;
        [SerializeField] private bool m_DownforceEnabled = true;
        [SerializeField] private float m_FullTorqueOverAllWheels;
        [SerializeField] private float m_ReverseTorque;
        [SerializeField] private float m_MaxHandbrakeTorque;
        [SerializeField] private float m_Downforce = 50f;
        [SerializeField] private SpeedType m_SpeedType;
        [SerializeField] private float m_Topspeed = 150;
        [SerializeField] private static int NoOfGears = 6;
        [SerializeField] private float m_RevRangeBoundary = 1f;
        [SerializeField] private float m_BrakeTorque;

        private Quaternion[] m_WheelMeshLocalRotations;
        private Vector3 m_Prevpos, m_Pos;
        private float m_SteerAngle;
        private int m_GearNum;
        private float m_GearFactor;
        private Rigidbody m_Rigidbody;

        private float m_GearShiftDelay = 0.3f;
        private float m_GearShiftTimer = 0f;

        public float BrakeInput { get; private set; }
        public float CurrentSteerAngle { get { return m_SteerAngle; } }
        public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * 3.6f; } }
        public float MaxSpeed { get { return m_Topspeed; } }
        public float Revs { get; private set; }
        public float AccelInput { get; private set; }

        public int GetCurrentGear()
        {
            return m_GearNum;
        }

        private void Start()
        {
            InitializeWheelMeshes();
            m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(float steering, float accel, float footbrake, float handbrake)
        {
            UpdateWheelPositionsAndRotations();

            steering = Mathf.Clamp(steering, -1, 1);
            AccelInput = accel = Mathf.Clamp(accel, 0, 1);
            BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
            handbrake = Mathf.Clamp(handbrake, 0, 1);

            m_SteerAngle = steering * m_MaximumSteerAngle;
            ApplySteering(m_SteerAngle);

            ApplyDrive(accel, footbrake);
            CapSpeed();

            if (handbrake > 0f)
            {
                ApplyHandbrake(handbrake);
            }

            if (m_DownforceEnabled)
                AddDynamicDownForce();

            if (m_TractionControlEnabled)
                TractionControl();

            UpdateEngineSound();
            ShiftGears();
        }

        private void InitializeWheelMeshes()
        {
            m_WheelMeshLocalRotations = new Quaternion[4];
            for (int i = 0; i < 4; i++)
            {
                m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
            }
        }

        private void UpdateWheelPositionsAndRotations()
        {
            for (int i = 0; i < 4; i++)
            {
                Quaternion quat;
                Vector3 position;
                m_WheelColliders[i].GetWorldPose(out position, out quat);
                m_WheelMeshes[i].transform.position = position;
                m_WheelMeshes[i].transform.rotation = quat;
            }
        }

        private void ApplySteering(float steerAngle)
        {
            m_WheelColliders[0].steerAngle = steerAngle;
            m_WheelColliders[1].steerAngle = steerAngle;
        }

        private void ApplyHandbrake(float handbrake)
        {
            var hbTorque = handbrake * m_MaxHandbrakeTorque;
            m_WheelColliders[2].brakeTorque = hbTorque;  
            m_WheelColliders[3].brakeTorque = hbTorque;  
            m_WheelColliders[2].motorTorque = 0f;  
            m_WheelColliders[3].motorTorque = 0f;  

            WheelFrictionCurve rearLeftFriction = m_WheelColliders[2].forwardFriction;
            WheelFrictionCurve rearRightFriction = m_WheelColliders[3].forwardFriction;

            rearLeftFriction.stiffness = Mathf.Clamp(rearLeftFriction.stiffness - 0.2f, 0.3f, 0.7f);
            rearRightFriction.stiffness = Mathf.Clamp(rearRightFriction.stiffness - 0.2f, 0.3f, 0.7f);

            m_WheelColliders[2].forwardFriction = rearLeftFriction;  
            m_WheelColliders[3].forwardFriction = rearRightFriction; 
        }

        private void CapSpeed()
        {
            float speed = m_Rigidbody.velocity.magnitude;
            switch (m_SpeedType)
            {
                case SpeedType.MPH:
                    speed *= 2.23693629f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.velocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.velocity.normalized;
                    break;

                case SpeedType.KPH:
                    speed *= 3.6f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.velocity = (m_Topspeed / 3.6f) * m_Rigidbody.velocity.normalized;
                    break;
            }
        }

        private void ApplyDrive(float accel, float footbrake)
        {
            float currentTorque = 0f;
            float combinedInput = accel - footbrake;

            if (combinedInput > 0) 
            {
                currentTorque = combinedInput * m_FullTorqueOverAllWheels;
            }
            else if (combinedInput < 0) 
            {
                currentTorque = combinedInput * m_ReverseTorque;
            }
            switch (m_CarDriveType)
            {
                case CarDriveType.AWD:
                    ApplyTorqueToAllWheels(currentTorque);
                    break;

                case CarDriveType.FrontWheelDrive:
                    ApplyTorqueToFrontWheels(currentTorque);
                    break;

                case CarDriveType.RearWheelDrive:
                    ApplyTorqueToRearWheels(currentTorque);
                    break;

                case CarDriveType.FourWheelDrive:
                    ApplyTorqueToAllWheels(currentTorque);
                    break;

                default:
                    break;
            }
        }

        private void ApplyTorqueToFrontWheels(float torque)
        {
            m_WheelColliders[0].motorTorque = torque / 2f;
            m_WheelColliders[1].motorTorque = torque / 2f;
        }

        private void ApplyTorqueToRearWheels(float torque)
        {
            m_WheelColliders[2].motorTorque = torque / 2f;
            m_WheelColliders[3].motorTorque = torque / 2f;
        }

        private void ApplyTorqueToAllWheels(float torque)
        {
            m_WheelColliders[0].motorTorque = torque / 4f;
            m_WheelColliders[1].motorTorque = torque / 4f;
            m_WheelColliders[2].motorTorque = torque / 4f;
            m_WheelColliders[3].motorTorque = torque / 4f;
        }

        private void ApplyBrakes(float brakeInput)
        {
            float brakeTorque = brakeInput * m_BrakeTorque;

            m_WheelColliders[0].brakeTorque = brakeTorque;
            m_WheelColliders[1].brakeTorque = brakeTorque;
            m_WheelColliders[2].brakeTorque = brakeTorque;
            m_WheelColliders[3].brakeTorque = brakeTorque;
        }

        private void ApplyRearWheelDrive(float accel)
        {
            WheelHit frontLeftHit;
            WheelHit frontRightHit;
            WheelHit rearLeftHit;
            WheelHit rearRightHit;
            m_WheelColliders[0].GetGroundHit(out frontLeftHit);
            m_WheelColliders[1].GetGroundHit(out frontRightHit);
            m_WheelColliders[2].GetGroundHit(out rearLeftHit);
            m_WheelColliders[3].GetGroundHit(out rearRightHit);
            float frontGrip = (frontLeftHit.forwardSlip + frontRightHit.forwardSlip) / 2f;
            float rearGrip = (rearLeftHit.forwardSlip + rearRightHit.forwardSlip) / 2f;
            float tractionFactor = m_TractionControlEnabled ? 1.5f : 0.5f;  

            float rearTorque = accel * m_FullTorqueOverAllWheels * tractionFactor;

            if (m_TractionControlEnabled)
            {
                rearTorque *= 0.5f;  
            }
            else
            {
                rearTorque *= 2.0f; 
            }
            m_WheelColliders[2].motorTorque = rearTorque / 2f;
            m_WheelColliders[3].motorTorque = rearTorque / 2f;
            ReduceRearGrip(m_TractionControlEnabled);
            ApplyDownforce(CurrentSpeed, reduceRearGrip: !m_TractionControlEnabled);
            AdjustSteering(frontGrip, rearGrip);
        }
        private void ReduceRearGrip(bool tractionControlEnabled)
        {
            foreach (var wheel in new[] { m_WheelColliders[2], m_WheelColliders[3] })
            {
                WheelFrictionCurve forwardFriction = wheel.forwardFriction;

                if (tractionControlEnabled)
                {
                    forwardFriction.stiffness = Mathf.Clamp(forwardFriction.stiffness - 0.5f, 0.1f, 0.6f);
                }
                else
                {
                    forwardFriction.stiffness = Mathf.Clamp(forwardFriction.stiffness + 0.5f, 0.6f, 1f);
                }

                wheel.forwardFriction = forwardFriction;

                WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

                if (tractionControlEnabled)
                {
                    sidewaysFriction.stiffness = Mathf.Clamp(sidewaysFriction.stiffness - 0.5f, 0.1f, 0.6f);
                }
                else
                {
                    sidewaysFriction.stiffness = Mathf.Clamp(sidewaysFriction.stiffness + 0.5f, 0.6f, 1f);
                }

                wheel.sidewaysFriction = sidewaysFriction;
            }
        }
        private void ApplyDownforce(float speed, bool reduceRearGrip = false)
        {
            float downforce = speed * 0.015f;  
            if (reduceRearGrip)
            {
                m_WheelColliders[2].attachedRigidbody.AddForce(Vector3.down * downforce * 0.2f);  
                m_WheelColliders[3].attachedRigidbody.AddForce(Vector3.down * downforce * 0.2f);
                m_WheelColliders[0].attachedRigidbody.AddForce(Vector3.down * downforce * 0.8f);  
                m_WheelColliders[1].attachedRigidbody.AddForce(Vector3.down * downforce * 0.8f);
            }
            else
            {
                foreach (var wheel in m_WheelColliders)
                {
                    wheel.attachedRigidbody.AddForce(Vector3.down * downforce);
                }
            }
        }
        private void AdjustSteering(float frontGrip, float rearGrip)
        {
            if (frontGrip < rearGrip)
            {
                m_SteerAngle *= 1.1f;
            }
            else
            {
                m_SteerAngle *= 0.9f;
            }
        }


        private void ApplyFrontWheelDrive(float accel)
        {
            float thrustTorque = accel * (m_FullTorqueOverAllWheels / 2f);
            WheelHit rearLeftHit;
            WheelHit rearRightHit;
            m_WheelColliders[2].GetGroundHit(out rearLeftHit);
            m_WheelColliders[3].GetGroundHit(out rearRightHit);
            float slipDifference = Mathf.Abs(rearLeftHit.forwardSlip - rearRightHit.forwardSlip);
            if (m_TractionControlEnabled && slipDifference > 0.3f)
            {
                thrustTorque *= Mathf.Lerp(0.5f, 1f, 1 - slipDifference);
            }

            float speedFactor = Mathf.Clamp01(CurrentSpeed / m_Topspeed);
            float powerMultiplier = 1.2f - (speedFactor * 0.2f);
            powerMultiplier = Mathf.Clamp(powerMultiplier, 1f, 1.5f);
            float rearWeightFactor = Mathf.Clamp01((m_Rigidbody.mass * Physics.gravity.y * m_CentreOfMassOffset.z) / (1000f * 10f));
            thrustTorque *= 1f + (rearWeightFactor * 0.5f);
            if (!m_TractionControlEnabled)
            {
                float steeringAngle = m_WheelColliders[0].steerAngle;
                float steeringAngleFactor = Mathf.Abs(steeringAngle) / 45f;
                float understeerBias = Mathf.Clamp01(steeringAngleFactor * 0.6f);  
                thrustTorque *= 1 + understeerBias;
            }

            thrustTorque *= powerMultiplier;
            if (rearLeftHit.forwardSlip > 1.0f || rearRightHit.forwardSlip > 1.0f)
            {
                thrustTorque *= 0.6f;
            }
            m_WheelColliders[0].motorTorque = thrustTorque / 2f;  
            m_WheelColliders[1].motorTorque = thrustTorque / 2f;  
        }

 private void ApplyFourWheelDrive(float accel)
{
    WheelHit frontLeftHit;
    WheelHit frontRightHit;
    WheelHit rearLeftHit;
    WheelHit rearRightHit;
    m_WheelColliders[0].GetGroundHit(out frontLeftHit);
    m_WheelColliders[1].GetGroundHit(out frontRightHit);
    m_WheelColliders[2].GetGroundHit(out rearLeftHit);
    m_WheelColliders[3].GetGroundHit(out rearRightHit);

    float frontLeftGrip = frontLeftHit.forwardSlip;
    float frontRightGrip = frontRightHit.forwardSlip;
    float rearLeftGrip = rearLeftHit.forwardSlip;
    float rearRightGrip = rearRightHit.forwardSlip;
    float frontGrip = (frontLeftGrip + frontRightGrip) / 2f;
    float rearGrip = (rearLeftGrip + rearRightGrip) / 2f;

    float frontTorque = accel * m_FullTorqueOverAllWheels * 0.5f;
    float rearTorque = accel * m_FullTorqueOverAllWheels * 0.5f;
    m_WheelColliders[0].motorTorque = frontTorque / 2f;  
    m_WheelColliders[1].motorTorque = frontTorque / 2f;  
    m_WheelColliders[2].motorTorque = rearTorque / 2f;   
    m_WheelColliders[3].motorTorque = rearTorque / 2f;   
    ApplyDownforce(CurrentSpeed);
}
        private void ApplyDynamicAllWheelDrive(float accel)
        {
            WheelHit frontLeftHit;
            WheelHit frontRightHit;
            WheelHit rearLeftHit;
            WheelHit rearRightHit;

            m_WheelColliders[0].GetGroundHit(out frontLeftHit);
            m_WheelColliders[1].GetGroundHit(out frontRightHit);
            m_WheelColliders[2].GetGroundHit(out rearLeftHit);
            m_WheelColliders[3].GetGroundHit(out rearRightHit);
            float frontLeftGrip = frontLeftHit.forwardSlip;
            float frontRightGrip = frontRightHit.forwardSlip;
            float rearLeftGrip = rearLeftHit.forwardSlip;
            float rearRightGrip = rearRightHit.forwardSlip;
            float frontGrip = (frontLeftGrip + frontRightGrip) / 2f;
            float rearGrip = (rearLeftGrip + rearRightGrip) / 2f;
            float tractionFactor = 1f;

            if (frontGrip > 0.3f || rearGrip > 0.3f) 
            {
                tractionFactor = Mathf.Clamp(1f - Mathf.Max(frontGrip, rearGrip) * 3f, 0f, 1f);
            }
            float frontLeftTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
            float frontRightTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
            float rearLeftTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
            float rearRightTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
            float maxFrontGrip = Mathf.Max(frontLeftGrip, frontRightGrip);
            float maxRearGrip = Mathf.Max(rearLeftGrip, rearRightGrip);
            if (maxRearGrip > maxFrontGrip)
            {
                rearLeftTorque *= 1.2f;
                rearRightTorque *= 1.2f;
            }
            else if (maxFrontGrip > maxRearGrip)
            {
                frontLeftTorque *= 1.2f;
                frontRightTorque *= 1.2f;
            }
            else
            {
                frontLeftTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
                frontRightTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
                rearLeftTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
                rearRightTorque = accel * m_FullTorqueOverAllWheels * 0.25f * tractionFactor;
            }
            m_WheelColliders[0].motorTorque = frontLeftTorque;
            m_WheelColliders[1].motorTorque = frontRightTorque;
            m_WheelColliders[2].motorTorque = rearLeftTorque;
            m_WheelColliders[3].motorTorque = rearRightTorque;
            if (CurrentSpeed > 50f)
            {
                m_SteerAngle *= 0.8f; 
            }
            ApplyDownforce(CurrentSpeed);
        }


        private void ShiftGears()
        {
            if (m_GearShiftTimer > 0)
            {
                m_GearShiftTimer -= Time.deltaTime;
                return;
            }

            if (Revs > 0.85f && m_GearNum < NoOfGears - 1)  
            {
                m_GearNum++;
                m_GearShiftTimer = m_GearShiftDelay;
            }
            else if (Revs < 0.25f && m_GearNum > 0)  
            {
                m_GearNum--;
                m_GearShiftTimer = m_GearShiftDelay;
            }
        }

        private void UpdateEngineSound()
        {
            float gearRatio = (float)m_GearNum / NoOfGears;
            float speedFactor = Mathf.Clamp01(CurrentSpeed / MaxSpeed);
            Revs = Mathf.Lerp(gearRatio, speedFactor, m_RevRangeBoundary);
        }

        private void AddDynamicDownForce()
        {
            float speedFactor = Mathf.Clamp01(CurrentSpeed / MaxSpeed);
            float downforceAmount = m_Downforce * speedFactor; 
            m_Rigidbody.AddForce(-transform.up * downforceAmount, ForceMode.Force);
        }

        private void TractionControl()
        {
            float maxAllowedSlip = 0.1f; 
            float slipFactor = 1f; 

            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelHit;
                m_WheelColliders[i].GetGroundHit(out wheelHit);
                float slip = Mathf.Abs(wheelHit.forwardSlip);

                if (slip > maxAllowedSlip)
                {
                    slipFactor = Mathf.Clamp01(1f - (slip - maxAllowedSlip) * 2f); 
                    m_WheelColliders[i].motorTorque *= slipFactor;
                }
                if (m_TractionControlEnabled)
                {
                    if (i < 2) 
                    {
                        if (Mathf.Abs(wheelHit.forwardSlip) > 0.3f)
                        {
                            m_WheelColliders[i].motorTorque *= 0.5f; 
                        }
                    }
                }
            }
            AdjustForOversteer();
        }

        private void AdjustForOversteer()
        {
            if (CurrentSpeed < 20f)
            {
                m_WheelColliders[2].motorTorque *= 0.5f; 
                m_WheelColliders[3].motorTorque *= 0.5f; 
            }
            else if (CurrentSpeed < 50f)
            {
                m_WheelColliders[2].motorTorque *= 0.7f; 
                m_WheelColliders[3].motorTorque *= 0.7f; 
            }
            else
            {
                m_WheelColliders[2].motorTorque *= 1f; 
                m_WheelColliders[3].motorTorque *= 1f; 
            }
        }


    }
}
