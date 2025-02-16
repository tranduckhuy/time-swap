export const environment = {
  production: true,
  apiAuthBaseUrl: process.env['API_AUTH_BASE_URL'] || '',
  apiBaseUrl: process.env['API_BASE_URL'] || '',
  authClientUrl: process.env['AUTH_CLIENT_URL'],
};
