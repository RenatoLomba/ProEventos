using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface ISpeakerPersist
    {
        Task<Speaker[]> GetSpeakersByNameAsync(string name, bool includeEvents = false);
        Task<Speaker[]> GetSpeakersAsync(bool includeEvents = false);
        Task<Speaker> GetSpeakerByIdAsync(int id, bool includeEvents = false);
    }
}
