using System;

namespace Reqres.APITests.APIs.ReqresAPIService.Response
{
    public partial class ReqresApipostCreateUserResponseProperties
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
