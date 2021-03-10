# Sirtatji

This Discord bot will watch a channel for arrivals and departures. Whenever someone joins or leaves the channel, it will join it then announce it using some stupid pre-recorded voices.

It's also about me testing [Discord.NET](https://github.com/discord-net/Discord.Net) and writing a proof-of-concept for sending actual voices using it.

## Voice sending

Sending voice is pretty simple if you [follow the documentation](https://docs.stillu.cc/guides/voice/sending-voice.html). However, it has a native dependency on the `libsodium`, `ffpmeg` and `opus` libraries. They can be a pain in the ass to install, especially if you're on a Windows machine. For this reason I have provided a `Dockerfile` which installs all the required dependencies on an Ubuntu container. 

For more advanced stuff, it may be interesting to have a look at [Lavalink](https://github.com/Frederikam/Lavalink), a library to send Audio which is used by popular bots like Rythm.

## Getting started

Edit `appsettings.json` with your bot's token. Then, use the provided `Dockerfile` to run the bot.

Use the `!watch` command on any channel to make the bot watch for your specific channel for user departures and arrivals.
