namespace System.Graphics
{
    public interface IPictureReader
    {
        Picture Read(byte[] data);
    }
}