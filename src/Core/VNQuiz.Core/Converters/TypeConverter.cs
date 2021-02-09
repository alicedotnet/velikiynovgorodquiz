using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VNQuiz.Core.Converters
{
    public class TypeConverter : JsonConverter<Type>
    {
        public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? type = reader.GetString();
            if(type == null)
            {
                return null;
            }
            return Type.GetType(type) ?? throw new CoreException($"Can't find '{type}' type");
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
