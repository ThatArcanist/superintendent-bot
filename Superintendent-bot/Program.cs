using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Superintendent_bot
{
	public class Program
	{
		private DiscordSocketClient _client;
		private CommandHandler _commandHandler;
		private CommandService _commandService;

		public static Task Main(string[] args) => new Program().MainAsync();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_commandService = new CommandService();
			_commandHandler = new CommandHandler(_client, _commandService);

			await _commandHandler.InstallCommandsAsync();

			_client.Log += Log;

			var token = "";

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            try
            {
				token = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText("config/config.json")).token;
			}
			catch (FileNotFoundException e)
            {
				Console.WriteLine("Unable to read config file.");
				return;
            }

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}
		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
