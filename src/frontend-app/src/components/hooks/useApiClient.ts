import { useState, useCallback } from 'react';

export function useApiCall<T>() {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);

    const execute = useCallback(async (apiCall: () => Promise<T>): Promise<T | null> => {
        setIsLoading(true);
        setError(null);

        try {
            const result = await apiCall();
            return result;
        } catch (err) {
            const error = err instanceof Error ? err : new Error('An unknown error occurred');
            setError(error);
            console.error('API call failed:', error);
            return null;
        } finally {
            setIsLoading(false);
        }
    }, []);

    const reset = useCallback(() => {
        setError(null);
        setIsLoading(false);
    }, []);

    return { execute, isLoading, error, reset };
}

/**
 * Hook for mutations (create, update, delete operations)
 * Provides additional success state tracking
 */
export function useApiMutation<TInput, TOutput>(apiCall: (input: TInput) => Promise<TOutput>) {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);
    const [isSuccess, setIsSuccess] = useState(false);

    const mutate = useCallback(async (input: TInput): Promise<TOutput | null> => {
        setIsLoading(true);
        setError(null);
        setIsSuccess(false);

        try {
            const result = await apiCall(input);
            setIsSuccess(true);
            return result;
        } catch (err) {
            const error = err instanceof Error ? err : new Error('An unknown error occurred');
            setError(error);
            console.error('API mutation failed:', error);
            return null;
        } finally {
            setIsLoading(false);
        }
    }, [apiCall]);

    const reset = useCallback(() => {
        setError(null);
        setIsLoading(false);
        setIsSuccess(false);
    }, []);

    return { mutate, isLoading, error, isSuccess, reset };
}