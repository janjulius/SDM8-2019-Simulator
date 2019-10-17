using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Something that is subscribed to a mqtt thing
/// </summary>
public class SdmSub : MonoBehaviour
{
    private string teamId;
    public LaneType laneType;
    public Direction direction;
    public string groupId;
    public string subgroupId;
    public ComponentType componentType;
    public string componentId;

    private void Start()
    {
        teamId = SdmManager.Instance.connectedGroup;
    }

    public override string ToString()
    {
        return $"{teamId}/{laneType.ToString().ToLower()}/{direction.ToString().ToLower()}/{groupId}/{subgroupId}/{componentType.ToString().ToLower()}/{componentId}";
    }
}
