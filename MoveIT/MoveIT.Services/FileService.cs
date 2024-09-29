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

        public async Task Upload(string path)
        {
        }
    }
}
