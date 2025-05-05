using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Game :GameWindow
{
    double DeltaTime = 0;
    public Vector3 PlayerPosition = new Vector3(0.0f, 0.0f, -5.0f);
    public const float PlayerSpeed = 0.5f;
    ObjLoader monkey = new ObjLoader();

    public Game(GameWindowSettings gameWindowSettings,NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings,nativeWindowSettings) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100f);
        GL.LoadMatrix(ref perspective);

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Lighting);
        GL.Enable(EnableCap.Light0);

        GL.ClearColor(Color4.Black);

        monkey.Load(@"E:\C#Sources\GraphicsEngine\GraphicsEngine\Models\monkey.obj"); //Your path to source file

    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.Translate(PlayerPosition);

        GL.MatrixMode(MatrixMode.Modelview);

        GL.Rotate(-(50 * DeltaTime), 0.0f, 1.0f, 0.0f);
        //GL.Translate(-5.0f, 5.0f, -7.0f);
        monkey.DrawModel(monkey);
        GL.LoadIdentity();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        DeltaTime += args.Time;

        if (KeyboardState.IsKeyPressed(Keys.Escape)) 
        {
            Close();
        }
        if (KeyboardState.IsKeyDown(Keys.S))
        {
            PlayerPosition.Z -= PlayerSpeed;
        }
        if (KeyboardState.IsKeyDown(Keys.W))
        {
            PlayerPosition.Z += PlayerSpeed;
        }
        if (KeyboardState.IsKeyDown(Keys.A))
        {
            PlayerPosition.X += PlayerSpeed;
        }
        if (KeyboardState.IsKeyDown(Keys.D))
        {
            PlayerPosition.X -= PlayerSpeed;
        }
        if (KeyboardState.IsKeyDown(Keys.Z))
        {
            PlayerPosition.Y += PlayerSpeed;
        }
        if (KeyboardState.IsKeyDown(Keys.X))
        {
            PlayerPosition.Y -= PlayerSpeed;
        }
    }

    private static void Main() 
    {
        NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
        {
            APIVersion = new Version(3, 3),
            API = ContextAPI.OpenGL,
            Flags = ContextFlags.Default,
            Size = (1024,720),
            Title = "Graphics Engine",
            WindowBorder = WindowBorder.Fixed,
            Profile = ContextProfile.Compatability,
            Vsync = VSyncMode.On
        };

        using (Game game = new Game(GameWindowSettings.Default,nativeWindowSettings)) 
        {
            game.Run();
        }
    }
}