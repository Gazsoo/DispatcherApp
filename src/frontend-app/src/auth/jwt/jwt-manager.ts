import { assertRequiredFields } from '../../utils/assertion';
import type { AuthResponse, RefreshRequest } from '../../services/web-api-client';
import { jwtDecode } from 'jwt-decode';

export class JWTManager {
  private static readonly ACCESS_TOKEN_KEY = 'accessToken';
  private static readonly REFRESH_TOKEN_KEY = 'refreshToken';
  private static readonly TOKEN_EXPIRY_KEY = 'tokenExpiry';

  static setTokens(authResponse: AuthResponse): void {
    assertRequiredFields(authResponse, ['accessToken', 'refreshToken']);

    const expiresInMs = (authResponse.expiresIn ?? 3600) * 1000;
    localStorage.setItem(this.ACCESS_TOKEN_KEY, authResponse.accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, authResponse.refreshToken);
    localStorage.setItem(this.TOKEN_EXPIRY_KEY, expiresInMs.toString());
  }

  static getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  static getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  static getRefreshRequest(): RefreshRequest | null {
    const accessToken = this.getAccessToken();
    const refreshToken = this.getRefreshToken();

    if (!accessToken || !refreshToken) {
      return null;
    }

    return {
      accessToken,
      refreshToken
    };
  }

  static clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.TOKEN_EXPIRY_KEY);
  }

  static isTokenExpired(): boolean {
    const expiryTime = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
    if (!expiryTime) return true;

    return Date.now() > parseInt(expiryTime);
  }

  static isAuthenticated(): boolean {
    const token = this.getAccessToken();
    return token !== null && !this.isTokenExpired();
  }

  static getAuthHeader(): string | null {
    const token = this.getAccessToken();
    return token ? `Bearer ${token}` : null;
  }

  static getRemainingTime(): number {
    const expiryTime = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
    if (!expiryTime) return 0;

    const remaining = parseInt(expiryTime) - Date.now();
    return Math.max(0, remaining);
  }
  static getRoles(): string[] {
    const token = this.getAccessToken();
    if (!token) return [];
    try {
      const payload: any = jwtDecode(token);
      const candidates = [
        'role',
        'roles',
        'Role',
        'Roles',
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role'
      ];
      for (const k of candidates) {
        const v = payload[k];
        if (!v) continue;
        return Array.isArray(v) ? v.map(String) : [String(v)];
      }
    } catch {
      return [];
    }
    return [];
  }
  static getPrimaryRole(): string | null {
    console.log('Getting primary role from JWT: ' + this.getRoles());
    const roles = this.getRoles();
    return roles.length ? roles[0] : null;
  }

  static hasRole(role: string): boolean {
    return this.getRoles().includes(role);
  }
}