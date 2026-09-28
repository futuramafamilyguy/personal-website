using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(FondhoodJsonConverter))]
public enum Fondhood
{
    Unfonded,
    Wellfonded,
    Illfonded
}

public class FondhoodJsonConverter : JsonConverter<Fondhood>
{
    public override Fondhood Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var fondhood = reader.GetString();
        return fondhood switch
        {
            "unfonded" => Fondhood.Unfonded,
            "wellfonded" => Fondhood.Wellfonded,
            "illfonded" => Fondhood.Illfonded,
            _ => throw new JsonException($"invalid fondhood value: {fondhood}")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        Fondhood value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            Fondhood.Unfonded => "unfonded",
            Fondhood.Wellfonded => "wellfonded",
            Fondhood.Illfonded => "illfonded",
            _ => throw new JsonException($"invalid fondhood value: {value}")
        });
    }
}