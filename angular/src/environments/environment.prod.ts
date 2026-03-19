import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44377/',
  redirectUri: baseUrl,
  clientId: 'Weigh_App',
  responseType: 'code',
  scope: 'offline_access Weigh',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Weigh',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44377',
      rootNamespace: 'Further.Weigh',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
