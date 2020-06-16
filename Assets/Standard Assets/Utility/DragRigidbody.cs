using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class DragRigidbody : MonoBehaviour
    {
        private const float k_Spring = 50.0f;
        private const float k_Damper = 5.0f;
        private const float k_Drag = 10.0f;
        private const float k_AngularDrag = 5.0f;
        private const float k_Distance = 0.2f;
        private const bool k_AttachToCenterOfMass = false;

        private SpringJoint m_SpringJoint;


        private void Update()
        {
            // Make sure the user pressed the mouse down
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            Camera mainCamera = FindCamera();

            // We need to actually hit an object
            RaycastHit hit = new RaycastHit();
            if (
                !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                 mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                                 Physics.DefaultRaycastLayers))
            {
                return;
            }
            // We need to hit a rigidbody that is not kinematic
            if (!hit.rigidbody || hit.rigidbody.isKinematic)
            {
                return;
            }

            if (!m_SpringJoint)
            {
                GameObject go = new GameObject("Rigidbody dragger");
                Rigidbody body = go.AddComponent<Rigidbody>();
                m_SpringJoint = go.AddComponent<SpringJoint>();
                body.isKinematic = true;
            }

            m_SpringJoint.transform.position = hit.point;
            m_SpringJoint.anchor = Vector3.zero;

            m_SpringJoint.spring = k_Spring;
            m_SpringJoint.damper = k_Damper;
            m_SpringJoint.maxDistance = k_Distance;
            m_SpringJoint.connectedBody = hit.rigidbody;

            StartCoroutine("DragObject", hit.distance);
        }


        private IEnumerator DragObject(float distance)
        {
            float oldDrag = m_SpringJoint.connectedBody.drag;
            float oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
            m_SpringJoint.connectedBody.drag = k_Drag;
            m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
            Camera mainCamera = FindCamera();
            while (Input.GetMouseButton(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                m_SpringJoint.transform.position = ray.GetPoint(distance);
                yield return null;
            }
            if (m_SpringJoint.connectedBody)
            {
                m_SpringJoint.connectedBody.drag = oldDrag;
                m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                m_SpringJoint.connectedBody = null;
            }
        }


        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }
    }
}
