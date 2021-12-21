using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Superintendent_bot
{
    public class UploadModule : ModuleBase<SocketCommandContext>
    {
		[Command("upload")]
		[Summary("Uploads the given attachments")]
		public Task UploadAsync()
        {

			var channelID = Context.Channel.Id.ToString();
			ConfigModel currentConfig = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText("config/config.json"));

			if (!currentConfig.channels.Contains(channelID))
			{
				Console.WriteLine($"Files submitted in {channelID} were ignored.");
				return Task.CompletedTask;
			}

			foreach (Discord.Attachment attachment in Context.Message.Attachments)
            {
				var pathSplit = attachment.Filename.Split(".");
				var extension = pathSplit[pathSplit.Length - 1];
				var downloadPath = "";
				if (currentConfig.supportedExtensions.TryGetValue(extension, out downloadPath))
                {
					using (var client = new WebClient())
					{
						try
						{
							client.DownloadFile(attachment.Url, downloadPath + attachment.Filename);
						}
						catch (Exception e)
						{
							ReplyAsync("**WARNING**: Configured path didn't exist. Creating path");
							Directory.CreateDirectory(downloadPath);
							client.DownloadFile(attachment.Url, downloadPath + attachment.Filename);

						}
						Console.WriteLine($"file uploaded by: {Context.User.Username}, {attachment.Filename}.");
						ReplyAsync($"**Success**. {attachment.Filename} has been uploaded.");
					}
				} 
				else
                {
					Console.WriteLine($"Ignored file uploaded by: {Context.User.Username}, File extention isn't configured.");
					ReplyAsync($"**WARNING**: Ignoring file: {attachment.Filename}, these files aren't accepted. See system staff if you believe this is an error.");
                }
				
			}
			return Task.CompletedTask;
		}

	}
}
