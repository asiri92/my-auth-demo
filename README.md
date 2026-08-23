# OAuth2 / OIDC Demo — Angular + ASP.NET Core + Microsoft Entra ID

A hands-on reference implementation of Authorization Code + PKCE, RBAC, and
resource-based authorization using Microsoft Entra ID.

## Architecture

- **`client/`** — Angular SPA (MSAL Angular v3), public client, Auth Code + PKCE
- **`api/`** — ASP.NET Core Web API (.NET 9), resource server validating Entra-issued JWTs

The SPA authenticates users against Entra, acquires an access token, and calls
the API. The API validates the token's signature, issuer, audience, and scope,
then enforces role-based and resource-based authorization on top.

## Prerequisites

- .NET 9 SDK
- Node.js + Angular CLI 17
- An Azure subscription with permission to register applications in Entra ID
  (or a free [Microsoft 365 Developer tenant](https://developer.microsoft.com/microsoft-365/dev-program))

## Entra ID setup (required before running)

You need **two app registrations**:

1. **API app registration**
   - Expose an API → add scope `access_as_user`
   - App roles → add `Reader`, `Writer`, `Admin` (member type: Users/Groups)
2. **SPA app registration**
   - Platform: Single-page application
   - Redirect URI: `http://localhost:4200`
   - API permissions → delegated → your API's `access_as_user` scope → grant admin consent

Assign yourself an app role under **Enterprise Applications → [API app] → Users and groups**.

## Configuration

Real config values are intentionally excluded from this repo. You must supply your own.

### API (`api/`)

```bash
cd api
dotnet user-secrets init
dotnet user-secrets set "AzureAd:TenantId" "<your-tenant-id>"
dotnet user-secrets set "AzureAd:ClientId" "<your-api-client-id>"
dotnet user-secrets set "AzureAd:Scopes" "access_as_user"
```

### Client (`client/`)

```bash
cd client
cp src/environments/environment.template.ts src/environments/environment.ts
cp src/environments/environment.template.ts src/environments/environment.development.ts
```

Edit both new files with your values:

```typescript
msalConfig: {
  clientId: '<your-spa-client-id>',
  authority: 'https://login.microsoftonline.com/<your-tenant-id>',
  redirectUri: 'http://localhost:4200',
  postLogoutRedirectUri: 'http://localhost:4200'
},
apiConfig: {
  endpoint: 'https://localhost:7052/WeatherForecast',
  scope: 'api://<your-api-client-id>/access_as_user'
}
```

## Running locally

```bash
# Terminal 1 — API
cd api
dotnet run

# Terminal 2 — Client
cd client
npm install
ng serve
```

Browse to `http://localhost:4200`. You'll be redirected to Entra sign-in, then
back to the app with a working session.

## What this demonstrates

- Auth Code + PKCE flow (SPA as public client)
- Automatic token acquisition/attachment via `MsalInterceptor`
- API-side validation: signature, issuer, audience, scope
- Role-based authorization (`[Authorize(Policy = "...")]` over the `roles` claim)
- Resource-based authorization (`IAuthorizationHandler`) — per-record ownership checks
- Deny-by-default fallback policy
- Client-side role checks are cosmetic only — the API is the actual enforcement boundary

## Project structure

```
my-auth-demo/
├── client/     Angular SPA
└── api/        ASP.NET Core Web API
```
