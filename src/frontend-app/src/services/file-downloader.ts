import { apiClient } from "../api/client";

export type DownloadResult =
    | { success: true; filename: string }
    | { success: false; reason: string; details?: string };

export async function downloadFileById(fileId: number, fallback?: string): Promise<DownloadResult> {
    if (fileId == null) return { success: false, reason: "invalid-id" };

    try {
        const resp = await apiClient.files_DownloadFile(fileId);
        const anyResp = resp as any;

        // Generated client returns { data: Blob, fileName?, status?, headers? }
        if (!anyResp || !anyResp.data) {
            // try to provide diagnostics
            const status = anyResp?.status ?? 'unknown';
            const headers = anyResp?.headers ? JSON.stringify(anyResp.headers) : undefined;
            return { success: false, reason: `no-data-status-${status}`, details: headers };
        }

        const blob: Blob = anyResp.data;
        const filename = anyResp.fileName || fallback || `file-${fileId}`;

        const objectUrl = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = objectUrl;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        a.remove();
        setTimeout(() => URL.revokeObjectURL(objectUrl), 1500);

        return { success: true, filename };
    } catch (err: any) {
        return { success: false, reason: 'exception', details: String(err) };
    }
}

export default downloadFileById;
