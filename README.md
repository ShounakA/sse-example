# SSE - Example

This repo was created as an example to show off server-side events (SSE) with C# and .NET.

## To Run

```shell
    cd SSE-Example
    dotnet run
```

OR if you don't want to install dotnet SDK, containerized version available:

```shell
docker build -t sse-example .
docker run -d -p 8443:8443 sse-example

```

## To Try out SSE

```shell
    curl http://localhost:8443/api/sse
```

OR you can navigate to `http://localhost:8443/api/sse` via your favourite browser. If you want to retry the request, hit refresh!