using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Dto.Base;

namespace HotelWise.Domain.Dto
{
    public class GetUserAuthenticatedDto : EntityDtoBase
    {
        public GetUserAuthenticatedDto()
        {
            TokenAuth = new TokenVO();
        }
        public string Name { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public TokenVO? TokenAuth { get; set; } 

        public long? MedicalId { get; set; }
    }
}
