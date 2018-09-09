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

        private Logger logger;

        private ShaderCompiler shaderCompiler;

        private int vertexArray;

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

            window.CursorVisible = true;





            shaderCompiler = new ShaderCompiler(shaders, logger);

        }

        private void GetShaders()
        {
            shaders.Add(new Shader(ShaderType.FragmentShader, @"Shaders\frag.shader", "SimpleFragmentShader"));
            shaders.Add(new Shader(ShaderType.VertexShader, @"Shaders\vert.shader", "SimpleVertexShader"));
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
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GL.DeleteProgram(shaderCompiler.Program);
        }

        private void Window_updateFrame(object sender, FrameEventArgs e)
        {

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

            window.Title = $"Overlord - (Vsync: {window.VSync}) FPS: {1f / e.Time:0}";

            GL.UseProgram(shaderCompiler.Program);
            GL.DrawArrays(PrimitiveType.Points, 0, 1);
            GL.PointSize(10);

            window.SwapBuffers();

        }

        public void Run() => window.Run();

    }
}
