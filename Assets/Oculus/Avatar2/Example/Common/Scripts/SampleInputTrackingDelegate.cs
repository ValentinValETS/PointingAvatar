using Oculus.Avatar2;
using UnityEngine;
using Node = UnityEngine.XR.XRNode;

/*
 *
 */
public class SampleInputTrackingDelegate : OvrAvatarInputTrackingDelegate
{
    private OVRCameraRig _ovrCameraRig = null;

    /** EDIT **/
    private GameObject _leftControllerOffset;
    private GameObject _rightControllerOffset;
    /** EDIT **/

    public SampleInputTrackingDelegate(OVRCameraRig ovrCameraRig,
        /** EDIT **/
        GameObject leftControllerOffset, GameObject rightControllerOffset
        /** EDIT **/
        )
    {
        _ovrCameraRig = ovrCameraRig;

        /** EDIT **/
        _leftControllerOffset = leftControllerOffset;
        _rightControllerOffset = rightControllerOffset;
        /** EDIT **/
    }

    public override bool GetRawInputTrackingState(out OvrAvatarInputTrackingState inputTrackingState)
    {
        inputTrackingState = default;

        bool leftControllerActive = false;
        bool rightControllerActive = false;
        if (OVRInput.GetActiveController() != OVRInput.Controller.Hands)
        {
            leftControllerActive = OVRInput.GetControllerOrientationTracked(OVRInput.Controller.LTouch);
            rightControllerActive = OVRInput.GetControllerOrientationTracked(OVRInput.Controller.RTouch);
        }

        if (_ovrCameraRig)
        {
            inputTrackingState.headsetActive = true;
            inputTrackingState.leftControllerActive = leftControllerActive;
            inputTrackingState.rightControllerActive = rightControllerActive;
            inputTrackingState.leftControllerVisible = false;
            inputTrackingState.rightControllerVisible = false;
            inputTrackingState.headset = _ovrCameraRig.centerEyeAnchor;
            inputTrackingState.leftController = _ovrCameraRig.leftControllerAnchor;
            inputTrackingState.rightController = _ovrCameraRig.rightControllerAnchor;
            return true;
        }
        else if (OVRNodeStateProperties.IsHmdPresent())
        {
            inputTrackingState.headsetActive = true;
            inputTrackingState.leftControllerActive = leftControllerActive;
            inputTrackingState.rightControllerActive = rightControllerActive;
            inputTrackingState.leftControllerVisible = true;
            inputTrackingState.rightControllerVisible = true;

            if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.CenterEye, NodeStatePropertyType.Position,
                OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out var headPos))
            {
                inputTrackingState.headset.position = headPos;
            }
            else
            {
                inputTrackingState.headset.position = Vector3.zero;
            }

            if (OVRNodeStateProperties.GetNodeStatePropertyQuaternion(Node.CenterEye, NodeStatePropertyType.Orientation,
                OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out var headRot))
            {
                inputTrackingState.headset.orientation = headRot;
            }
            else
            {
                inputTrackingState.headset.orientation = Quaternion.identity;
            }

            inputTrackingState.headset.scale = Vector3.one;

            /** EDIT **/
            inputTrackingState.leftController.position = _leftControllerOffset.transform.position;
            inputTrackingState.rightController.position = _rightControllerOffset.transform.position;
            /** EDIT **/

            //inputTrackingState.leftController.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            //inputTrackingState.rightController.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            inputTrackingState.leftController.orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            inputTrackingState.rightController.orientation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            inputTrackingState.leftController.scale = Vector3.one;
            inputTrackingState.rightController.scale = Vector3.one;
            return true;
        }

        return false;
    }
}
