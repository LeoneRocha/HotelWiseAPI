using AutoMapper;
using HotelWise.Domain.Constants;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelWise.Service.Entity
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _entityRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenConfigurationDto _configurationToken;
         
        public UserService(IUserRepository entityRepository, IMapper mapper, ITokenService tokenService, ITokenConfigurationDto configurationToken)
        {
            _entityRepository = entityRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _configurationToken = configurationToken;
        }

        public async Task<ServiceResponse<GetUserAuthenticatedDto>> Login(string login, string password)
        {
            var response = new ServiceResponse<GetUserAuthenticatedDto>();

            var user = await _entityRepository.FindByLogin(login);
            if (user == null)
            {
                response.Success = false;
                response.Message = ValidatorConstants.Validade_UserNotFound;
                return response;
            }
            else if (!SecurityHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
                return response;
            }

            response.Data = await executeLoginJwt(user);

            response.Success = true;
            response.Message = "User Logged.";
            return response;
        }
        private async Task<GetUserAuthenticatedDto> executeLoginJwt(User user)
        {
            TokenVO token = await validateCredentials(user);
            GetUserAuthenticatedDto response = _mapper.Map<GetUserAuthenticatedDto>(user);
             
            response.TokenAuth = token;
            return response;
        }
        private async Task<TokenVO> validateCredentials(User user)
        {
            if (user == null) return new TokenVO();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            DateTime refreshTokenExpiryTime = DataHelper.GetDateTimeNow().AddDays(_configurationToken.DaysToExpiry);

            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _entityRepository.RefreshUserInfo(user);

            DateTime createDate = DataHelper.GetDateTimeNow();
            DateTime expirationDate = createDate.AddMinutes(_configurationToken.Minutes);

            var tokenResult = new TokenVO(true,
                createDate.ToString(AppConfigConstants.DATE_FORMAT2),
                expirationDate.ToString(AppConfigConstants.DATE_FORMAT2),
                accessToken,
                refreshToken
                );

            return tokenResult;
        }
    }
}