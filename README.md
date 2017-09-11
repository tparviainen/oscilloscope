# Oscilloscope
SW that allows to control DSO (Digital Storage Oscilloscope) via LAN interface. Implementation and testing is done using Rigol DS1054Z oscilloscope that has SW version 00.04.04.SP3.

# LAN Interface
At the moment LAN interface is the only one supported. When connecting to Rigol oscilloscope the IP address and PORT number are required. Rigol oscilloscopes use **PORT number 5555** and the IP address what the router gives to the oscilloscope. The network connection status (press `Utility > IO Settings > LAN Conf.`) shows the current IP address of the instrument (see screenshot below).

It is recommended to configure static IP address to oscilloscope so that the IP address remains the same every time oscilloscope is powered on.

![lansetting](https://user-images.githubusercontent.com/25169598/30288342-13710bdc-9731-11e7-9d48-6c28670dd6d1.png)

# Usage
This project has a command line application (`scope`) that can be used to send SCPI commands to instrument. When the application is executed without arguments it prints next help:

```
Scope v1.0

Usage: scope [optional] <required>

Required arguments:
 -i <interface> <args>
 -c <command> <args>

Optional arguments:
 -o <output-filename>
 -p <plugin-folder> <plugin-folder> ...
 -s (list of supported commands)

Examples:
 scope -i LAN 192.168.1.160:5555 -c IDN
 scope -i LAN 192.168.1.160:5555 -c RAW *IDN?
 scope -i LAN 192.168.1.160:5555 -c RAW *IDN? -o received.raw
 scope -s
```

That gives an idea how to use `scope` app and what arguments are required.
