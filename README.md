# Battleship Game

This application implements the [Battleship game](https://en.wikipedia.org/wiki/Battleship_(game)). I tried to implement the code with a simple architecture as described below

## Architectural Design

Almost all components of the game is modelled into a class to be as object oriented as possible. Some of the components/objects include

* **Player** which represents a player in the game
* **BattleBoard** which represent a player's first board where the ships are going to seat
* **MapBoard** which represents a player's map board where they record the moves made by their opponent
* **Coordinate** which represents the x/y coordinate on the board (labeled A-Z on the rows and 1-10 on the column)
* **ShipPosition** which holds information about a ship's current position
* **Game** which the game itself

### Game Play

To begin a game, an instance of a `Game` must be created which is dependent on a `Player` instance. An player attacks their enemy by calling the `Attack` method of the game and passing in an enemy player and a coordinate to attack. Whoever sinks all of their opponent's ships first wins the game! The method `GetWinner` returns the current winner of the game or `null` if the game is still in play.

> Note: If a player is instantiated without passing in ships, it will randomly create and place ships in random positions on the **BattleBoard**

## Running the Game

I've created the game as an console application to play the game between two players. It accepts input from the user and interacts with the rest of the application. Check the `Program.cs` file for more details. Note that I've excluded this file from testing and concentrated more on writing unit test as it makes more sense for a console application. The game itself is contained in the a folder `BattleshipGame` which is contained in the solution

To run the game outside a container, follow the steps below;

* Install the latest (probably still in preview at the time of writing this) version of .net 6
* Install dotnet CLI if you haven't already done so
* Restore the application dependencies
* Go to the root of the application and run the command `dotnet run --project BattleshipGame`

## Testing

The unit tests are contained in a folder `BattleshipGame.Tests`. To run unit tests (including test coverage report) run the shell script `run-tests.script.sh` with the command `bash run-tests.script.sh` or simply run the command;

* WITH test coverage report - `dotnet test BattleshipGame.Tests/BattleshipGame.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov;`
* WITHOUT test coverage report - `dotnet test BattleshipGame.Tests/BattleshipGame.Tests.csproj`
  
## Docker

I've added a `Dockerfile` to the application to allow the application to be "dockerized". Follow the steps below to build a docker image of the application

* Build the application into a docker image with the command `docker build -t ofuochi/battleship -f Dockerfile .`
* Run the application in interactive mode with the command `docker run -it --rm ofuochi/battleship` (the `--rm` option stops the container when the console application is exited with `Ctrl + C`). Note that you must run the command in ***interactive mode*** (with the flag `-it`) to be able to interact with the console application.

I've pushed the finished application to [docker hub](https://hub.docker.com/r/ofuochi/battleship). To run the application, simply pull the application from docker hub with the command `docker pull ofuochi/battleship` and follow the 2nd steps above.

> Note: The application is build with the latest (preview) ASP.NET core framework (.net 6), hence if you're not running it in a docker container, you will have to install this version of .net to run the application without issue.

## Improvements

Currently, I haven't used the dependency injection approach in solving this problem but that will be a good improvement to the application to further reduce coupling of components/entities
