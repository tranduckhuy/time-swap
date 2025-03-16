export const environment = {
  production: true,
  apiAuthBaseUrl: process.env['API_AUTH_BASE_URL'] || '',
  apiBaseUrl: process.env['API_BASE_URL'] || '',
  apiGptUrl: process.env['API_GPT_URL'] || '',
  authClientUrl: process.env['AUTH_CLIENT_URL'] || '',
  chatGptKey: process.env['CHAT_GPT_KEY'] || '',
  chatGptOrgKey: process.env['CHAT_GPT_ORG_KEY'] || '',
};
