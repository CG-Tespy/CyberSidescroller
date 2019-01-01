using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping =                      1;
        public float lookAheadFactor =              3; 
        // How far to look ahead
        public float lookAheadReturnSpeed =         0.5f;
        public float lookAheadMoveThreshold =       0.1f; 
        // How far in meters the player has to move on the x axis before the camera 
        // decides to look ahead

        [SerializeField] bool _xAxisFollow =        true;
        [SerializeField] bool _yAxisFollow =        true;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private Vector3 camMoveDelta; // Movement of this camera
        private Vector3 targetMoveDelta;

        float targetXMoveDelta { get { return targetMoveDelta.x; } }
        float targetYMoveDelta { get { return targetMoveDelta.x; } }

        public bool xAxisFollow 
        {
            get { return _xAxisFollow; }
            set { _xAxisFollow = value; }
        }

        public bool yAxisFollow 
        {
            get { return _yAxisFollow; }
            set { _yAxisFollow = value; }
        }

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition =              target.position;
            m_OffsetZ =                         (transform.position - target.position).z;
            transform.parent =                  null;
        }


        // Update is called once per frame
        private void Update()
        {
            // Only update lookAhead pos if accelerating or changing direction
            targetMoveDelta =               target.position - m_LastTargetPosition;

            bool updateLookAheadTarget =    false;

            // Depending on the axes being followed, you might need to check the movement
            // on just one axis or the other
            if (xAxisFollow) 
                updateLookAheadTarget =     Mathf.Abs(targetXMoveDelta) > lookAheadMoveThreshold;
            else if (yAxisFollow)
                updateLookAheadTarget =     Mathf.Abs(targetXMoveDelta) > lookAheadMoveThreshold;

            // m_LookAheadPos gets set to a position relative to where the camera is in this frame.
            if (updateLookAheadTarget)
                m_LookAheadPos =            lookAheadFactor * Vector3.right * 
                                            Mathf.Sign(targetXMoveDelta);
                // ^ Moving somewhere away from where the camera is

            else
                m_LookAheadPos =            Vector3.MoveTowards(m_LookAheadPos, 
                                                                Vector3.zero, 
                                                                Time.deltaTime * lookAheadReturnSpeed);
            

            Vector3 aheadTargetPos =        target.position + m_LookAheadPos + 
                                            (Vector3.forward * m_OffsetZ);
            Vector3 newPos =                Vector3.SmoothDamp(transform.position, 
                                                                aheadTargetPos, 
                                                                ref m_CurrentVelocity, damping);

            // Only update position on the followed axes
            if (!xAxisFollow)
                newPos.x =                  transform.position.x;
            if (!yAxisFollow)
                newPos.y =                  transform.position.y;

            transform.position =            new Vector3 (target.transform.position.x, target.transform.position.y, -10.0f);//newPos;
            m_LastTargetPosition =          target.position;
        }
    }
}
