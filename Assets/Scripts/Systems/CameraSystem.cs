using Components;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;


[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial class CameraSystem : SystemBase
{
    private Entity Target;
    private Random Random;
    private EntityQuery TanksQuery;

    protected override void OnCreate()
    {
        Random = Random.CreateFromIndex(1234);
        TanksQuery = GetEntityQuery(typeof(Tank));
        RequireAnyForUpdate(TanksQuery);
    }

    protected override void OnUpdate()
    {
        if (Target == Entity.Null || UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
        {
            var tanks = TanksQuery.ToEntityArray(Allocator.Temp);
            Target = tanks[Random.NextInt(tanks.Length)];
        }

        var cameraTransform = CameraSingleton.Instance.transform;
        var tankTransform = GetComponent<LocalToWorld>(Target);
        cameraTransform.position =
            tankTransform.Position - 10.0f * tankTransform.Forward + new float3(0.0f, 5.0f, 0.0f);
        cameraTransform.LookAt(tankTransform.Position, new float3(0.0f, 1.0f, 0.0f));
    }
}
