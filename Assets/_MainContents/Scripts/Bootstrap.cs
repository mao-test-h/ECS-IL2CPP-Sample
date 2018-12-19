namespace MainContents
{
    using UnityEngine;

    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;
    using Unity.Rendering;

    using UnityRandom = UnityEngine.Random;

    public sealed class Bootstrap : MonoBehaviour
    {
#pragma warning disable 0649
        // ------------------------------
        #region // Private Members(Editable)

        [SerializeField] int _maxObjectNum = 10000;
        [SerializeField] float _boundSize = 128f;
        [SerializeField] MeshInstanceRenderer _meshInstanceRenderer;

        #endregion // Private Members(Editable)
#pragma warning restore 0649

        // ----------------------------------------------------
        #region // Unity Events

        /// <summary>
        /// MonoBehaviour.Start
        /// </summary>
        void Start()
        {
            // Create World & Systems.
            World.Active = new World("Sample World");
            var entityManager = World.Active.CreateManager<EntityManager>();
            // Built-in System
            World.Active.CreateManager(typeof(EndFrameTransformSystem));
            World.Active.CreateManager(typeof(RenderingSystemBootstrap));
            // Original System
            World.Active.CreateManager(typeof(RotationSytstem));
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.Active);

            // Create Archetype.
            var archetype = entityManager.CreateArchetype(
                ComponentType.Create<Prefab>(),
                ComponentType.Create<Position>(),
                ComponentType.Create<Rotation>(),
                ComponentType.Create<LocalToWorld>(),
                ComponentType.Create<MeshInstanceRenderer>());

            // Create Prefab Entities.
            var prefabEntity = entityManager.CreateEntity(archetype);
            entityManager.SetSharedComponentData(prefabEntity, this._meshInstanceRenderer);

            // Create Entities.
            for (int i = 0; i < _maxObjectNum; ++i)
            {
                var entity = entityManager.Instantiate(prefabEntity);
                entityManager.SetComponentData(entity, new Position { Value = this.GetRandomPosition() });
                entityManager.SetComponentData(entity, new Rotation { Value = UnityEngine.Random.rotation });
            }
        }

        /// <summary>
        /// MonoBehaviour.OnDestroy
        /// </summary>
        void OnDestroy()
        {
            World.DisposeAllWorlds();
        }

        #endregion // Unity Events

        // ----------------------------------------------------
        #region // Private Methods

        float3 GetRandomPosition()
        {
            float halfSize = this._boundSize / 2f;
            return new float3(
                UnityRandom.Range(-halfSize, halfSize),
                UnityRandom.Range(-halfSize, halfSize),
                UnityRandom.Range(-halfSize, halfSize));
        }

        #endregion // Private Methods
    }
}
