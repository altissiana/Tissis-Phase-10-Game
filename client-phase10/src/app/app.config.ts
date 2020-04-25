export default {
    oidc: {
      clientId: '0oa57sjliOjRkwaOq4x6',
      issuer: 'https://dev-545127.okta.com/oauth2/default',
      redirectUri: 'http://localhost:4200/implicit/callback',
      scopes: ['openid', 'profile', 'email']
    },
    resourceServer: {
      messagesUrl: 'http://localhost:8000/api/messages',
    },
  };