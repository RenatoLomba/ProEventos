using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Implementations
{
    public class BatchService : IBatchService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly IBatchPersist _batchPersist;
        private readonly IMapper _mapper;
        public BatchService(
            IGeneralPersist generalPersist,
            IBatchPersist batchPersist,
            IMapper mapper
        )
        {
            this._batchPersist = batchPersist;
            this._generalPersist = generalPersist;
            this._mapper = mapper;
        }

        public async Task<BatchDto[]> SaveBatch(int eventId, BatchDto[] models)
        {
            try
            {
                var batches = await this._batchPersist.GetBatchesByEventIdAsync(
                    eventId
                );

                foreach (var model in models)
                {
                    model.EventId = eventId;

                    if (model.Id == 0)
                    {
                        var batch = this._mapper.Map<Batch>(model);
                        this._generalPersist.Add<Batch>(batch);
                    }
                    else
                    {
                        var batch = batches
                            .FirstOrDefault(bt => bt.Id.Equals(model.Id));
                        if (batch == null)
                            throw new Exception("Trying to update an unexisting batch");

                        this._mapper.Map(model, batch);
                        this._generalPersist.Update<Batch>(batch);
                    }

                    await this._generalPersist.SaveChangesAsync();
                }

                var batchesReturn = await this._batchPersist.GetBatchesByEventIdAsync(
                    eventId
                );
                return this._mapper.Map<BatchDto[]>(batchesReturn);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteBatch(int eventId, int batchId)
        {
            try
            {
                var batch = await this._batchPersist
                    .GetBatchByIdAsync(eventId, batchId);
                if (batch == null) throw new Exception("Batch Not Found");

                this._generalPersist.Delete(batch);
                return await this._generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BatchDto[]> GetBatchesByEventId(int eventId)
        {
            try
            {
                var batches = await this._batchPersist.GetBatchesByEventIdAsync(
                    eventId
                );
                return this._mapper.Map<BatchDto[]>(batches);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BatchDto> GetBatchById(int eventId, int batchId)
        {
            try
            {
                var batch = await this._batchPersist
                    .GetBatchByIdAsync(eventId, batchId);
                return this._mapper.Map<BatchDto>(batch);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
