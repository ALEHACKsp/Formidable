using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        private static void SetDoorAngle(Door door, float value)
        {
            if (door == null)
                throw new ArgumentNullException(nameof(door));

            FieldInfo fieldInfo = door.GetType().GetField("_currentAngle", (BindingFlags.Instance | BindingFlags.NonPublic));

            if (fieldInfo == null)
                throw new InvalidOperationException();

            fieldInfo.SetValue(door, value);

            if (door.transform.parent != null)
                door.transform.rotation = door.GetDoorRotation(value) * door.transform.parent.rotation;
        }

        public override void Toggle()
        {
            foreach (Door door in GameObject.FindObjectsOfType<Door>())
            {
                if ((door.DoorState == EDoorState.Open) || Vector3.Distance(Camera.main.transform.position, door.transform.position) > _doorDistance)
                    continue;

                door.enabled = true;
                door.DoorState = EDoorState.Open;

                SetDoorAngle(door, door.GetAngle(door.DoorState));
            }
        }

    }

}
