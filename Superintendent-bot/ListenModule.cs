using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Superintendent_bot
{
    public class ListenModule : ModuleBase<SocketCommandContext>
    {
        [Command("listen")]
        [Summary("Adds the channel to the list of channels")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task AddListen()
        {
            var channelID = Context.Channel.Id.ToString();
            ConfigModel currentConfig = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText("config/config.json"));

            if (currentConfig.channels.Contains(channelID))
            {
                Console.WriteLine($"{Context.User.Username} tried to configure channel {Context.Channel.Id} but it's already configured.");
                return ReplyAsync("Already listening to this channel.");
            }

            else
            {
                currentConfig.channels.Add(Context.Channel.Id.ToString());
                File.WriteAllText("config/config.json", JsonConvert.SerializeObject(currentConfig).ToString());
                Console.WriteLine($"{Context.User.Username} added channel {Context.Channel.Id} to the list of approved channels.");
                return ReplyAsync("Now listening to this channel.");
            }
        }

        [Command("ignore")]
        [Summary("removes the channel to the list of channels")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task StopListen()
        {
            var channelID = Context.Channel.Id.ToString();
            ConfigModel currentConfig = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText("config/config.json"));

            if (currentConfig.channels.Contains(channelID))
            {
                currentConfig.channels.Remove(channelID);
                File.WriteAllText("config/config.json", JsonConvert.SerializeObject(currentConfig).ToString());
                Console.WriteLine($"{Context.User.Username} removed channel {Context.Channel.Id} to the list of approved channels.");
                return ReplyAsync("No longer listening to this channel.");
            }

            else
            {
                Console.WriteLine($"{Context.User.Username} tried to remove channel {Context.Channel.Id} but it's not configured.");
                return ReplyAsync("**ERROR**: I've not been listening to this channel.");
            }
        }

    }
}
