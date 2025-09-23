import type { AxiosResponse } from "axios";

export abstract class ApiClientBase {
    protected baseUrl: string | undefined;
    
    protected getBaseUrl(url: string): string {
        const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7000';
        return baseUrl + url;
    }

    protected transformResult(
        url: string,
        response: AxiosResponse,
        processor: (response: AxiosResponse) => Promise<any>
    ): Promise<any> {
        // Fix the Axios auto-parsing issue
        if (typeof response.data === 'object' && response.data !== null) {
            response.data = JSON.stringify(response.data);
        }
        return processor(response);
    }
}