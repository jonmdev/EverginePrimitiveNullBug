using Evergine.Common.Graphics;
using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics.Effects;
using Evergine.Framework.Graphics.Materials;
using Evergine.Framework.Graphics;
using Evergine.Framework.Managers;
using Evergine.Framework.Services;
using Evergine.Framework.Threading;
using Evergine.Platform;
using System.Net.NetworkInformation;
using BulletSharp;

namespace EverginePrimitiveNullBug
{
    public partial class MyApplication : Application
    {
        public MyApplication()
        {
            this.Container.Register<Settings>();
            this.Container.Register<Clock>();
            this.Container.Register<TimerFactory>();
            this.Container.Register<Random>();
            this.Container.Register<ErrorHandler>();
            this.Container.Register<ScreenContextManager>();
            this.Container.Register<GraphicsPresenter>();
            this.Container.Register<AssetsDirectory>();
            this.Container.Register<AssetsService>();
            this.Container.Register<ForegroundTaskSchedulerService>();
            this.Container.Register<WorkActionScheduler>();
        }

        public override void Initialize()
        {
            base.Initialize();

            // Get ScreenContextManager
            var screenContextManager = this.Container.Resolve<ScreenContextManager>();
            var assetsService = this.Container.Resolve<AssetsService>();

            // Navigate to scene
            var scene = assetsService.Load<MyScene>(EvergineContent.Scenes.MyScene_wescene);
            ScreenContext screenContext = new ScreenContext(scene);
            screenContextManager.To(screenContext);

            //===================
            //CUSTOM CODE
            //===================

            // Load a Render Layer description...
            RenderLayerDescription layerDescription = assetsService.Load<RenderLayerDescription>(EvergineContent.RenderLayers.Opaque);

            // Make material
            StandardMaterial materialDecorator = new StandardMaterial(assetsService.Load<Effect>(EvergineContent.Effects.StandardEffect));
            materialDecorator.BaseColor = Color.PaleVioletRed;
            materialDecorator.Reflectance = 0.5f;
            materialDecorator.LightingEnabled = true;
            materialDecorator.Material.LayerDescription = layerDescription;

            // Apply to an entity
            Entity primitive = new Entity()
                .AddComponent(new Transform3D())
                .AddComponent(new MaterialComponent() { Material = materialDecorator.Material }) //====THIS LINE BREAKS IT IN ANDROID====//
                .AddComponent(new CubeMesh())
                .AddComponent(new MeshRenderer());

            scene.Managers.EntityManager.Add(primitive);

        }
    }
}


