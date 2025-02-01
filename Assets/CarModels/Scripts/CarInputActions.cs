using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace aaa
{
    public partial class @aaa: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @aaa()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""CarInputActions"",
    ""maps"": [
        {
            ""name"": ""Car Controls"",
            ""id"": ""6fcc71b3-e543-4d86-90ba-09df2ed5ebd9"",
            ""actions"": [
                {
                    ""name"": ""Steer"",
                    ""type"": ""Button"",
                    ""id"": ""c818f18e-e565-453d-8ab2-bd12f1b37dba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Throttle"",
                    ""type"": ""Button"",
                    ""id"": ""ade8fb15-467e-4ac2-b6a0-912eaa0bd17d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Button"",
                    ""id"": ""07765105-84ad-4c5d-96fe-56546c82ce28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Handbrake"",
                    ""type"": ""Button"",
                    ""id"": ""23d57bf9-f331-4be7-9ea7-3ab54acb49e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraHorizontal"",
                    ""type"": ""Button"",
                    ""id"": ""d63df9d7-1c21-4751-80df-10bdac446e28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18e46aad-d94d-43ae-91ce-75dea5095358"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38f54b51-218c-415b-858d-49c7296efea0"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd913e30-576a-4db2-9d45-2ae48df6618f"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Handbrake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""SteerAxis"",
                    ""id"": ""2b7b662e-1d0d-4a6c-9527-452379b3de2d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""78d3d9f8-a1a0-4cd6-9ae4-9d12d3605a4a"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""838abd38-e046-4ce1-b935-7ef133407e02"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""e9557299-0264-4df9-afbb-96a21776e8d0"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHorizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d2c6115f-7616-4393-ad4f-ac23e17cb72e"",
                    ""path"": ""<XInputController>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""711b6096-872c-4adf-a6c1-87eca0099278"",
                    ""path"": ""<XInputController>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Car Controls
            m_CarControls = asset.FindActionMap("Car Controls", throwIfNotFound: true);
            m_CarControls_Steer = m_CarControls.FindAction("Steer", throwIfNotFound: true);
            m_CarControls_Throttle = m_CarControls.FindAction("Throttle", throwIfNotFound: true);
            m_CarControls_Brake = m_CarControls.FindAction("Brake", throwIfNotFound: true);
            m_CarControls_Handbrake = m_CarControls.FindAction("Handbrake", throwIfNotFound: true);
            m_CarControls_CameraHorizontal = m_CarControls.FindAction("CameraHorizontal", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Car Controls
        private readonly InputActionMap m_CarControls;
        private List<ICarControlsActions> m_CarControlsActionsCallbackInterfaces = new List<ICarControlsActions>();
        private readonly InputAction m_CarControls_Steer;
        private readonly InputAction m_CarControls_Throttle;
        private readonly InputAction m_CarControls_Brake;
        private readonly InputAction m_CarControls_Handbrake;
        private readonly InputAction m_CarControls_CameraHorizontal;
        public struct CarControlsActions
        {
            private @aaa m_Wrapper;
            public CarControlsActions(@aaa wrapper) { m_Wrapper = wrapper; }
            public InputAction @Steer => m_Wrapper.m_CarControls_Steer;
            public InputAction @Throttle => m_Wrapper.m_CarControls_Throttle;
            public InputAction @Brake => m_Wrapper.m_CarControls_Brake;
            public InputAction @Handbrake => m_Wrapper.m_CarControls_Handbrake;
            public InputAction @CameraHorizontal => m_Wrapper.m_CarControls_CameraHorizontal;
            public InputActionMap Get() { return m_Wrapper.m_CarControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CarControlsActions set) { return set.Get(); }
            public void AddCallbacks(ICarControlsActions instance)
            {
                if (instance == null || m_Wrapper.m_CarControlsActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CarControlsActionsCallbackInterfaces.Add(instance);
                @Steer.started += instance.OnSteer;
                @Steer.performed += instance.OnSteer;
                @Steer.canceled += instance.OnSteer;
                @Throttle.started += instance.OnThrottle;
                @Throttle.performed += instance.OnThrottle;
                @Throttle.canceled += instance.OnThrottle;
                @Brake.started += instance.OnBrake;
                @Brake.performed += instance.OnBrake;
                @Brake.canceled += instance.OnBrake;
                @Handbrake.started += instance.OnHandbrake;
                @Handbrake.performed += instance.OnHandbrake;
                @Handbrake.canceled += instance.OnHandbrake;
                @CameraHorizontal.started += instance.OnCameraHorizontal;
                @CameraHorizontal.performed += instance.OnCameraHorizontal;
                @CameraHorizontal.canceled += instance.OnCameraHorizontal;
            }

            private void UnregisterCallbacks(ICarControlsActions instance)
            {
                @Steer.started -= instance.OnSteer;
                @Steer.performed -= instance.OnSteer;
                @Steer.canceled -= instance.OnSteer;
                @Throttle.started -= instance.OnThrottle;
                @Throttle.performed -= instance.OnThrottle;
                @Throttle.canceled -= instance.OnThrottle;
                @Brake.started -= instance.OnBrake;
                @Brake.performed -= instance.OnBrake;
                @Brake.canceled -= instance.OnBrake;
                @Handbrake.started -= instance.OnHandbrake;
                @Handbrake.performed -= instance.OnHandbrake;
                @Handbrake.canceled -= instance.OnHandbrake;
                @CameraHorizontal.started -= instance.OnCameraHorizontal;
                @CameraHorizontal.performed -= instance.OnCameraHorizontal;
                @CameraHorizontal.canceled -= instance.OnCameraHorizontal;
            }

            public void RemoveCallbacks(ICarControlsActions instance)
            {
                if (m_Wrapper.m_CarControlsActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICarControlsActions instance)
            {
                foreach (var item in m_Wrapper.m_CarControlsActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CarControlsActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CarControlsActions @CarControls => new CarControlsActions(this);
        private int m_NewcontrolschemeSchemeIndex = -1;
        public InputControlScheme NewcontrolschemeScheme
        {
            get
            {
                if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
                return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
            }
        }
        public interface ICarControlsActions
        {
            void OnSteer(InputAction.CallbackContext context);
            void OnThrottle(InputAction.CallbackContext context);
            void OnBrake(InputAction.CallbackContext context);
            void OnHandbrake(InputAction.CallbackContext context);
            void OnCameraHorizontal(InputAction.CallbackContext context);
        }
    }
}
