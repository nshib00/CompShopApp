using BLL.DTO;
using DAL.Context;
using DAL.Entities;

namespace ComputerShop.Models
{
    public class ManufacturerModel
    {
        private readonly AppDbContext _context = new AppDbContext();

        public List<ManufacturerDto> GetAllManufacturers()
        {
            return _context.Manufacturers
                .Select(m => new ManufacturerDto(m))
                .ToList();
        }

        public void DeleteManufacturer(int id)
        {
            var manufacturer = _context.Manufacturers.Find(id);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
                _context.SaveChanges();
            }
        }

        public int CreateManufacturer(ManufacturerDto dto)
        {
            var manufacturer = new Manufacturer
            {
                Name = dto.Name,
                Description = dto.Description,
                Country = dto.Country,
                Products = [],
            };

            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            return manufacturer.Id;
        }

        public void UpdateManufacturer(ManufacturerDto dto)
        {
            var manufacturer = _context.Manufacturers.Find(dto.Id);
            if (manufacturer != null)
            {
                manufacturer.Name = dto.Name;
                manufacturer.Description = dto.Description;
                manufacturer.Country = dto.Country;

                _context.SaveChanges();
            }
        }
    }
}
