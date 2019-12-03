using KTU_STPP_biudzetas.Models;
using KTU_STPP_biudzetas.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Services
{
    public class CheckService : ICheckService
    {
        private readonly ICheckRepository _checkRepository;

        public CheckService(ICheckRepository checkRepository)
        {
            _checkRepository = checkRepository;
        }
        public async Task<Check> CreateAsync(Check newData)
        {
            if (newData == null) throw new ArgumentNullException(nameof(newData));

            await _checkRepository.AddAsync(newData);

            return newData;
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _checkRepository.GetByIdAsync(id);
            if (data == null)
            {
                return;
            }

            await _checkRepository.RemoveAsync(id);
        }

        public async Task<IEnumerable<Check>> GetAllAsync()
        {
            var dataList = await _checkRepository.GetAllAsync();
            return dataList;
        }

        public async Task<Check> GetAsync(int id)
        {
            var data = await _checkRepository.GetByIdAsync(id);
            return data;
        }

        public async Task<Check> UpdateAsync(Check updateData)
        {
            if (updateData == null) throw new ArgumentNullException(nameof(Member));

            var oldData = await _checkRepository.GetByIdAsync(updateData.Id);
            if (oldData == null) throw new InvalidOperationException($"Event with the id: {updateData.Id} was not found");

            await _checkRepository.UpdateAsync(oldData);
            return oldData;
        }
    }
}
