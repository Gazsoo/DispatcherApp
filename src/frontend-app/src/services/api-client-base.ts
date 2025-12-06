import type { AxiosResponse } from "axios";

export abstract class ApiClientBase {

    protected getBaseUrl(url: string): string {
        const baseUrl = import.meta.env.VITE_API_BASE_URL || '';
        return baseUrl + url;
    }

    protected transformResult(
        url: string,
        response: AxiosResponse,
        processor: (response: AxiosResponse) => Promise<any>
    ): Promise<any> {
        void url;
        // Only stringify JSON responses. Do NOT stringify binary responses
        try {
            const contentType = response && response.headers ? response.headers['content-type'] : undefined;
            const isJson = typeof contentType === 'string' && contentType.indexOf('application/json') !== -1;
            const isBlob = typeof Blob !== 'undefined' && response.data instanceof Blob;

            if (response && response.data != null && typeof response.data === 'object' && !isBlob && isJson) {
                // Only stringify when the server returned a JSON payload
                response.data = JSON.stringify(response.data);
            }
        } catch (e) {
            console.warn('transformResult: failed to normalize response data', e);
        }

        return processor(response);
    }
}
