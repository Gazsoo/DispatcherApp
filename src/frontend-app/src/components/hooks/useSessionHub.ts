import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { JWTManager } from '../../auth/jwt/jwt-manager';
import { DispatcherSessionStatus, ParticipantDto, SessionResponse } from '../../services/web-api-client';
import { Session } from 'react-router-dom';



export function useSessionHub(groupId: string | undefined, hubUrl?: string) {
    const [isConnected, setIsConnected] = useState(false);
    const [sessionStatus, setSessionStatus] = useState<DispatcherSessionStatus>();
    const [sessionParticipants, setSessionParticipants] = useState<ParticipantDto[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [log, setLog] = useState<string[]>([]);

    const effectiveToken = JWTManager.getAccessToken() ?? '';
    const itemRef = useRef<signalR.HubConnection | null>(null);

    const append = useCallback((line: string) => {
        setLog((x) => [...x, `[${new Date().toISOString()}] ${line}`]);
    }, []);

    // Expose the current connection (if any) for advanced usage
    const connection = useMemo(() => itemRef.current ?? null, [itemRef.current]);

    useEffect(() => {
        if (!groupId) {
            setError('groupId is required');
            return;
        }

        let isMounted = true;

        const ensureConnection = async () => {
            if (!connection) {
                const url = (import.meta as any).env.VITE_API_BASE_URL + hubUrl;
                const conn = new signalR.HubConnectionBuilder()
                    .withUrl(url, { accessTokenFactory: () => effectiveToken || '' })
                    .withAutomaticReconnect()
                    .build();

                // store connection in ref
                itemRef.current = conn;

                conn.onclose((err) => {
                    if (!isMounted) return;
                    setIsConnected(false);
                    if (err) setError(err.message);
                    append(`[session:${groupId}] Closed${err ? ' - ' + err.message : ''}`);
                });

                conn.onreconnected(async () => {
                    append(`[session:${groupId}] Reconnected, re-joining session`);
                    try {
                        await conn.invoke('JoinSession', groupId);
                        append(`[session:${groupId}] Re-joined session`);
                        if (!isMounted) return;
                        setIsConnected(true);
                    } catch (e) {
                        append(`[session:${groupId}] Re-join failed: ${(e as Error).message}`);
                    }
                });

                conn.on('SessionUpdated', (msg: SessionResponse) => {
                    append(`[session:${groupId}] SessionUpdated: ${JSON.stringify(msg)}`);
                    if (!isMounted) return;
                    setSessionParticipants(msg.participants ?? []);
                    setSessionStatus(msg.status);
                });
                try {
                    await conn.start();
                    if (!isMounted) return;
                    append(`[session:${groupId}] Connected`);
                    await conn.invoke('JoinSession', groupId);
                    append(`[session:${groupId}] Joined session`);
                    setIsConnected(true);
                } catch (e) {
                    if (!isMounted) return;
                    const msg = (e as Error).message;
                    setError(msg);
                    append(`[session:${groupId}] Failed to start or join: ${msg}`);
                }
            }

            // set connection state for consumer
            const stateConnected = itemRef.current?.state === signalR.HubConnectionState.Connected;
            setIsConnected(stateConnected);
        };

        ensureConnection();

        return () => {
            isMounted = false;
            const item = itemRef.current;
            if (!item) return;

            item.stop().catch(() => { /* ignore */ });

            itemRef.current = null;
        };
    }, [groupId, hubUrl, effectiveToken, append]);

    return { connection, isConnected, error, log, sessionStatus, sessionParticipants } as const;
}
