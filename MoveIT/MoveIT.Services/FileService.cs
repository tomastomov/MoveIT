using MoveIT.Gateways.Contracts;
using MoveIT.Services.Contracts;

namespace MoveIT.Services
{
    public class FileService : IFileService
    {
        private readonly IMoveITGateway _gateway;

        public FileService(IMoveITGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task Upload(Func<Task<(byte[] File, string FileName)>> fileReader, int directoryId)
        {
            var fileInfo = await fileReader();

            await _gateway.UploadFileToDirectory(fileInfo.File, fileInfo.FileName, directoryId);

            return;
        }
    }
}
