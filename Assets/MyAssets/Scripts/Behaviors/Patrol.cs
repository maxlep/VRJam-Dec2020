using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Shapes;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Patrol : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform patrolPointContainer;
    
    [SerializeField] [PropertySpace(5f, 0f)]
    private GameObject PatrolPointPrefab;

    [SerializeField] [PropertySpace(0f, 5f)]
    private bool randomOrder = false;

    [InfoBox("Patrol Points should only be added via buttons. Removing patrol point deletes its GameObject. " +
             "Patrol Points require \"PatrolPoint\" tag (Add button does this for you).")]
    [Required("Null or missing patrol points!")]
    [SerializeField] [ListDrawerSettings(Expanded = true, CustomRemoveElementFunction = "RemovePatrolPoint")] 
    private List<Transform> PatrolPoints = new List<Transform>();

    private string patrolPointTag = "PatrolPoint";
    private string patrolPointPrefix = "Point";
    private int nextPointIndex = 0;
    private bool isStopped;

    private void ValidatePatrolPoints()
    {
        //Remove points that are null or dont have correct tag
        for (int i = PatrolPoints.Count - 1; i > 0; i--)
        {
            if (PatrolPoints[i] == null || PatrolPoints[i].tag != patrolPointTag)
            {
                PatrolPoints.RemoveAt(i);
            }
        }
    }

    [Button(ButtonSizes.Medium)]
    private void AddPatrolPoint()
    {
        //Spawn at last point or this transform
        Vector3 newPointPosition = (PatrolPoints.Count > 0) ?
            PatrolPoints[PatrolPoints.Count - 1].position : transform.position;

        var newPoint = GameObject.Instantiate(PatrolPointPrefab, newPointPosition, 
            Quaternion.identity, transform);

        newPoint.tag = patrolPointTag;
        newPoint.name = $"{patrolPointPrefix}{PatrolPoints.Count + 1}";
        PatrolPoints.Add(newPoint.transform);
    }
    
    [Button(ButtonSizes.Medium)]
    private void RenamePatrolPoints()
    {
        for (int i = 0; i < PatrolPoints.Count; i++)
        {
            PatrolPoints[i].name = $"{patrolPointPrefix}{i + 1}";
        }
    }

    private void RemovePatrolPoint(Transform pointTransform)
    {
        //DestroyImmediate(pointTransform.gameObject);
        #if UNITY_EDITOR
        Undo.DestroyObjectImmediate(pointTransform.gameObject);
        #endif
    }


    private void OnValidate()
    {
        ValidatePatrolPoints();
    }

    private void Awake()
    {
        UpdateCurrentPoint();

        foreach (var patrolPoint in PatrolPoints)
        {
            patrolPoint.parent = patrolPointContainer;
        }
    }

    private void Update()
    {
        MoveToPatrolPoint();
    }

    private void MoveToPatrolPoint()
    {
        if (isStopped) return;
        if (PatrolPoints.IsNullOrEmpty()) return;
        
        
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            UpdateCurrentPoint();
        }
    }

    private void UpdateCurrentPoint()
    {
        agent.SetDestination(PatrolPoints[nextPointIndex].position);

        if (randomOrder)
        {
            nextPointIndex = Random.Range(0, PatrolPoints.Count);
        }
        else
            nextPointIndex++;

        //Wrap around
        if (nextPointIndex > PatrolPoints.Count - 1)
            nextPointIndex = 0;

        
    }

    public void StopPatrol()
    {
        agent.isStopped = true;
        isStopped = true;
    }


    #region Gizmos

    private void OnDrawGizmos()
    {
        Draw.LineGeometry = LineGeometry.Volumetric3D;
        Draw.LineThicknessSpace = ThicknessSpace.Meters;
        Draw.LineThickness = .2f;
        Draw.LineDashStyle = DashStyle.DefaultDashStyleLine;

        //Draw spawn sphere
        // Draw.Cuboid(ShapesBlendMode.Transparent, ThicknessSpace.Meters, transform.position, Quaternion.identity, 
        //     Vector3.one * 1.5f, new Color(209f/255f, 130f/255f, 171f/255f, .5f));

        DrawPatrolPathGizmos();
    }

    private void DrawPatrolPathGizmos()
    {
        if (PatrolPoints.IsNullOrEmpty()) return;
        
        //Draw dotted line from spawn to first patrol point
        Draw.LineDashed(transform.position, PatrolPoints[0].position,
            new Color(150f/255f, 50f/255f, 220f/255f, .5f));
        
        
        //Draw dotted lines with arrows (cones) between patrol points
        for (int i = 0; i < PatrolPoints.Count; i++)
        {
            Vector3 startPoint = PatrolPoints[i].position;
            Vector3 endPoint;
            Vector3 forward;
            Vector3 coneOffset;
            float coneRadius = .5f;
            float coneHeight = 1.35f;
            float startSphereRadius = 1f;
            Color lineColor = new Color(235f/255f, 201f/255f, 12f/255f, .5f);
            Color coneColor = new Color(235f/255f, 100f/255f, 0f/255f, .5f);
            Color startSphereColor = new Color(181f/255f, 14f/255f, 21f/255f, .5f);
            Color startConeColor = new Color(150f/255f, 50f/255f, 220f/255f, .5f);

            if (i == 0 && PatrolPoints.Count > 1)
            {
                Draw.Sphere(startPoint, startSphereRadius, startSphereColor);
                Draw.Cone(transform.position, transform.rotation, coneRadius, coneHeight, startConeColor);
            }

            if (i < PatrolPoints.Count - 1)
            {
                endPoint = PatrolPoints[i + 1].position;
            }
            else
            {
                endPoint = PatrolPoints[0].position;
            }

            //If only 1 point, point cone forward
            if (PatrolPoints.Count == 1)
            {
                forward = transform.forward;
                coneOffset = Vector3.zero;
            }
            else
            {
                forward = (endPoint- startPoint).normalized;
                coneOffset = -forward * (coneHeight * 1.15f);
            }

            
            Draw.LineDashed(startPoint, endPoint, lineColor);
            Quaternion coneRot = (!Mathf.Approximately(forward.magnitude, 0f))
                ? Quaternion.LookRotation(forward)
                : Quaternion.identity;
            Draw.Cone(endPoint + coneOffset, coneRot, coneRadius, coneHeight, coneColor);
        }
    }
    
    
    #endregion
}
