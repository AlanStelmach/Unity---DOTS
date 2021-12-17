using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class LifetimeSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {

        float deltaTime = Time.DeltaTime;
        var ecb = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        
        Entities.ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => 
        {
            lifetime.Value -= deltaTime;

            if(lifetime.Value <= 0f)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }
        }).ScheduleParallel();

        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
