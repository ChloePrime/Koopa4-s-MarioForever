// GENERATED AUTOMATICALLY FROM 'Assets/InputControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SweetMoleHouse.MarioForever.Generated
{
    public class @InputControl : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputControl()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControl"",
    ""maps"": [
        {
            ""name"": ""Debug"",
            ""id"": ""6012a333-711a-42ef-b3e1-3bad9afd397f"",
            ""actions"": [
                {
                    ""name"": ""FreeCamera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7ecfe918-0a27-40d5-bed2-b2e0458312f7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""47606b67-5af8-45df-8ea6-052a24bb1bb3"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FreeCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bb6ee3cd-a507-4baa-9fcd-0d0d21a91890"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FreeCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9ba7e057-ac92-4bc5-93bb-720277bb4884"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FreeCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6455925c-00b6-4ce7-a600-655d356c91d5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FreeCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""26300b85-c3cb-4136-8145-2fcb2652d541"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FreeCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Mario"",
            ""id"": ""38b4ff7f-e94e-4c23-918a-3e908dc52206"",
            ""actions"": [
                {
                    ""name"": ""HorizontalMove"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5eca29af-819b-43ca-9afc-b54ec1887a46"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""05d1fbfd-bbe7-4662-b002-c843ff0a9e91"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""4ee99cc9-457b-4d3a-a7fd-0895ba1d97fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b08075ce-2c85-4a53-b107-603392af039f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FireOrRun"",
                    ""type"": ""Button"",
                    ""id"": ""510d1c06-0277-42f4-89d7-c617cd955538"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""9e75faab-460c-4029-a8db-f771cfc85ce9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""95571bf4-f2c1-4357-a0bd-aae8e4e4bf27"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""21da4bed-baf7-4263-9266-b91275604124"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c89c8451-b56e-4601-a55a-100ebe588814"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48a0074d-4b1f-4aaf-8539-f76a5ece714e"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireOrRun"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a12507d6-3cc7-4e11-a306-930a9e86faa2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab167f95-173e-4ee6-a32d-0ca1ca803224"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Debug
            m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
            m_Debug_FreeCamera = m_Debug.FindAction("FreeCamera", throwIfNotFound: true);
            // Mario
            m_Mario = asset.FindActionMap("Mario", throwIfNotFound: true);
            m_Mario_HorizontalMove = m_Mario.FindAction("HorizontalMove", throwIfNotFound: true);
            m_Mario_Up = m_Mario.FindAction("Up", throwIfNotFound: true);
            m_Mario_Down = m_Mario.FindAction("Down", throwIfNotFound: true);
            m_Mario_Jump = m_Mario.FindAction("Jump", throwIfNotFound: true);
            m_Mario_FireOrRun = m_Mario.FindAction("FireOrRun", throwIfNotFound: true);
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

        // Debug
        private readonly InputActionMap m_Debug;
        private IDebugActions m_DebugActionsCallbackInterface;
        private readonly InputAction m_Debug_FreeCamera;
        public struct DebugActions
        {
            private @InputControl m_Wrapper;
            public DebugActions(@InputControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @FreeCamera => m_Wrapper.m_Debug_FreeCamera;
            public InputActionMap Get() { return m_Wrapper.m_Debug; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
            public void SetCallbacks(IDebugActions instance)
            {
                if (m_Wrapper.m_DebugActionsCallbackInterface != null)
                {
                    @FreeCamera.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnFreeCamera;
                    @FreeCamera.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnFreeCamera;
                    @FreeCamera.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnFreeCamera;
                }
                m_Wrapper.m_DebugActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @FreeCamera.started += instance.OnFreeCamera;
                    @FreeCamera.performed += instance.OnFreeCamera;
                    @FreeCamera.canceled += instance.OnFreeCamera;
                }
            }
        }
        public DebugActions @Debug => new DebugActions(this);

        // Mario
        private readonly InputActionMap m_Mario;
        private IMarioActions m_MarioActionsCallbackInterface;
        private readonly InputAction m_Mario_HorizontalMove;
        private readonly InputAction m_Mario_Up;
        private readonly InputAction m_Mario_Down;
        private readonly InputAction m_Mario_Jump;
        private readonly InputAction m_Mario_FireOrRun;
        public struct MarioActions
        {
            private @InputControl m_Wrapper;
            public MarioActions(@InputControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @HorizontalMove => m_Wrapper.m_Mario_HorizontalMove;
            public InputAction @Up => m_Wrapper.m_Mario_Up;
            public InputAction @Down => m_Wrapper.m_Mario_Down;
            public InputAction @Jump => m_Wrapper.m_Mario_Jump;
            public InputAction @FireOrRun => m_Wrapper.m_Mario_FireOrRun;
            public InputActionMap Get() { return m_Wrapper.m_Mario; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MarioActions set) { return set.Get(); }
            public void SetCallbacks(IMarioActions instance)
            {
                if (m_Wrapper.m_MarioActionsCallbackInterface != null)
                {
                    @HorizontalMove.started -= m_Wrapper.m_MarioActionsCallbackInterface.OnHorizontalMove;
                    @HorizontalMove.performed -= m_Wrapper.m_MarioActionsCallbackInterface.OnHorizontalMove;
                    @HorizontalMove.canceled -= m_Wrapper.m_MarioActionsCallbackInterface.OnHorizontalMove;
                    @Up.started -= m_Wrapper.m_MarioActionsCallbackInterface.OnUp;
                    @Up.performed -= m_Wrapper.m_MarioActionsCallbackInterface.OnUp;
                    @Up.canceled -= m_Wrapper.m_MarioActionsCallbackInterface.OnUp;
                    @Down.started -= m_Wrapper.m_MarioActionsCallbackInterface.OnDown;
                    @Down.performed -= m_Wrapper.m_MarioActionsCallbackInterface.OnDown;
                    @Down.canceled -= m_Wrapper.m_MarioActionsCallbackInterface.OnDown;
                    @Jump.started -= m_Wrapper.m_MarioActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_MarioActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_MarioActionsCallbackInterface.OnJump;
                    @FireOrRun.started -= m_Wrapper.m_MarioActionsCallbackInterface.OnFireOrRun;
                    @FireOrRun.performed -= m_Wrapper.m_MarioActionsCallbackInterface.OnFireOrRun;
                    @FireOrRun.canceled -= m_Wrapper.m_MarioActionsCallbackInterface.OnFireOrRun;
                }
                m_Wrapper.m_MarioActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @HorizontalMove.started += instance.OnHorizontalMove;
                    @HorizontalMove.performed += instance.OnHorizontalMove;
                    @HorizontalMove.canceled += instance.OnHorizontalMove;
                    @Up.started += instance.OnUp;
                    @Up.performed += instance.OnUp;
                    @Up.canceled += instance.OnUp;
                    @Down.started += instance.OnDown;
                    @Down.performed += instance.OnDown;
                    @Down.canceled += instance.OnDown;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @FireOrRun.started += instance.OnFireOrRun;
                    @FireOrRun.performed += instance.OnFireOrRun;
                    @FireOrRun.canceled += instance.OnFireOrRun;
                }
            }
        }
        public MarioActions @Mario => new MarioActions(this);
        private int m_NewcontrolschemeSchemeIndex = -1;
        public InputControlScheme NewcontrolschemeScheme
        {
            get
            {
                if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
                return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
            }
        }
        public interface IDebugActions
        {
            void OnFreeCamera(InputAction.CallbackContext context);
        }
        public interface IMarioActions
        {
            void OnHorizontalMove(InputAction.CallbackContext context);
            void OnUp(InputAction.CallbackContext context);
            void OnDown(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnFireOrRun(InputAction.CallbackContext context);
        }
    }
}
