using FoodShareNET.Domain.Entities;
//using FoodShareNetAPI.DTO.Beneficiary;


namespace FoodShareNet.Application.Interfaces
{
    public interface IBeneficiaryService
    {
        Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary);
        Task<Beneficiary> GetBeneficiaryAsync(int? id);
        Task<IList<Beneficiary>> GetAllBeneficiariesAsync();

        Task<bool> EditBeneficiaryAsync(int beneficiaryId, Beneficiary beneficiary);
        
        Task<bool> DeleteBeneficiaryAsync(int beneficiaryId);
    }
}
