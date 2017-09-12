# Oscilloscope
SW that allows to control DSO (Digital Storage Oscilloscope) via LAN interface. Implementation and testing is done using Rigol DS1054Z oscilloscope that has SW version 00.04.04.SP3.

## LAN Interface
At the moment LAN interface is the only one supported. When connecting to Rigol oscilloscope the IP address and PORT number are required. Rigol oscilloscopes use **PORT number 5555** and the IP address what the router gives to the oscilloscope. The network connection status (press `Utility > IO Settings > LAN Conf.`) shows the current IP address of the instrument (see screenshot below).

It is recommended to configure static IP address to oscilloscope so that the IP address remains the same every time oscilloscope is powered on.

<p align="center">
  <img alt="Lan Setting" src="https://user-images.githubusercontent.com/25169598/30288342-13710bdc-9731-11e7-9d48-6c28670dd6d1.png">
</p>

## Build Instructions

Clone the source repository from Github. On the command line, enter:

```
git clone https://github.com/tparviainen/oscilloscope.git
```

Open a terminal/console/command prompt, change to the directory where you cloned oscilloscope, and type:

```
dotnet restore
dotnet build
```

Now everything is built once, type next command to run scope app:

```
dotnet run --project .\Scope\Scope.csproj
```

Alternatively go to project folder and then execute `dotnet run`:

```
cd Scope
dotnet run
```

That outputs the usage of the application and example commands to get started.

## Usage
This project has a command line application (`scope`) that can be used to send SCPI commands to instrument. When the application is executed without arguments it prints next help:

```
Scope v1.0

Usage: scope [optional] <required>

Required arguments:
 -i <interface> <arg>
 -c <command> [<arg> ...]

Optional arguments:
 -o <output-filename>
 -p <plugin-folder> [<plugin-folder> ...]
 -s (list of supported commands)

Examples:
 scope -i LAN 192.168.1.160:5555 -c IDN
 scope -i LAN 192.168.1.160:5555 -c RAW *IDN?
 scope -i LAN 192.168.1.160:5555 -c RAW *IDN? -o received.raw
 scope -s
```

That gives an idea how to use `scope` app and what arguments are required. When using dotnet command line to run the application the command `scope` must be replaced with the command `dotnet run` i.e.:

```
Examples:
 dotnet run -i LAN 192.168.1.160:5555 -c IDN
 dotnet run -i LAN 192.168.1.160:5555 -c RAW *IDN?
 dotnet run -i LAN 192.168.1.160:5555 -c RAW *IDN? -o received.raw
 dotnet run -s
```
