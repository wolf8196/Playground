start "Server" cmd.exe /c dotnet run --project "SignalRSample.Server\SignalRSample.Server.csproj" --no-build
start "Client 1" cmd.exe /c dotnet run --project "SignalRSample.Client\SignalRSample.Client.csproj" --no-build
start "Client 2" cmd.exe /c dotnet run --project "SignalRSample.Client\SignalRSample.Client.csproj" --no-build