using Business.Factories;
using Business.Handlers;
using Data.Repositories;
using Domain.DTOs;
using Domain.Models;

namespace Business.Services;

public interface IMemberService
{
    Task<MemberModel?> CreateMemberAsync(AddMemberForm formData);
    Task<bool> DeleteMemberAsync(Guid id);
    Task<IEnumerable<MemberModel>> GetAllMembersAsync();
    Task<MemberModel?> GetMemberByIdAsync(Guid id);
    Task<MemberModel?> UpdateMemberAsync(EditMemberForm formData);
}

public class MemberService(IMemberRepository memberRepository, ICacheHandler<IEnumerable<MemberModel>> cacheHandler) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly ICacheHandler<IEnumerable<MemberModel>> _cacheHandler = cacheHandler;
    private const string _cacheKey = "Members";

    public async Task<MemberModel?> CreateMemberAsync(AddMemberForm formData)
    {
        var memberExists = await _memberRepository.ExistsAsync(x => x.Email == formData.Email);
        if (memberExists) return null;

        var entity = MemberFactory.ToEntity(formData);
        await _memberRepository.AddAsync(entity);

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == entity.Id);
    }

    public async Task<IEnumerable<MemberModel>> GetAllMembersAsync()
        => _cacheHandler.GetFromCache(_cacheKey) ?? await UpdateCacheAsync();

    public async Task<MemberModel?> GetMemberByIdAsync(Guid id)
    {
        var entity = _cacheHandler.GetFromCache(_cacheKey)?.FirstOrDefault(x => x.Id == id);
        if (entity is not null) return entity;

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == id);
    }

    public async Task<MemberModel?> UpdateMemberAsync(EditMemberForm formData)
    {
        var entity = await _memberRepository.GetAsync(x => x.Id == formData.Id);
        if (entity is null) return null;

        MemberFactory.UpdateEntity(entity, formData);
        await _memberRepository.UpdateAsync(entity);

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == entity.Id);
    }

    public async Task<bool> DeleteMemberAsync(Guid id)
    {
        var entity = await _memberRepository.DeleteAsync(x => x.Id == id);
        if (entity) await UpdateCacheAsync();
        return entity;
    }

    private async Task<IEnumerable<MemberModel>> UpdateCacheAsync()
    {
        var entities = await _memberRepository.GetAllAsync();
        var models = entities.Select(MemberFactory.ToModel).ToList();

        _cacheHandler.SetCache(_cacheKey, models);
        return models;
    }
}
