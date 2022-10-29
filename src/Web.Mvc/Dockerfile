#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /source
COPY ["src/Web.Mvc/Web.Mvc.csproj", "src/Web.Mvc/"]
COPY ["src/Web.Framework/Web.Framework.csproj", "src/Web.Framework/"]
COPY ["src/Core.Application/Core.Application.csproj", "src/Core.Application/"]
COPY ["src/Core.Application.Contracts/Core.Application.Contracts.csproj", "src/Core.Application.Contracts/"]
COPY ["src/Core.Domain.Identity/Core.Domain.Identity.csproj", "src/Core.Domain.Identity/"]
COPY ["src/Core.Domain.Persistence/Core.Domain.Persistence.csproj", "src/Core.Domain.Persistence/"]
COPY ["src/Infrastructure.Identity/Infrastructure.Identity.csproj", "src/Infrastructure.Identity/"]
COPY ["src/Infrastructure.Persistence/Infrastructure.Persistence.csproj", "src/Infrastructure.Persistence/"]
RUN dotnet restore "src/Web.Mvc/Web.Mvc.csproj"
COPY . ./

RUN dotnet publish "./src/Web.Mvc/Web.Mvc.csproj" -c Release -o app


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


FROM base AS final
COPY --from=build-env /source/app .
ENTRYPOINT ["dotnet", "Web.Mvc.dll"]
