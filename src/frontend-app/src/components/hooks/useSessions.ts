import { useEffect } from "react";
import { useActivityHub } from "./useActivityHub";

export function useSessions() {
    const { sessions, log } = useActivityHub({ hubUrl: "/ws/sessions" });

    useEffect(() => {
        // Hook intentionally returns activity log as-is for consumers.
    }, [log]);

    return { sessions, log } as const;
}
