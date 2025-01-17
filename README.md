# FantasyBasketball App
This app is intended to display information about a user's ESPN Fantasy Basketball team that isn't available on the ESPN Fantasy App or website.

## Description
App's backend logic is coded in C#, including the call to ESPN's Fantasy API to extract information about a League's fantasy team. The GUI was build on Avalonia's framework. Using this app, users are able to see information about their team not available on ESPN's Fantasy App or website such as how many and which players are occupying each position on a Fantasy team.

## Getting Started
These instructions are for building and running the project on Ubuntu. You need to have [Avalonia](https://docs.avaloniaui.net/docs/get-started/install) and [.NET9.0](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?pivots=os-linux-ubuntu-2404&tabs=dotnet9) installed before building and running this project. 

Cloning:
```
git clone https://github.com/eawong1/FantasyBasketball.git
```
Building and Running Project:
```
cd FantasyBasketball/FantasyBasketball
dotnet build
dotnet run
```

## Acknowledgments
Referenced cwendt94's EPSN API to make the proper API call: https://github.com/cwendt94/espn-api.
