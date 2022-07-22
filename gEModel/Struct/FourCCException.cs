namespace gEModel.Struct;

public class FourCCException : Exception
{
    public FourCCException(string message) : base(message)
    {
    }

    public FourCCException(string expected, string got) : base($"Expected FourCC {expected}, got {got}")
    {
        
    }
}
