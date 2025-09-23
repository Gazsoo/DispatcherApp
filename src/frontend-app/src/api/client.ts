
import { AuthenticatedApiClient } from '../services/authenticated-api-client';

const createApiClient = () => {
  return new AuthenticatedApiClient();
};

export const apiClient = createApiClient();