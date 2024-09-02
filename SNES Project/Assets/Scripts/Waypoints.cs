using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Vector3 WallSize = new Vector3(1, 1, 1);

    private void OnDrawGizmos()
    {
        if (this.transform.childCount < 2)
            return;

        //draw line between each of the checkpoints
        for (int i = 0; i < this.transform.childCount - 1; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.GetChild(i).position, this.transform.GetChild(i + 1).position);
        }
        //last line is red
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.GetChild(this.transform.childCount - 1).position, this.transform.GetChild(0).position);
    }
    public void AngleSizeCheckpointWalls()
    {
        Transform currCheckpoint;
        Transform nextCheckpoint;
        Transform prevCheckpoint;
        int next;
        int prev;
        Quaternion currRotation;
        Quaternion prevRotation;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            next = idxNextCheckpoint(i);
            prev = idxPrevCheckpoint(i);

            //surrounding checkpoints
            currCheckpoint = this.transform.GetChild(i);
            nextCheckpoint = this.transform.GetChild(next);
            prevCheckpoint = this.transform.GetChild(prev);

            //wallsize
            currCheckpoint.localScale = WallSize;

            //angles to surrounding checkpoints
            currCheckpoint.LookAt(nextCheckpoint);
            currRotation = new Quaternion(currCheckpoint.transform.rotation.x, currCheckpoint.transform.rotation.y, currCheckpoint.transform.rotation.z, currCheckpoint.transform.rotation.w);
            currCheckpoint.LookAt(prevCheckpoint);
            prevRotation = new Quaternion(prevCheckpoint.transform.rotation.x, prevCheckpoint.transform.rotation.y, prevCheckpoint.transform.rotation.z, prevCheckpoint.transform.rotation.w);

            //set checkpoint to smooth angle between surrounding checkpoints
            currCheckpoint.transform.rotation = Quaternion.Lerp(currRotation, prevRotation, 0.5f);
        }
    }
    private int idxNextCheckpoint(int i)
    {
        if (i < this.transform.childCount - 1)
            return i + 1;
        else
            return 0;
    }
    private int idxPrevCheckpoint(int i)
    {
        if (i == 0)
            return this.transform.childCount - 1;
        else
            return i - 1;
    }
    public string Description()
    {
        return string.Format("There are {0} checkpoints.", this.transform.childCount);
    }
}
