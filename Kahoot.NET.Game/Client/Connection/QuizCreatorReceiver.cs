﻿using System.Text;
using Kahoot.NET.Game.Internal.Data.Messages;
using Kahoot.NET.Internal;
using Kahoot.NET.Internal.Data.SourceGenerators.Messages;
using Kahoot.NET.Internal.Data.SourceGenerators.Responses;

namespace Kahoot.NET.Game.Client;

public partial class QuizCreator
{
    internal async Task ReceiveAsync()
    {
        while (Socket.State == WebSocketState.Open)
        {
            Memory<byte> buffer = new byte[1024];

            var result = await Socket.ReceiveAsync(buffer, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }

            await ProcessAsync(buffer);
        }
    }

    private static string RemoveBrackets(ReadOnlySpan<char> span)
    {
        int start = 1;
        int end = span.LastIndexOf(']');

        return span.Slice(start, end - 1).ToString();
    }

    private async Task ProcessAsync(Memory<byte> data)
    {
        string json = RemoveBrackets(Encoding.UTF8.GetString(data.Span));

        Logger?.LogDebug("Received: {json}", json);

        var message = JsonSerializer.Deserialize(json.AsSpan(), LiveBaseMessageContext.Default.LiveBaseMessage);

        int id = message!.Id is null ? -1 : int.Parse(message!.Id.AsSpan());


        switch ((id, message.Channel))
        {
            case (1, LiveMessageChannels.Handshake):
                var obj = JsonSerializer.Deserialize(json.AsSpan()!, LiveClientHandshakeResponseContext.Default.LiveClientHandshakeResponse);

                if (obj is null)
                {
                    throw new InvalidOperationException("An internal problem occured whilst parsing the websocket data");
                }

                _sessionObject.clientId = obj.ClientId;

                await SendAsync(CreateSecondHandshake());

                break;
            case (2, LiveMessageChannels.Connection):
                Interlocked.Increment(ref _sessionObject.id);
                Interlocked.Increment(ref _sessionObject.ack);

                await SendAsync(CreateStartGameMessage(), StartGameMessageContext.Default.StartGameMessage);
                await Task.Delay(500);
                Interlocked.Increment(ref _sessionObject.id);
                await SendAsync(CreateKeepAlive());

                break;
            case (4, LiveMessageChannels.Connection):
                Interlocked.Increment(ref _sessionObject.id);
                Interlocked.Increment(ref _sessionObject.ack);
                await SendAsync(CreateKeepAlive());

                if (QuizCreated is not null)
                {
                    await QuizCreated.Invoke(this, EventArgs.Empty);
                }

                break;
        }
    }
}
