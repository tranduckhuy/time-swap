import Dotenv from 'dotenv-webpack';
import { DefinePlugin } from 'webpack'; 
import { resolve } from 'path';

export const plugins = [
  new DefinePlugin({
    API_AUTH_BASE_URL: JSON.stringify(process.env?.['API_AUTH_BASE_URL']),
    API_BASE_URL: JSON.stringify(process.env?.['API_BASE_URL'])
  }),
  new Dotenv({
    path: resolve(__dirname, '.env'),
    systemvars: true,
    safe: true,
    allowEmptyValues: true
  })
];