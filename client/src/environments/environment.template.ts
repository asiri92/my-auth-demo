export const environment = {
  production: false,
  msalConfig: {
    clientId: 'YOUR_SPA_CLIENT_ID',
    authority: 'https://login.microsoftonline.com/YOUR_TENANT_ID',
    redirectUri: 'http://localhost:4200',
    postLogoutRedirectUri: 'http://localhost:4200'
  },
  apiConfig: {
    endpoint: 'https://localhost:7052/WeatherForecast',
    scope: 'api://YOUR_API_CLIENT_ID/access_as_user'
  }
};