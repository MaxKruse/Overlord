using OpenTK.Graphics.OpenGL;

namespace Overlord.Engine
{
    class Shader
    {
        public ShaderType Type { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }

        public Shader(ShaderType _type, string _path, string _name)
        {
            Type = _type;
            Path = _path;
            Name = _name;
        }

    }
}
