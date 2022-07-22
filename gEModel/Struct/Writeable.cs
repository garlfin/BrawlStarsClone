namespace gEModel.Struct;

public interface IWriteable
{
    public void Write(BinaryWriter writer);
    public void Read(BinaryReader reader);
}

public interface ITranslatable<out T> where T : IWriteable
{
    public T Translate();
}