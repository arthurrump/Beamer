# Beamer

A simple self-contained F# Windows Forms application that takes over your screen and displays some non-functioning beamer screens. To change the mode, press one of the following keys:

- Q: No Signal
- W: Loading
- E: Blank screen
- Esc: Close the application

## Building and running

Run the application from source: `dotnet run`

Build a single executable: `dotnet publish -r win-x86 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true`
