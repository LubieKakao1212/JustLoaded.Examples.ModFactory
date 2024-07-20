using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Custom2d_Engine.Input;
using Custom2d_Engine.Rendering;
using Custom2d_Engine.Rendering.PostProcess;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Rendering.Sprites.Atlas;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Events;
using Custom2d_Engine.Ticking;
using Custom2d_Engine.Tilemap;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Content.Database.Extensions;
using JustLoaded.Core;
using JustLoaded.Core.Discovery;
using JustLoaded.Filesystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory;
using Template.Factory.Content;

namespace Template
{
    public class ModFactoryGame : Game
    {
        private const string ModsDir = "mods";
        private int ScreenWidth = 800;
        private int ScreenHeight = 800;
        
        [NotNull] public static RenderPipeline? RenderPipeline { get; private set; }
        private SpriteAtlas<Color> SpriteAtlas;
        private Hierarchy MainHierarchy;
        private TickManager TickManager;
        private Camera Camera;
        private World PhysicsWorld;
        private InputManager InputManager;
        private ModLoaderSystem ModLoader;
        private GameTime GameTime;

        private BuildingPlacementHandler BuildingPlacementHandler;
        
        public ModFactoryGame()
        {
            var _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() {
            InitSystems();
            
            LoadMods();
            
            InitPhysics();
            InitHierarchy();
            InitKeyBinds();
        }

        private void InitSystems() 
        {
            Effects.Init(Content);
            Effects.Default.CurrentTechnique = Effects.Default.Techniques["Lit"];
            Effects.TilemapDefault.CurrentTechnique = Effects.TilemapDefault.Techniques["Lit"];
            
            RenderPipeline = new RenderPipeline();
            RenderPipeline.Init(GraphicsDevice, ScreenWidth, ScreenHeight);
            RenderPipeline.PostProcessing.Add(new SinglePassPostProcess(Effects.CorrectHdr));
            
            TickManager = new TickManager();
            InputManager = new InputManager(Window);
            SpriteAtlas = new SpriteAtlas<Color>(GraphicsDevice, 2048, 2);

            if (!Directory.Exists(ModsDir)) {
                Directory.CreateDirectory(ModsDir);
            }

            var masterDb = new MasterDatabase();
            masterDb.SetGlobal();
            ModLoader = new ModLoaderSystem.Builder(
                new AssemblyModProvider(
                    new FilesystemAssemblyProvider(
                        new PhysicalFilesystem("mods")
                        )
                    )
                    .WithMods(
                        new Mod(new ModMetadata.Builder(FactoryReg.ModId).Build())
                            .AddInitializer(new ModFactoryGameModInitializer(Content, SpriteAtlas, RenderPipeline))
                            .AddAssembly(Assembly.GetExecutingAssembly())
                        ).Verbose()
                ) { MasterDb = masterDb }.Build();
        }

        private void LoadMods() {
            try {
                Console.WriteLine("Beginning mod discovery...");
                ModLoader.DiscoverMods();
                Console.WriteLine("Resolving mod dependencies...");
                ModLoader.ResolveDependencies();
                Console.WriteLine("Executing entrypoints...");
                ModLoader.InitMods();
                Console.WriteLine("Loading...");
                ModLoader.Load();
            }
            catch (Exception e) {
                Console.WriteLine(ModLoader.CurrentInitPhase);
                Console.WriteLine(e);
            }
            Console.WriteLine(ModLoader.CurrentInitPhase);
        }

        private void InitPhysics() {
            PhysicsWorld = new World(Vector2.Zero);

            //Do init here
        }

        private void InitHierarchy()
        {
            MainHierarchy = new Hierarchy(TickManager);
            Camera = new Camera {
                ViewSize = 10f,
                AspectRatio = 1f
            };
            
            MainHierarchy.AddObject(Camera);
            
            var spawner = new RandomItemSpawner(3f, PhysicsWorld, Items.Keys.RawIron, Items.Keys.DarkIron);
            MainHierarchy.AddObject(spawner);

            var belt = new BeltObject(PhysicsWorld, Vector2.UnitX, Color.White) {
                Transform = {
                    LocalPosition = new Vector2(2f, 0.5f),
                    LocalScale = new Vector2(2f, 0.5f)
                },
                Controller = {
                    TargetLinearSpeed = 2f,
                    TargetAlignmentSpeed = 5f
                }
            };
            
            var dir = Vector2.UnitY;
            dir.Normalize();
            var belt2 = new BeltObject(PhysicsWorld, dir, Color.Bisque) {
                Transform = {
                    LocalPosition = new Vector2(2f, 0.5f),
                    LocalScale = new Vector2(1f, 1f)
                },
                Controller = {
                    TargetLinearSpeed = 2f,
                    TargetAlignmentSpeed = 10f,
                    MaxForce = 100f,
                    Filter = BeltObject.ItemFilter(true, (item) => item == Items.RawIron)
                }
            };
            
            var belt3 = new BeltObject(PhysicsWorld, dir, Color.White) {
                Transform = {
                    LocalPosition = new Vector2(2.5f, 3f),
                    LocalScale = new Vector2(0.5f, 2f)
                },
                Controller = {
                    TargetLinearSpeed = 2f,
                    TargetAlignmentSpeed = 5f
                }
            };
            MainHierarchy.AddObject(belt3);
            MainHierarchy.AddObject(belt);
            MainHierarchy.AddObject(belt2);
            
            var grid = new Grid(Vector2.One);
            MainHierarchy.AddObject(grid);

            /*var blueprint = new BuildingBlueprintObject(PhysicsWorld);
            blueprint.SetBuilding(Buildings.Assembler);
            MainHierarchy.AddObject(blueprint);*/

            /*blueprint.AddSimpleRepeatingAction(() => 
                blueprint.Transform.LocalPosition = grid.WorldToCell(
                    Camera.ViewToWorldPos(MousePosView())
                    ).ToVector2(), 
                0.001f);*/

            BuildingPlacementHandler = new BuildingPlacementHandler(PhysicsWorld, grid);
            BuildingPlacementHandler.Spawn(MainHierarchy);

            var gridMap = new Tilemap<RenderPipeline.InstanceSpriteData>();
            var gridMapRenderer = new TilemapRenderer(gridMap, grid, RenderPipeline, Color.White, -1f) {
                Parent = grid,
            };

            var spriteRef = new DatabaseReference<Sprite>(Sprites.Grid);
            for (int x = -1024; x < 1024; x++) {
                for (int y = -1024; y < 1024; y++) {
                    gridMap.SetTile(new Point(x, y), new RenderPipeline.InstanceSpriteData() {
                        atlasPos = spriteRef.Value!.AtlasPos,
                        color = Color.White.ToVector4()
                    });
                }
            }

            Buildings.BasicLight.Place(MainHierarchy, new Point(0, 5), PhysicsWorld, BuildingPlacement.Up);
            Buildings.WhiteLight.Place(MainHierarchy, new Point(0, -5), PhysicsWorld, BuildingPlacement.Up);
        }

        private void InitKeyBinds() {
            var inputHorizontal = InputManager.CreateSimpleAxisBinding("horizontal", Keys.A, Keys.D);
            var inputVertical = InputManager.CreateSimpleAxisBinding("vertical", Keys.S, Keys.W);
            
            InputManager.RegisterBinding(inputHorizontal);
            InputManager.RegisterBinding(inputVertical);
            
            inputHorizontal.Performed += (input) => Camera.Transform.GlobalPosition += Camera.Transform.Right * (CamMoveSpeed(GameTime) * input.GetCurrentValue<float>()/*.LogThis("Horizongtal: ")*/);
            inputVertical.Performed += (input) => Camera.Transform.GlobalPosition += Camera.Transform.Up * (CamMoveSpeed(GameTime) * input.GetCurrentValue<float>()/*.LogThis("Vertical: ")*/);
            
            BuildingPlacementHandler.BindInput(InputManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            this.GameTime = gameTime;
            
            InputManager.UpdateState();
            
            BuildingPlacementHandler.Update(Camera.ViewToWorldPos(MousePosView()));
            
            MainHierarchy.BeginUpdate();
            foreach (var upd in MainHierarchy.OrderedInstancesOf<IUpdatable>())
            {
                upd.Update(gameTime);
            }
            MainHierarchy.EndUpdate();

            TickManager.Forward(gameTime.ElapsedGameTime);
            PhysicsWorld.Step(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderPipeline.RenderScene(MainHierarchy, Camera, Color.Black);

            base.Draw(gameTime);
        }
        
        private Vector2 MousePosView()
        {
            var screenPos = InputManager.CursorPosition.GetCurrentValue<Point>();

            return new Vector2((screenPos.X / (float)Window.ClientBounds.Width) * 2f - 1f, -((screenPos.Y / (float)Window.ClientBounds.Height) * 2f - 1f));
        }
        
        private float CamMoveSpeed(GameTime gameTime) {
            var cameraSpeed = 10f;
            var boost = InputManager.GetKey(Keys.LeftShift).GetCurrentValue<bool>();
            return (float)gameTime.ElapsedGameTime.TotalSeconds * cameraSpeed * (boost ? 2f : 1f);
        }
    }
}
