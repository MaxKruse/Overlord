using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Overlord.Engine;
using Overlord.Utils;
using System;
using System.Collections.Generic;

namespace Overlord
{
    class Game
    {
        private GameWindow window;
        private long timer = 60;
        private Logger logger;

        private ShaderCompiler shaderCompiler;

        private Vector2[] vertBuffer;
        private int VBO;

        private List<Shader> shaders;

        public Game(GameWindow _window)
        {
            window = _window;
            window.VSync = VSyncMode.On;
            window.Title = "Overlord";
            shaders = new List<Shader>();

            RegisterCallbacks();
            LoggerStart();
            GetShaders();

            shaderCompiler = new ShaderCompiler(shaders, ref logger);

            StartShaders();

            window.CursorVisible = true;
        }

        private void StartShaders()
        {
            GL.UseProgram(shaderCompiler.Program);
        }

        private void GetShaders()
        {
            shaders.Add(new Shader(ShaderType.FragmentShader, @"Shaders\simple.frag", "SimpleFragmentShader"));
            shaders.Add(new Shader(ShaderType.VertexShader, @"Shaders\simple.vert", "SimpleVertexShader"));
        }

        private void LoggerStart()
        {
            logger = new Logger();

            var Renderer = GL.GetString(StringName.Renderer);
            var ShadingLanguageVersion = GL.GetString(StringName.ShadingLanguageVersion);
            var Vendor = GL.GetString(StringName.Vendor);
            var Version = GL.GetString(StringName.Version);

            logger.Log("OPEN_GL", "Renderer: " + Renderer);
            logger.Log("OPEN_GL", "ShadingLanguageVersion: " + ShadingLanguageVersion);
            logger.Log("OPEN_GL", "Vendor: " + Vendor);
            logger.Log("OPEN_GL", "Version: " + Version);
        }

        private void RegisterCallbacks()
        {
            window.Load += Window_load;
            window.Closing += Window_closing;
            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_updateFrame;
            window.KeyDown += Window_KeyDown;
            window.KeyUp += Window_KeyUp;

            window.MouseDown += Window_MouseDown;
            window.MouseUp += Window_MouseUp;

        }


        private void Window_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
#if DEBUG
            Console.WriteLine(e.Button + " released");
#endif
        }

        private void Window_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
#if DEBUG
            Console.WriteLine(e.Button + " clicked");
#endif
        }

        private void Window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
#if DEBUG
            Console.WriteLine("Key Up: " + e.Key);
#endif
        }

        private void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
#if DEBUG
            Console.WriteLine("Key Down: " + e.Key);
#endif
            switch (e.Key)
            {
                case OpenTK.Input.Key.Escape:
                    window.Close();
                    break;
            }
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_load(object sender, EventArgs e)
        {
            window.Closed += Window_Closed;

            vertBuffer = new Vector2[5]
            {
                new Vector2(0, 0.5f),
                new Vector2(-0.25f, 0),
                new Vector2(0.6f, 0.1f),
                new Vector2(-0.25f, -0.5f),
                new Vector2(0, 0.5f)
            };

            //Buffer
            VertexBuffer();
        }

        private void VertexBuffer()
        {
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(Vector2.SizeInBytes * vertBuffer.Length), vertBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GL.DeleteProgram(shaderCompiler.Program);
        }

        private void Window_updateFrame(object sender, FrameEventArgs e)
        {
            timer++;
            //Update Window Title every second
            if (timer % 60 == 0)
            {
                window.Title = $"Overlord - (Vsync: {window.VSync}) FPS: {1f / e.Time:0}";
                timer = 0;
            }
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.3f;
            GL.ClearColor(backColor);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(shaderCompiler.Program);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes, 0);

            GL.DrawArrays(PrimitiveType.LineStrip, 0, vertBuffer.Length);
            GL.PointSize(10);

            GL.Flush();
            window.SwapBuffers();

        }

        public void Run() => window.Run();

    }
}
