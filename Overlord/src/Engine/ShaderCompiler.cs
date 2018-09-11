using OpenTK.Graphics.OpenGL;
using Overlord.Utils;
using System.Collections.Generic;
using System.IO;

namespace Overlord.Engine
{
    class ShaderCompiler
    {
        public int Program { get; set; }
        private List<int> compiledShaders;

        public ShaderCompiler(List<Shader> shaders, ref Logger logger)
        {
            int shaderID;
            string shaderPath;
            string shaderSrc;
            string info;

            compiledShaders = new List<int>();

            foreach (Shader shader in shaders)
            {
                //check for shaders
                shaderPath = Path.Combine(Directory.GetCurrentDirectory(), shader.Path);
                if (!File.Exists(shaderPath))
                {
                    logger.Log("SHADER_ERROR", "ShaderPath invalid.");
                    logger.Log("SHADER_ERROR", shader.Type + " in " + shader.Path);
                }
                else
                {
                    shaderID = GL.CreateShader(shader.Type);
                    shaderSrc = File.ReadAllText(shaderPath);

                    GL.ShaderSource(shaderID, shaderSrc);
                    GL.CompileShader(shaderID);
                    info = GL.GetShaderInfoLog(shaderID);
                    if (!string.IsNullOrWhiteSpace(info))
                        logger.Log("SHADER_ERROR", "Compiling " + shader.Type + " had an error:\r\n" + info);
                    compiledShaders.Add(shaderID);
                    logger.Log("SHADER", "Compiled " + shader.Name);
                }
            }


            Program = GL.CreateProgram();
            logger.Log("SHADER", "Created Program");

            foreach (int compiledID in compiledShaders)
            {
                GL.AttachShader(Program, compiledID);
                logger.Log("SHADER_PROGRAM", "Attached shader ID " + compiledID);
            }
            GL.LinkProgram(Program);

            logger.Log("SHADER", "Linked Program");


            foreach (int compiledID in compiledShaders)
            {
                GL.DetachShader(Program, compiledID);
                GL.DeleteShader(compiledID);
            }

            logger.Log("SHADER", "Cleanup of shaders");

        }


    }
}
