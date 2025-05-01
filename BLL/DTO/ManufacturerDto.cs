using DAL.Models;

namespace BLL.DTO
{
    public class ManufacturerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }

        public ManufacturerDto() { }

        public ManufacturerDto(Manufacturer manufacturer)
        {
            Id = manufacturer.Id;
            Name = manufacturer.Name;
            Description = manufacturer.Description;
            Country = manufacturer.Country;
        }
    }
}