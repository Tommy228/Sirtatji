# Sirtatji

This Discord bot will watch a channel for arrivals and departures.
Whenever someone joins or leaves the channel, it will join it then
announce it using some stupid pre-recorded voices.

More importantly it's me testing 
[https://github.com/discord-net/Discord.Net](Discord.NET) and writing a 
proof-of-concept  for sending voices using it.

## Getting started

Edit `appsettings.json` with your bot's token. Then, use the provided 
`Dockerfile` to run the bot.

Use the `!watch` command on any channel to make the bot watch for
your specific channel for user departures and arrivals.