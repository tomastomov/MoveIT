namespace MoveIT.Services.Contracts
{
    public interface IFileService
    {
        Task Upload(Func<Task<byte[]>> fileReader, int directoryId);
    }
}
