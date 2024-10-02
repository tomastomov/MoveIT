using MoveIT.Common.Helpers;

namespace MoveIT.Services.Contracts
{
    public interface IFileService
    {
        Task<Result> Upload(Func<Task<(byte[] File, string FileName)>> fileReader, int directoryId);
    }
}
