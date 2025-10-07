import { Link, Outlet, useParams } from "react-router-dom";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { LoadingSpinner } from "../../ui/LoadingSpinner";

export default function Files() {
    const { fileId } = useParams();

    // const { files, isLoading, error } = useFiles();

    // if (isLoading) return <LoadingSpinner />;
    // if (error) return <ErrorDisplay error={error} />;

    // Otherwise show the files list
    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <h1 className="text-3xl font-bold">Files</h1>
                <button className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">
                    Upload File
                </button>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {/* {files.map((file) => (
                    <Link
                        key={file.id}
                        to={`/dashboard/files/${file.id}`}
                        className="p-4 border border-gray-200 rounded-lg hover:border-blue-500 hover:shadow-md transition-all"
                    >
                        <div className="flex items-start justify-between">
                            <div className="flex-1 min-w-0">
                                <h3 className="text-lg font-semibold truncate">{file.fileName}</h3>
                                <p className="text-sm text-gray-500 mt-1">{file.contentType}</p>
                            </div>
                            <svg className="w-5 h-5 text-gray-400 flex-shrink-0 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
                            </svg>
                        </div>
                    </Link>
                ))} */}
            </div>
        </div>
    );
}