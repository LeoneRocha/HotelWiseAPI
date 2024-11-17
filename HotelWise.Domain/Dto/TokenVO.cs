namespace HotelWise.Domain.Dto
{
    public class TokenVO
    {
        public TokenVO()
        {
        }
        public TokenVO(bool authenticated, string created, string expiration, string accessToken, string refreshToken)
        {
            Authenticated = authenticated;
            Created = created;
            Expiration = expiration;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public bool Authenticated { get; private set; }
        public string Created { get; private set; } = string.Empty;
        public string Expiration { get; private set; } = string.Empty;
        public string AccessToken { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;
    }
}
