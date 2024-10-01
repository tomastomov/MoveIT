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

        public async Task Upload(Func<Task<byte[]>> fileReader, int directoryId)
        {
            var file = await fileReader();
            await _gateway.UploadFileToDirectory(file, "test", directoryId);
            return;
        }
    }
}
