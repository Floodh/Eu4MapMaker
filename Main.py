#py Main.py

import discord
from discord.ext import commands

#   own stuff
from Wrapper import run_mapmaker, set_tags

#   get token
token = open("token.txt", 'r').readline()
if (token == ""):
    print("Warning: could not find a token from token.txt file, exiting")
    exit(1)
print("token: " + token)

#   intents - why did discord even add these?
intents = discord.Intents.all()

# commands:
prefix = "-"
botclient = commands.Bot(command_prefix=prefix, intents=intents)
botclient.login(token)


@botclient.event
async def on_ready():
    print("Bot is active")

#   register tag
@botclient.event
async def on_message(message):
    
    print(message.content)

    if (message.channel.name == "reservation"):

        print("New:")

        array_str = [] 
        async for message in message.channel.history(limit=200):
            
            print("     [" + message.content + "] ")
            array_str.append(message.content)

        set_tags(array_str) # edits the text file that holds all the game tags
    else:
        
        #   -map
        if (message.content == prefix + "map"):

            print("generating map...")
            await message.channel.send("generating map...")
            run_mapmaker()
            await message.channel.send(file=discord.File('output/reduced.png'))  

        #   -ping
        if (message.content == prefix + "ping"):
            print("pong")
            await message.channel.send("pong")    


if __name__ == "__main__":

    botclient.run(token) #   token be here



































botclient.run('NjY1NTMyNzM5MTU5ODUxMDE5.XhnJhQ.Uuwsl8F6FlBENM2jkuooXQBG0go')