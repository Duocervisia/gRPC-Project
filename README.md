# gRPC Project

This Project contains a Phyton gRPC Server, a NodeJS Client and a C# Client. 

# Setup Python

1. Install Python and pip https://www.python.org/downloads/
2. In "python server" Folder run this to install requirements: py -m pip install -r requirements.txt
3. Run Server with: python server.py
4. Enjoy

# Setup C# Client

1. Install C# https://visualstudio.microsoft.com/de/vs/community/
2. Get the ClientLibrary and the c# Client.
3. Make sure all the packages (Google.Protobuf, Grpc.Net.Client, Grpc.Tools) are really included.
4. Run ClientLibrary one time.
5. Check that ClientLibrary is referenced in c# Client (would be visible in Waterlevel.csproj)
6. Run C# Client
7. For fun: The method "GetLastDayToUnix()" in the Class "RequestHandler" sets the beginning timestamp. e.g. Uncomment line 72 oder change line 73 to see different time spans.

# Setup Node.js Client

1. Install Node https://nodejs.org/en/
2. Run: npm install
3. Run client with: node client.js
4. See under: localhost:8000 the website
5. Enjoy
