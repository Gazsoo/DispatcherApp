import { useEffect, useState } from "react";
import type { FileMetadataResponse } from "../../services/web-api-client";
import { useApiCall } from "./useApiClient";
import { apiClient } from "../../api/client";



export function useFiles() {
    const { execute, error, isLoading } = useApiCall<FileMetadataResponse[]>();
    const [files, setFiles] = useState<FileMetadataResponse[]>([]);

    useEffect(() => {
        const fetchFiles = async () => {
            const data = await execute(() => apiClient.files_GetFiles());
            if (data) {
                setFiles(data);
            }
        };
        fetchFiles();
    }, [execute]);

    const refetch = async () => {
        const data = await execute(() => apiClient.files_GetFiles());
        if (data) {
            setFiles(data);
        }
    };

    return {
        files,
        isLoading,
        refetch,
        error,
    };
}