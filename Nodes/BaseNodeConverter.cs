using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sandbox;

namespace SandMix.Nodes;

public class BaseNodeConverter : JsonConverter<BaseNode>
{
	public override bool CanConvert( Type type )
	{
		return typeof( BaseNode ).IsAssignableFrom( type );
	}

	public override BaseNode Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options )
	{
		if ( reader.TokenType != JsonTokenType.StartObject )
		{
			throw new JsonException();
		}

		if ( !reader.Read()
				|| reader.TokenType != JsonTokenType.PropertyName
				|| reader.GetString() != "NodeType" )
		{
			throw new JsonException();
		}

		if ( !reader.Read() || reader.TokenType != JsonTokenType.String )
		{
			throw new JsonException();
		}

		BaseNode node;

		string nodeTypeName = reader.GetString();

		var nodeType = Library.GetType( nodeTypeName );
		if ( nodeType is null )
		{
			throw new NotSupportedException( nodeTypeName );
		}

		if ( !reader.Read() || reader.GetString() != "NodeData" )
		{
			throw new JsonException();
		}
		if ( !reader.Read() || reader.TokenType != JsonTokenType.StartObject )
		{
			throw new JsonException();
		}

		node = (BaseNode) JsonSerializer.Deserialize( ref reader, nodeType );

		if ( !reader.Read() || reader.TokenType != JsonTokenType.EndObject )
		{
			throw new JsonException();
		}

		return node;
	}

	public override void Write(
		Utf8JsonWriter writer,
		BaseNode value,
		JsonSerializerOptions options )
	{
		writer.WriteStartObject();

		writer.WriteString( "NodeType", value.GetType().Name );

		writer.WritePropertyName( "NodeData" );
		JsonSerializer.Serialize( writer, value, value.GetType() );

		writer.WriteEndObject();
	}
}
