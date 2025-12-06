import { useEffect, useMemo, useState } from "react";
import { useApiCall } from "./useApiClient";
import { FileMetadataResponse } from "../../services/web-api-client";
import { apiClient } from "../../api/client";

export function useFileDetails(fileId: string | undefined) {
    const numericFileId = useMemo(() => {
        const parsed = Number(fileId);
        return Number.isFinite(parsed) ? parsed : null;
    }, [fileId]);

    const { execute, isLoading, error } = useApiCall<FileMetadataResponse>();
    const [file, setFile] = useState<FileMetadataResponse | null>(null);

    useEffect(() => {
        if (numericFileId === null) {
            setFile(null);
            return;
        }

        const fetchFile = async () => {
            const data = await execute(() => apiClient.files_GetFileMetadata(numericFileId));
            if (data) {
                setFile(data);
            }
        };

        fetchFile();
    }, [execute, numericFileId]);

    const formattedSize = useMemo(() => {
        if (!file) {
            return "—";
        }
        const units = ["B", "KB", "MB", "GB", "TB"];
        let size = file.fileSize ?? 0;
        let unitIndex = 0;
        while (size >= 1024 && unitIndex < units.length - 1) {
            size /= 1024;
            unitIndex++;
        }
        return `${size.toFixed(1)} ${units[unitIndex]}`;
    }, [file]);

    return { file, formattedSize, isLoading, error };
}