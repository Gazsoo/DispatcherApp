import { useEffect, useState } from "react";
import type { UserInfoResponse } from "../../services/web-api-client";
import { apiClient } from "../../api/client";
import { useApiCall } from "./useApiClient";

export function useUser(userId?: string) {
    const [user, setUser] = useState<UserInfoResponse | null>(null);
    const { execute, error, isLoading } = useApiCall<UserInfoResponse>();

    useEffect(() => {
        if (!userId) {
            setUser(null);
            return;
        }

        const fetchUser = async () => {
            const data = await execute(() => apiClient.user_GetUser(userId));
            if (data) {
                setUser(data);
            }
        };

        fetchUser();
    }, [execute, userId]);

    return {
        user,
        isLoading,
        error,
    };
}
