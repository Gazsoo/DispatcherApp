import { Link } from "react-router-dom";
import { FileMetadataResponse } from "../../../services/web-api-client";
import { downloadFileById } from "../../../services/file-downloader";

interface FileLinkProps {
    file: FileMetadataResponse;
}
const FileLink = ({ file }: FileLinkProps) => {
    const handleDownload = async (e: React.MouseEvent, fileId?: number, defaultName?: string) => {
        e.preventDefault();
        e.stopPropagation();

        if (fileId === undefined || fileId === null) {
            alert("Invalid file id");
            return;
        }

        try {
            const result = await downloadFileById(fileId, defaultName);
            if (!result.success) {
                alert(`Download failed: ${result.reason}`);
            }
        } catch (err) {
            alert(`Error downloading file: ${err instanceof Error ? err.message : String(err)}`);
        }
    };
    return (
        <Link
            key={file.id}
            to={`/dashboard/files/${file.id}`}
        // className="p-4 border border-gray-200 rounded-lg hover:border-blue-500 hover:shadow-md transition-all"
        >
            <div className="flex items-start justify-between p-4 border border-gray-200 rounded-lg hover:border-blue-500 hover:shadow-md transition-all">
                <div className="flex-1 min-w-0">
                    <h3 className="text-lg font-semibold truncate">{file.fileName}</h3>
                    {/* <p className="text-sm text-gray-500 mt-1">{file.contentType}</p> */}
                </div>
                <div className="flex items-center ml-2 space-x-2">
                    <button
                        className="p-2 rounded hover:bg-gray-100 text-gray-600"
                        title="Download"
                        onClick={(e) => handleDownload(e, file.id, file.fileName)}
                        aria-label={`Download ${file.fileName}`}
                    >
                        <svg className="w-6 h-6 text-gray-800 dark:text-gray-200" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 10 14">
                            <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 1v12m0 0 4-4m-4 4L1 9" />
                        </svg>
                    </button>
                </div>
            </div>
        </Link>)
}

export default FileLink;