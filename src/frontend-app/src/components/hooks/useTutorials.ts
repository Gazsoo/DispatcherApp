import { useEffect, useState } from 'react';
import { type TutorialResponse } from '../../services/web-api-client';
import { useApiCall } from './useApiClient';
import { apiClient } from '../../api/client';

export function useTutorials() {
    const { execute, error, isLoading } = useApiCall<TutorialResponse[]>();
    const [tutorials, setTutorials] = useState<TutorialResponse[]>([]);

    useEffect(() => {
        const fetchTutorials = async () => {
            const data = await execute(() => apiClient.tutorial_GetTutorials());
            if (data) {
                setTutorials(data);
            }
        };
        fetchTutorials();
    }, [execute]);

    const refetch = async () => {
        const data = await execute(() => apiClient.tutorial_GetTutorials());
        if (data) {
            setTutorials(data);
        }
    };

    return {
        tutorials,
        isLoading,
        error,
        refetch,
    };
}

export function useTutorial(tutorialId: number | undefined) {
    const { execute, error, isLoading } = useApiCall<TutorialResponse>();
    const [tutorial, setTutorial] = useState<TutorialResponse | null>(null);

    useEffect(() => {
        // Skip API call if ID is invalid
        if (!tutorialId) {
            setTutorial(null);
            return;
        }

        const fetchTutorial = async () => {
            const data = await execute(() => apiClient.tutorial_GetTutorial(tutorialId));
            if (data) {
                setTutorial(data);
            }
        };
        fetchTutorial();
    }, [execute, tutorialId]);

    return {
        tutorial,
        isLoading,
        error
    };
}
