using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using UnityEngine;

namespace Formidable.Modules
{
    public class DoorOpenModule : Module
    {

        public static readonly KeyCode _KeyCode = KeyCode.Keypad4;

        private static readonly float _doorDistance = 25f;

        public DoorOpenModule(ModuleInformation moduleInformation) : base(moduleInformation)
        {

        }

        public override void Toggle()
        {
            foreach (Door door in GameObject.FindObjectsOfType<Door>())
            {
                if ((door.DoorState == EDoorState.Open) || Vector3.Distance(Camera.main.transform.position, door.transform.position) > _doorDistance)
                    continue;

                door.enabled = true;
                door.DoorState = EDoorState.Shut;

                if (Singleton<GameWorld>.Instance != null)
                {
                    MethodInfo isLocalGameMethodInfo = Singleton<GameWorld>.Instance.GetType().GetMethod("IsLocalGame", (BindingFlags.Instance | BindingFlags.NonPublic));

                    bool isLocalGameReturnValue = ((bool) isLocalGameMethodInfo.Invoke(Singleton<GameWorld>.Instance, null));

                    if (isLocalGameReturnValue)
                    {
                        MethodInfo methodInfo = door.GetType().BaseType.GetMethod("Open", (BindingFlags.Instance | BindingFlags.NonPublic));

                        if (methodInfo == null)
                            continue;

                        methodInfo.Invoke(door, null);
                    }
                }
            }
        }

    }

}
