import type { FileMetadataResponse } from "../../../services/web-api-client";

interface ExistingFilesSelectorProps {
    files: FileMetadataResponse[];
    selectedFileIds: number[];
    onToggle: (fileId: number) => void;
    title?: string;
    emptyMessage?: string;
}

const formatFileSize = (bytes?: number) => {
    if (!bytes) {
        return "";
    }
    const units = ["B", "KB", "MB", "GB", "TB"];
    let size = bytes;
    let idx = 0;
    while (size >= 1024 && idx < units.length - 1) {
        size /= 1024;
        idx += 1;
    }
    const decimals = size < 10 && idx > 0 ? 1 : 0;
    return `${size.toFixed(decimals)} ${units[idx]}`;
};

const isSelected = (selectedFileIds: number[], fileId?: number) =>
    typeof fileId === "number" && selectedFileIds.includes(fileId);

export function ExistingFilesSelector({
    files,
    selectedFileIds,
    onToggle,
    title = "Use existing files",
    emptyMessage = "No files found. Upload new files above or visit the Files page to add some.",
}: ExistingFilesSelectorProps) {
    return (
        <div>
            {title && <p className="block text-sm font-medium mb-2">{title}</p>}
            {files.length === 0 ? (
                <p className="text-sm text-gray-500 dark:text-gray-400">
                    {emptyMessage}
                </p>
            ) : (
                <div className="max-h-64 overflow-y-auto border border-surface-light-border dark:border-surface-dark-border rounded-lg divide-y divide-surface-light-border dark:divide-surface-dark-border">
                    {files.map((file) => (
                        <label
                            key={file.id ?? file.fileName}
                            className={`flex items-center justify-between gap-3 px-3 py-2 text-sm cursor-pointer transition-colors bg-surface-light dark:bg-surface-dark hover:bg-surface-light dark:hover:bg-surface-dark ${isSelected(selectedFileIds, file.id) ? "ring-1 ring-accent bg-accent/10 dark:bg-accent/10" : ""}`}
                        >
                            <div className="flex items-center gap-3">
                                <input
                                    type="checkbox"
                                    className="h-4 w-4 text-accent border-gray-300 rounded focus:ring-0"
                                    checked={isSelected(selectedFileIds, file.id)}
                                    onChange={() => typeof file.id === "number" && onToggle(file.id)}
                                />
                                <div>
                                    <p className="font-medium">{file.fileName ?? "Untitled file"}</p>
                                    <p className="text-xs text-gray-500 dark:text-gray-400">
                                        {file.description || file.contentType || "No description"}
                                    </p>
                                </div>
                            </div>
                            <span className="text-xs text-gray-500">{formatFileSize(file.fileSize) || "—"}</span>
                        </label>
                    ))}
                </div>
            )}
        </div>
    );
}
