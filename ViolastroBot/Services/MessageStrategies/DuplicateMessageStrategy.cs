﻿using Discord;
using Discord.WebSocket;
using ViolastroBot.Services.MessageStrategies;

public sealed class DuplicateMessageStrategy : IMessageStrategy
{
    private const int Limit = 4;  // Fetch 4 previous messages to compare with the current message

    public async Task<bool> ExecuteAsync(SocketUserMessage message)
    {
        if (message.Channel is not SocketTextChannel channel)
        {
            return false;
        }
        
        // Fetch the last 4 messages (excluding the current message)
        List<IMessage> messages = (await channel.GetMessagesAsync(message, Direction.Before, Limit)
                .FlattenAsync())
            .ToList();

        // Include the current message for comparison
        messages.Add(message);

        // Check if we have 5 messages in total
        if (messages.Count != 5)
        {
            return false;
        }
        
        bool areAllMessagesSame = messages.All(m => m.Content == message.Content);

        if (!areAllMessagesSame)
        {
            return false;
        }
        
        await channel.SendMessageAsync("Y'all best stop spammin'!");
        
        return true;
    }
}
