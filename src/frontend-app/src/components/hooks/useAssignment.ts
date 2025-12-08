import { useEffect, useState, useCallback } from "react";
import type { AssignmentResponse } from "../../services/web-api-client";
import { apiClient } from "../../api/client";
import { useApiCall } from "./useApiClient";

export function useAssignment(id?: number) {
    const [assignment, setAssignment] = useState<AssignmentResponse>();
    const { execute, error, isLoading } = useApiCall<AssignmentResponse>();

    const fetchAssignment = useCallback(async () => {
        if (!id) return;
        const response = await execute(() => apiClient.assignment_GetAssignment(id));
        if (response) {
            setAssignment(response);
        }
    }, [execute, id]);
    useEffect(() => {
        fetchAssignment();
    }, [fetchAssignment]);

    return {
        assignment,
        isLoading,
        error,
        refetch: fetchAssignment,
        setAssignment,
    };
}
