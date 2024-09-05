# El Pato Stream

Servers and frontend for my stream alerts and other twitch interactions

This is built specifically for my channel but could be easily modified to work with other channels by replacing some of the hardcoded values

# Appsettings Configuration

The `ElPato.Stream.Backend/ElPato.Stream.Api` application requires a `appsettings.json` file as below:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Port": 8080,
  "ChannelId": "CHANNEL_ID",
  "Credentials": {
    "ClientId": "TWITCH_CLIENT_ID",
    "ClientSecret": "TWITCH_CLIENT_SECRET"
  }
}
```