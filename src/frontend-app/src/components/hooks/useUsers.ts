import { useEffect, useState, useCallback } from "react";
import type { UserInfoResponse, GetAllUsersResponse } from "../../services/web-api-client";
import { apiClient } from "../../api/client";
import { useApiCall } from "./useApiClient";

export function useUsers() {
    const [users, setUsers] = useState<UserInfoResponse[]>([]);
    const { execute, error, isLoading } = useApiCall<GetAllUsersResponse>();

    const fetchUsers = useCallback(async () => {
        const response = await execute(() => apiClient.user_GetAllUsers());
        if (response?.users) {
            setUsers(response.users);
        }
    }, [execute]);

    useEffect(() => {
        fetchUsers();
    }, [fetchUsers]);

    return {
        users,
        isLoading,
        error,
        refetch: fetchUsers,
    };
}
