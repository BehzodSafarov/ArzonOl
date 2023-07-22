using ArzonOL.Dtos.ProductVoterDtos;
using ArzonOL.Models;

namespace ArzonOL.Services.VoterService.Interfaces;


public interface IVoterService
{
    ValueTask<ProductVoterModel> CreateAsync(ProductVoterModel productVoterModel);
    ValueTask<ProductVoterModel> UpdateForVoteAsync(ProductVoterModel productVoterModel);
    ValueTask<ProductVoterModel> UpdateForCommentAsync(ProductVoterModel productVoterModel);
    ValueTask<ProductVoterModel> DeleteCommentAsync(ProductVoterModel productVoterModel);
    ValueTask<ProductVoterModel> DeleteVoteAsync(ProductVoterModel productVoterModel);
    ValueTask<ProductVoterModel> MupDtoToModel(VoterDto dto);
}