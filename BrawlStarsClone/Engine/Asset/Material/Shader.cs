using BrawlStarsClone.Engine.Windowing;
using Silk.NET.OpenGL;

namespace BrawlStarsClone.Engine.Asset.Material
{
    public class Shader : Asset
    {
        private readonly uint _id;

        public Shader(GameWindow gameWindow, string path, ShaderType type) : base(gameWindow)
        {
            _id = gameWindow.gl.CreateShader(type);
            gameWindow.gl.ShaderSource(_id, File.ReadAllText(path));
            gameWindow.gl.CompileShader(_id);

            string log = gameWindow.gl.GetShaderInfoLog(_id);
            if (!string.IsNullOrEmpty(log)) Console.WriteLine(log);
        }

        public void Attach(ShaderProgram program)
        {
            GameWindow.gl.AttachShader(program.Get(), _id);
        }

        public override void Delete()
        {
            GameWindow.gl.DeleteShader(_id);
        }

        public uint Get()
        {
            return _id;
        }
    }
}