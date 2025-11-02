import { useCallback, useEffect, useRef, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { JWTManager } from '../../auth/jwt/jwt-manager';
import type { SessionResponse } from '../../services/web-api-client';

export interface UseActivityHubOptions {
    hubUrl?: string;
    autoJoinActivityGroup?: boolean;
}

export function useActivityHub(options: UseActivityHubOptions = {}) {
    const { hubUrl = '/ws/sessions', autoJoinActivityGroup = true } = options;

    const [log, setLog] = useState<string[]>([]);
    const [sessions, setSessions] = useState<SessionResponse[]>([]);
    const [isConnected, setIsConnected] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const activityConnRef = useRef<signalR.HubConnection | null>(null);

    const effectiveToken = JWTManager.getAccessToken() ?? '';

    const append = useCallback((line: string) => {
        setLog((x) => [...x, `[${new Date().toISOString()}] ${line}`]);
    }, []);



    useEffect(() => {
        let mounted = true;

        const startActivityConnection = async () => {
            if (activityConnRef.current) return;

            const url = (import.meta as any).env.VITE_API_BASE_URL + hubUrl;
            const conn = new signalR.HubConnectionBuilder()
                .withUrl(url, { accessTokenFactory: () => effectiveToken || '' })
                .withAutomaticReconnect()
                .build();

            conn.on('ActivityUpdate', (msg: { sessions: SessionResponse[] }) => {
                append(`[activity] ActivityUpdate: ${JSON.stringify(msg)}`);
                if (!mounted) return;
                setSessions(msg.sessions || []);
            });

            conn.onclose((err) => {
                setIsConnected(false);
                if (err) setError(err.message);
                append(`[activity] Closed${err ? ' - ' + err.message : ''}`);
            });

            try {
                await conn.start();
                activityConnRef.current = conn;
                setIsConnected(true);
                append('[activity] Connected');

                if (autoJoinActivityGroup) {
                    try {
                        const result = await conn.invoke('JoinActivityGroup');
                        append(`[activity] JoinActivityGroup result: ${JSON.stringify(result)}`);
                        if (mounted && Array.isArray(result)) setSessions(result as SessionResponse[]);
                        append('[activity] Joined activity group');
                    } catch (err) {
                        append(
                            `[activity] JoinActivityGroup failed (maybe not implemented on server): ${(err as Error).message}`
                        );
                    }
                }
            } catch (err) {
                setError((err as Error).message);
                append(`[activity] Failed to start: ${(err as Error).message}`);
            }
        };

        startActivityConnection();

        return () => {
            mounted = false;
            const c = activityConnRef.current;
            if (c) {
                c.stop().catch(() => {
                    /* ignore */
                });
                activityConnRef.current = null;
            }
        };
    }, [hubUrl, effectiveToken, append, autoJoinActivityGroup]);

    return { sessions, log, isConnected, error } as const;
}
