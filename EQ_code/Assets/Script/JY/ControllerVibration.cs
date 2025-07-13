using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class ControllerVibration : MonoBehaviour
{
    public enum Hand { Left, Right }

    public void Vibrate(Hand hand, float amplitude, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(
            hand == Hand.Left ? XRNode.LeftHand : XRNode.RightHand);

        if (device.isValid)
        {
            Debug.Log($"Haptics OK on {hand} hand");
            device.SendHapticImpulse(0u, amplitude, duration);
        }
        else
        {
            Debug.LogWarning($"Haptics failed: {hand} not valid");
        }

    }

    public void VibrateBoth(float amplitude, float duration)
    {
        Vibrate(Hand.Left, amplitude, duration);
        Vibrate(Hand.Right, amplitude, duration);
    }
}
