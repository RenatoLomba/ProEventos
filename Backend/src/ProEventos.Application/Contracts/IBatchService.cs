using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface IBatchService
    {
        Task<BatchDto[]> SaveBatch(int eventId, BatchDto[] models);
        Task<bool> DeleteBatch(int eventId, int batchId);

        Task<BatchDto[]> GetBatchesByEventId(int eventId);
        Task<BatchDto> GetBatchById(int eventId, int batchId);
    }
}
