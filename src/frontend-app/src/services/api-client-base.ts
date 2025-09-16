export abstract class ApiClientBase {
    protected baseUrl: string | undefined;
    
    protected getBaseUrl(url: string): string {
        const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7000';
        return baseUrl + url;
    }
}