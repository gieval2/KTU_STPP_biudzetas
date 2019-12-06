using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Member> CreateAsync(Member newData)
        {
            if (newData == null) throw new ArgumentNullException(nameof(newData));

            await _memberRepository.AddAsync(newData);

            return newData;
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _memberRepository.GetByIdAsync(id);
            if (data == null)
            {
                return;
            }

            await _memberRepository.RemoveAsync(id);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            var dataList = await _memberRepository.GetAllAsync();
            return dataList;
        }

        public async Task<Member> GetAsync(int id)
        {
            var data = await _memberRepository.GetByIdAsync(id);
            return data;
        }

        public async Task<IEnumerable<Member>> ResetLimits()
        {
            var members = await _memberRepository.ResetLimits();
            return members;
        }

        public async Task<Member> SetLimit(int memberId, double newLimit)
        {
            var data = await _memberRepository.SetLimit(memberId, newLimit);
            return data;
        }

        public async Task<Member> ConfirmLimit(int memberId)
        {
            var data = await _memberRepository.ConfirmLimit(memberId);
            return data;
        }

        public async Task<Member> UpdateAsync(Member updateData)
        {
            if (updateData == null) throw new ArgumentNullException(nameof(Member));

            var oldData = await _memberRepository.GetByIdAsync(updateData.Id);
            if (oldData == null) throw new InvalidOperationException($"Event with the id: {updateData.Id} was not found");

            await _memberRepository.UpdateAsync(oldData);
            return oldData;
        }
    }
}
