namespace MoveIT.Services.Contracts
{
    public interface IFileService
    {
        Task Upload(Func<Task<(byte[] File, string FileName)>> fileReader, int directoryId);
    }
}
