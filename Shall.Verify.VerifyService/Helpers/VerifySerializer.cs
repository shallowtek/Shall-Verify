using Confluent.Kafka;
using Shall.Verify.Common.Dtos.Verify;
using Newtonsoft.Json;
using System.Text;

namespace Shall.Verify.VerifyService.Helpers;

public class VerifySerializer : ISerializer<VerifyResponse>
{
    public byte[] Serialize(VerifyResponse data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }
}