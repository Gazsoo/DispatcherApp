import { apiClient } from '../api/client';
import { JWTManager } from '../auth/jwt/jwt-manager';
import type { LoginRequest, RegisterRequest } from '../services/web-api-client';

export class AuthService {
  static async login(email: string, password: string): Promise<void> {
    const request: LoginRequest = { email, password };
    
    // The generated client's auth_Login returns void, but we need the response
    // You might need to check if cookies are used or modify this
    const authRespone = await apiClient.auth_Login(request);
    JWTManager.setTokens(authRespone);
    // If your API uses cookies, you might not need to store tokens manually
    // If it returns tokens in response body, you'll need to modify the generated code
  }

  static async register(email: string, password: string, firstName?: string, lastName?: string): Promise<void> {
    const request: RegisterRequest = { email, password, firstName, lastName };
    await apiClient.auth_Register(request);
  }

  static async getUserInfo() {
    return await apiClient.auth_GetUserInfo();
  }

  static logout(): void {
    JWTManager.clearTokens();
  }
}