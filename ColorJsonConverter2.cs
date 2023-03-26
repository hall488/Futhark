using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Futhark
{
    public class ColorJsonConverter2 : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ColorHelper.ToHex((Color)value));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color) || objectType == typeof(Dictionary<Color, string>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(objectType == typeof(Dictionary<Color, string>)) {
                
                Console.WriteLine(reader.Value);
                return new Dictionary<Color,string>();
            } else {
                var value = (string)reader.Value;
                return value.StartsWith("#") ? ColorHelper.FromHex(value) : ColorHelper.FromName(value);
            }
        }
    }
}