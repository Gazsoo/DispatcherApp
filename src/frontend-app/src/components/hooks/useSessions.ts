import { use, useEffect, useState } from "react";
import { useActivityHub } from "./useActivityHub";
import { useApiCall } from "./useApiClient";
import { SessionResponse } from "../../services/web-api-client";
import { apiClient } from "../../api/client";

export function useSessions() {
    const { sessions, log } = useActivityHub({ hubUrl: "/ws/sessions" });

    useEffect(() => {
        // Hook intentionally returns activity log as-is for consumers.
    }, [log]);

    return { sessions, log } as const;
}

export function useSessionsLog() {
    const { execute, isLoading, error } = useApiCall<SessionResponse[]>();
    const [sessions, setSessions] = useState<SessionResponse[]>([]);
    useEffect(() => {
        // Fetch sessions log from API if needed
        const fetchSessionsLog = async () => {
            const result = await execute(() => apiClient.session_GetAll());
            setSessions(result ?? []);
        };
        fetchSessionsLog();
    }, [execute]);

    return { sessions, isLoading, error } as const;

}
export function useSessionDetails(sessionId: string | undefined) {
    const { execute, isLoading, error } = useApiCall<SessionResponse>();
    const [session, setSession] = useState<SessionResponse | null>(null);
    useEffect(() => {
        if (!sessionId) return;
        const fetchSessionDetails = async () => {
            const result = await execute(() => apiClient.session_Get(sessionId));
            setSession(result);
        };
        fetchSessionDetails();
    }, [execute, sessionId]);

    return { session, isLoading, error } as const;
}