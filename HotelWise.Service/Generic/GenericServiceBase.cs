using AutoMapper;

namespace HotelWise.Service.Generic
{ 
    public abstract class GenericVectorStoreServiceBase  
    {
        protected readonly IMapper _mapper;
        protected readonly Serilog.ILogger _logger;
        protected long UserId { get; private set; }

        protected GenericVectorStoreServiceBase(IMapper mapper, Serilog.ILogger logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SetUserId(long id)
        {
            UserId = id;
        } 
    } 
}
