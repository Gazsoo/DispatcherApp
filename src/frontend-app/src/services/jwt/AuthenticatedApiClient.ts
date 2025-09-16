import { JWTManager } from './jwt-manager';
import { Client } from '../web-api-client'

export class AuthenticatedApiClient extends Client {
    private isRefreshing = false;
    private failedQueue: Array<{resolve: Function, reject: Function}> = [];

    constructor(baseUrl?: string) {
        super(baseUrl);
        
        this.instance.interceptors.request.use((config) => {
            const authHeader = JWTManager.getAuthHeader();
            if (authHeader) {
                config.headers.Authorization = authHeader;
            }
            return config;
        });
        
        this.instance.interceptors.response.use(
            (response) => response,
            async (error) => {
                if (error.response?.status === 401) {
                    return this.handleUnauthorized(error);
                }
                return Promise.reject(error as Error);
            }
        );
    }

    private async handleUnauthorized(error: any) {
        const originalRequest = error.config;
        
        if (this.isRefreshing) {
            return new Promise((resolve, reject) => {
                this.failedQueue.push({ resolve, reject });
            }).then(() => {
                const authHeader = JWTManager.getAuthHeader();
                if (authHeader) {
                    originalRequest.headers.Authorization = authHeader;
                }
                return this.instance.request(originalRequest);
            });
        }

        this.isRefreshing = true;
        
        try {
            const refreshRequest = JWTManager.getRefreshRequest();
            if (!refreshRequest) {
                throw new Error('No refresh token available');
            }
            
            const refreshResponse = await this.auth_Refresh(refreshRequest);
            JWTManager.setTokens(refreshResponse);
            
            // Process queued requests
            this.failedQueue.forEach(({ resolve }) => resolve());
            this.failedQueue = [];
            
            // Retry
            const authHeader = JWTManager.getAuthHeader();
            if (authHeader) {
                originalRequest.headers.Authorization = authHeader;
            }
            return this.instance.request(originalRequest);
            
        } catch (refreshError) {
            console.error('Token refresh failed:', refreshError);
            JWTManager.clearTokens();
            
            // failed queue
            this.failedQueue.forEach(({ reject }) => reject(refreshError));
            this.failedQueue = [];
            
            window.dispatchEvent(new CustomEvent('auth:logout', { detail: refreshError }));
            
            return Promise.reject(refreshError as Error);
        } finally {
            this.isRefreshing = false;
        }
    }


}