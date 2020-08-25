using CarMD.Fleet.Common.Enum;

namespace CarMD.Fleet.Data.Response
{
    public class ServiceResult<T>
    {
        public ResultCode ResultCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
