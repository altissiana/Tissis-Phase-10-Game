export const environment = {
  production: true,
  apiUrl: 'https://tissisphase10api.azurewebsites.net',
  oidc: {
    clientId: '0oa57sjliOjRkwaOq4x6',
    issuer: 'https://dev-545127.okta.com/oauth2/default',
    redirectUri: 'https://tissisphase10.azurewebsites.net/implicit/callback',
    scopes: ['openid', 'profile', 'email'],
    baseUrl: 'https://dev-545127.okta.com'
  },
};
