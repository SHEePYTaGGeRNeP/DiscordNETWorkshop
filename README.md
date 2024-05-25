# DiscordNETWorkshop
Discord.NET workshop for Kennisweekend

# Get started
All steps are also described here with images: https://docs.discordnet.dev/guides/getting_started/first-bot.html

1) Install the Discord.Net package in your solution (done already in project)
1) Create a bot on: https://discord.com/developers/applications/
    1) Click "New Application"
    1) Fill in the details and click save
    1) Go to the "Bot" tab on the left and Click "Add Bot" if needed
    1) Click Reset Token if you didn't get a token yet and copy and store it somewhere (for this project in the user secrets, by right clicking the project => "Manage User Secrets")
    1) Go to the "Privileged Gateway Intents" and check the last one "MESSAGE CONTENT INTENT", so our bot can read server messages.
    
1) Add your bot to a server
    1) Go to "OAuth2" tab
    1) Scroll down to OAuth2 URL Generator and under Scopes tick bot.
    1) Scroll down further to Bot Permissions and select the permissions that you wish to assign your bot with. (Probably "Send Messages" )
    1) Open the generated authorization URL in your browser.
    1) Select a server.
    1) Click on Authorize.

1) Connecting your bot to Discord
    1) Add the following code: 
    ```
    var client = new DiscordSocketClient();
    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();
    // Block this task until the program is closed.
    await Task.Delay(-1);
    ```
    1) The token is the token from the Discord Bot.
    1) Start your app and you should see the bot come online in the Discord server.

1) Making your bot do stuf