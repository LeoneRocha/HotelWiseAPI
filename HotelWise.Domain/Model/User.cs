namespace HotelWise.Domain.Model
{
    public class User : EntityBaseWithNameEmail
    {
        public User()
        { 
        }
        #region Columns 
        public string Login { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];
        public string Role { get; set; } = string.Empty;
        public bool Admin { get; set; }
        public string Language { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        #endregion Columns 

        #region Relationship  

        //public UserTokenSession? TokenSession { get; set; }

        #endregion Relationship
    }
}
