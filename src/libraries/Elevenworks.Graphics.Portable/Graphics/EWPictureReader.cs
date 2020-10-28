namespace Elevenworks.Graphics
{
    public interface EWPictureReader
    {
        EWPicture Read(byte[] data, string hash = null);
    }
}