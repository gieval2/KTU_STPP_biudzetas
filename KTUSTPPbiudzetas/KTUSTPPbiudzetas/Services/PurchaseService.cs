using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }
        public async Task<Purchase> CreateAsync(Purchase newData)
        {
            if (newData == null) throw new ArgumentNullException(nameof(newData));

            await _purchaseRepository.AddAsync(newData);

            return newData;
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _purchaseRepository.GetByIdAsync(id);
            if (data == null)
            {
                return;
            }

            await _purchaseRepository.RemoveAsync(id);
        }

        public async Task<IEnumerable<Purchase>> GetAllAsync()
        {
            var dataList = await _purchaseRepository.GetAllAsync();
            return dataList;
        }

        public async Task<Purchase> GetAsync(int id)
        {
            var data = await _purchaseRepository.GetByIdAsync(id);
            return data;
        }

        public async Task<IEnumerable<Purchase>> GetByCheckIdAsync(int id)
        {
            var data = await _purchaseRepository.GetByCheckIdAsync(id);
            return data;
        }

        public async Task<Purchase> UpdateAsync(Purchase updateData)
        {
            if (updateData == null) throw new ArgumentNullException(nameof(Member));

            var oldData = await _purchaseRepository.GetByIdAsync(updateData.Id);
            if (oldData == null) throw new InvalidOperationException($"Event with the id: {updateData.Id} was not found");

            await _purchaseRepository.UpdateAsync(oldData);
            return oldData;
        }
    }
}
