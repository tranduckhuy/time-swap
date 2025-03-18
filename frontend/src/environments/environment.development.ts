export const environment = {
  production: false,
  apiAuthBaseUrl: 'https://localhost:9001/api',
  apiBaseUrl: 'https://localhost:9002/api',
  apiGptUrl: process.env['API_GPT_URL'] || '',
  authClientUrl: 'http://localhost:4200/auth',
  chatGptKey: process.env['CHAT_GPT_KEY'] || '',
  chatGptOrgKey: process.env['CHAT_GPT_ORG_KEY'] || '',
};
