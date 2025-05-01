using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface IManufacturerService
    {
        List<ManufacturerDto> GetAllManufacturers();
        void DeleteManufacturer(int id);
        int CreateManufacturer(ManufacturerDto dto);
        void UpdateManufacturer(ManufacturerDto dto);
    }
}
