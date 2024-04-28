using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO  {ID=1,Name="Pool View",Sqft=100,Occupancy=2},
                new VillaDTO {ID=2,Name="Beach View",Sqft=200,Occupancy=4}
            };
    }
}
