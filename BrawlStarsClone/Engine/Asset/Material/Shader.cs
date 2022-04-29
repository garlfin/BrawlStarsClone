using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset.Material
{
    public class Shader : Asset
    {
        private readonly int _id;

        public Shader(string path, ShaderType type)
        {
            _id = GL.CreateShader(type);
            GL.ShaderSource(_id, File.ReadAllText(path));
            GL.CompileShader(_id);

            string log = GL.GetShaderInfoLog(_id);
            if (!string.IsNullOrEmpty(log)) Console.WriteLine(log);
        }

        public void Attach(ShaderProgram program)
        {
            GL.AttachShader(program.Get(), _id);
        }

        public override void Delete()
        {
            GL.DeleteShader(_id);
        }

        public int Get()
        {
            return _id;
        }
    }
}