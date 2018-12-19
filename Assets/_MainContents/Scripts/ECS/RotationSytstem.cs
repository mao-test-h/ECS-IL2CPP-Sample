namespace MainContents
{
    using UnityEngine;

    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;
    using Unity.Jobs;
    using Unity.Burst;

    /// <summary>
    /// 回転するだけのシステム
    /// </summary>
    public sealed class RotationSytstem : JobComponentSystem
    {
        [BurstCompile]
        public struct RotationJob : IJobProcessComponentData<Rotation>
        {
            float _deltaTime;
            public RotationJob(float deltaTime) => this._deltaTime = deltaTime;
            public void Execute(ref Rotation rot)
            {
                rot.Value = math.mul(
                    math.normalize(rot.Value),
                    quaternion.AxisAngle(math.up(), 2f * this._deltaTime));
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps) => new RotationJob(Time.deltaTime).Schedule(this, inputDeps);
    }
}
